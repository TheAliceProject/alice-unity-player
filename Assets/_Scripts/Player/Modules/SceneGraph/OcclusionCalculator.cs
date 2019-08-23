using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;

namespace Alice.Player.Unity
{
    public class OcclusionCalculator : MonoBehaviour
    {
        private Plane castPlane = new Plane(Vector3.zero, 0f);
        private float planeDistance = 20f;
        private UnityEngine.Vector3 planePos;
        public List<SGModel> objectsToWatch = new List<SGModel>();
        private BBPoints boundingBoxPoints = new BBPoints();
        private List<BBPoints> projectedOnPlane = new List<BBPoints>();
        private List<GameObject> planeColliderObjects = new List<GameObject>();

        private const float MAX_PLANE_DISTANCE = 200f;

        // Update is called once per frame
        void Update()
        {
            // Don't bother with this loop if we aren't watching any models for occlusion
            if(objectsToWatch.Count <= 0)
                return;

            // Get max distance for our plane
            float maxDistance = 0f;
            for (int i = 0; i < objectsToWatch.Count; i++){
                maxDistance = Mathf.Max(maxDistance, Vector3.Distance(transform.position, objectsToWatch[i].transform.position));
            }
            if(maxDistance > MAX_PLANE_DISTANCE)
                maxDistance = MAX_PLANE_DISTANCE;
            
            // Make our plane on which we will cast bounding box corners to check for occlusion
            planePos = transform.position + transform.forward * (maxDistance + 10f);
            castPlane.SetNormalAndPosition(-transform.forward, planePos);

            // Get our bounding box points
            projectedOnPlane.Clear();
            for (int i = 0; i < objectsToWatch.Count; i++)
            {
                Bounds bounds = objectsToWatch[i].GetBounds(true);
                bounds.center = objectsToWatch[i].transform.position + new Vector3(0f, 0.5f, 0f);

                boundingBoxPoints.points[0] = bounds.min;
                boundingBoxPoints.points[1] = bounds.max;
                boundingBoxPoints.points[2] = new UnityEngine.Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
                boundingBoxPoints.points[3] = new UnityEngine.Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
                boundingBoxPoints.points[4] = new UnityEngine.Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
                boundingBoxPoints.points[5] = new UnityEngine.Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
                boundingBoxPoints.points[6] = new UnityEngine.Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
                boundingBoxPoints.points[7] = new UnityEngine.Vector3(bounds.max.x, bounds.max.y, bounds.min.z);

                // Project bounding box points to plane
                BBPoints thisPoints = new BBPoints();
                thisPoints.SetModel(objectsToWatch[i]);

                for (int j = 0; j < boundingBoxPoints.points.Count; j++)
                {
                    Ray ray = new Ray(transform.position, transform.position - boundingBoxPoints.points[j]);
                    float enter = 0;
                    castPlane.Raycast(ray, out enter);
                    thisPoints.points[j] = ray.GetPoint(enter); // So points are not coplanar
                }
                projectedOnPlane.Add(thisPoints);
            }

            // Calculate if any line segments intersect
            DrawPlane(castPlane, projectedOnPlane);
            CheckPointIntersections(projectedOnPlane);
        }

        public void AddOcclusionModels(SGModel[] modelsToAdd)
        {
            for (int i = 0; i < modelsToAdd.Length; i++){
                objectsToWatch.Add(modelsToAdd[i]);
            }
        }

        void CheckPointIntersections(List<BBPoints> planePoints){
            // For every object
            for(int i = 0; i < planePoints.Count; i++) 
            {
                // Compare to every other object
                for(int j = 0; j < planePoints.Count; j++)
                {
                    if(j <= i)  // We've already compared these
                        continue;
                    
                    // For every point / line segment in object 1
                    for(int k = 0; k < planePoints[i].points.Count; k++) // Maximum of 8 points using bounding box
                    {
                        for(int l = 0; l < planePoints[i].points.Count; l++) // Average of 4 iterations
                        {
                            if(k == l)
                                continue;

                            // For every point / line segment in object 2
                            for(int m = 0; m < planePoints[j].points.Count; m++) // Maximum of 8 points using bounding box
                            {
                                for(int n = 0; n < planePoints[j].points.Count; n++) // Average of 4 iterations
                                {
                                    if(m == n)
                                        continue;
                                    
                                    Vector3 intersection;
                                    LineLineIntersection(out intersection, planePoints[i].points[k], planePoints[i].points[l],
                                                                    planePoints[j].points[m], planePoints[j].points[n]);
                                    if((SqDistancePtSegment(planePoints[i].points[k], planePoints[i].points[l], intersection) < 0.01f) && 
                                        (SqDistancePtSegment(planePoints[j].points[m], planePoints[j].points[n], intersection) < 0.01f)) 
                                    {
                                        // Found occlusion! 
                                        Debug.DrawLine(planePoints[i].points[k], planePoints[i].points[l], Color.red);
                                        Debug.DrawLine(planePoints[j].points[m], planePoints[j].points[n], Color.red);
                                        SGModel foregroundModel, backgroundModel;
                                        if(Vector3.Distance(transform.position, planePoints[j].associatedModel.transform.position) > Vector3.Distance(transform.position, planePoints[i].associatedModel.transform.position))
                                        {
                                            foregroundModel = planePoints[i].associatedModel;
                                            backgroundModel = planePoints[j].associatedModel;
                                        }
                                        else
                                        {
                                            foregroundModel = planePoints[j].associatedModel;
                                            backgroundModel = planePoints[i].associatedModel;
                                        }
                                             
                                        SceneGraph.Current.Scene.ObjectsOccluded(foregroundModel, backgroundModel);
                                        break;
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }
        }

        float SqDistancePtSegment( Vector3 a, Vector3 b, Vector3 p )
        {
            Vector3 n = b - a;
            Vector3 pa = a - p;

            float c = Vector3.Dot( n, pa );

            // Closest point is a
            if ( c > 0.0f )
                return Vector3.Dot( pa, pa );

            Vector3 bp = p - b;

            // Closest point is b
            if ( Vector3.Dot( n, bp ) > 0.0f )
                return Vector3.Dot( bp, bp );

            // Closest point is between a and b
            Vector3 e = pa - n * (c / Vector3.Dot( n, n ));

            return Vector3.Dot( e, e );
        }

        public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 linePoint1b, Vector3 linePoint2, Vector3 linePoint2b, bool stop = false)
        {

            Vector3 lineVec1 = linePoint1 - linePoint1b;
            Vector3 lineVec2 = linePoint2 - linePoint2b;
            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //is coplanar, and not parallel
            if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }
            else
            {
                if(stop){
                    intersection = Vector3.zero;
                    return false;
                }
                else{
                    return LineLineIntersection(out intersection, linePoint1b, linePoint1, linePoint2b, linePoint2, true);
                }
            }
        }

/*
        void OnDrawGizmos()
        {
            Gizmos.DrawSphere(planePos, 0.5f);
            Gizmos.color = Color.blue;
            for (int i = 0; i < bbPoints.Count; i++){
                for (int j = 0; j < bbPoints[i].points.Count; j++){
                    Gizmos.DrawCube(bbPoints[i].points[j], new Vector3(0.1f, 0.1f, 0.1f));
                }
            }
        }
*/

        void DrawPlane(Plane p, List<BBPoints> planePoints)
        {
            UnityEngine.Vector3 v3;
            UnityEngine.Vector3 position = planePos;
            if (p.normal.normalized != Vector3.forward)
                v3 = Vector3.Cross(p.normal, Vector3.forward).normalized * p.normal.magnitude * 5f;
            else
                v3 = Vector3.Cross(p.normal, Vector3.up).normalized * p.normal.magnitude * 5f;

            var corner0 = position + v3;
            var corner2 = position - v3;
            var q = Quaternion.AngleAxis(90.0f, p.normal);
            v3 = q * v3;
            var corner1 = position + v3;
            var corner3 = position - v3;

            Debug.DrawLine(corner0, corner2, Color.green);
            Debug.DrawLine(corner1, corner3, Color.green);
            Debug.DrawLine(corner0, corner1, Color.green);
            Debug.DrawLine(corner1, corner2, Color.green);
            Debug.DrawLine(corner2, corner3, Color.green);
            Debug.DrawLine(corner3, corner0, Color.green);
            Debug.DrawRay(position, p.normal, Color.red);

            for (int i = 0; i < planePoints.Count; i++)
            {
                for (int j = 0; j < planePoints[i].points.Count; j++)
                {
                    for (int k = 0; k < planePoints[i].points.Count; k++){
                        Debug.DrawLine(planePoints[i].points[j], planePoints[i].points[k], Color.yellow);
                    }
                }
            }
        }

        // Bounding box points, simply a list of 8 points.
        class BBPoints
        {
            public List<UnityEngine.Vector3> points;
            public SGModel associatedModel;
            public BBPoints()
            {
                points = new List<UnityEngine.Vector3>();
                for (int i = 0; i < 8; i++)
                {
                    points.Add(Vector3.zero);
                }
            }
            public void SetModel(SGModel model)
            {
                associatedModel = model;
            }
        }

    } 
}
