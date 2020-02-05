using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;

namespace Alice.Player.Unity
{
    public class OcclusionCalculator : MonoBehaviour
    {
        private Plane castPlane = new Plane(Vector3.zero, 0f);
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
            for (int i = 0; i < objectsToWatch.Count; i++){
                Bounds bounds = objectsToWatch[i].GetBoundsInWorldSpace(true);

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

                for (int j = 0; j < boundingBoxPoints.points.Count; j++){
                    boundingBoxPoints.points[j] = RotatePointAroundPivot(boundingBoxPoints.points[j], objectsToWatch[i].transform.position, new Vector3(0f, objectsToWatch[i].transform.eulerAngles.y, 0f));
                    Ray ray = new Ray(transform.position, transform.position - boundingBoxPoints.points[j]);
                    float enter = 0;
                    castPlane.Raycast(ray, out enter);
                    thisPoints.points[j] = ray.GetPoint(enter); // So points are not coplanar
                }
                projectedOnPlane.Add(thisPoints);
            }

            // Calculate if any line segments intersect
            CheckPointIntersections(projectedOnPlane);
        }

        public void AddOcclusionModels(SGModel[] modelsToAdd)
        {
            for (int i = 0; i < modelsToAdd.Length; i++){
                objectsToWatch.Add(modelsToAdd[i]);
            }
        }

        void CheckPointIntersections(List<BBPoints> planePoints){
            // The whole goal of this is to check if one object is occluding another in relation to the camera. I am doing this by
            // projecting the bounding box of the objects onto a plane, and checking if any line from one object intersects a line from
            // any other object in the list. 
            //
            // Ways we could improve detecting occlusion:
            // - Figure out some other way to do it in general (using raycasting? having the graphics card check which one is closer?)
            // - Only check the 'outline' of the object on the plane instead of every point. But we'd need to run a convex hull algorithm first
            // - Maybe we can break out early?

            // For every object
            for(int i = 0; i < planePoints.Count; i++){ // Hopefully they don't have TOO many checks for occluding objects.
                // Compare to every other object
                for(int j = 0; j < planePoints.Count; j++){
                    bool breakOut = false; // We want to break out of the loop and move on to the next object asap if we found occlusion
                    if(j <= i)  // We've already compared these
                        continue;
                    
                    // For every point / line segment in object 1
                    for(int k = 0; k < planePoints[i].points.Count; k++){ // Maximum of 8 points using bounding box
                        for(int l = 0; l < planePoints[i].points.Count; l++){ // Average of 4 iterations
                            if(k == l)
                                continue;

                            // For every point / line segment in object 2
                            for(int m = 0; m < planePoints[j].points.Count; m++){ // Maximum of 8 points using bounding box
                                for(int n = 0; n < planePoints[j].points.Count; n++){ // Average of 4 iterations
                                    if(m == n)
                                        continue;
                                    
                                    Vector3 intersection;
                                    LineLineIntersection(out intersection, planePoints[i].points[k], planePoints[i].points[l],
                                                                    planePoints[j].points[m], planePoints[j].points[n]);
                                    if((SqDistancePtSegment(planePoints[i].points[k], planePoints[i].points[l], intersection) < 0.01f) && 
                                        (SqDistancePtSegment(planePoints[j].points[m], planePoints[j].points[n], intersection) < 0.01f)) 
                                    {
                                        // Keep these in for future debuggings
                                        //Debug.DrawLine(planePoints[i].points[k], planePoints[i].points[l], Color.red);
                                        //Debug.DrawLine(planePoints[j].points[m], planePoints[j].points[n], Color.red);

                                        // Found occlusion! See which is in front
                                        if(Vector3.Distance(transform.position, planePoints[j].associatedModel.transform.position) > Vector3.Distance(transform.position, planePoints[i].associatedModel.transform.position)){
                                            SceneGraph.Current.Scene.ObjectsOccluded(planePoints[i].associatedModel, planePoints[j].associatedModel);
                                        }
                                        else{
                                            SceneGraph.Current.Scene.ObjectsOccluded(planePoints[j].associatedModel, planePoints[i].associatedModel);
                                        }
                                        // We know we've occluded, so stop comparing these objects
                                        breakOut = true;
                                        break;
                                    }
                                    
                                }
                                if(breakOut)
                                    break;
                            }
                            if(breakOut)
                                break;
                        }
                        if(breakOut)
                            break;
                    }
                    if(breakOut)
                        break;
                }
            }
        }

        public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
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
            if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f){
                float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }
            else{
                if(stop){
                    intersection = Vector3.zero;
                    return false;
                }
                else{
                    return LineLineIntersection(out intersection, linePoint1b, linePoint1, linePoint2b, linePoint2, true);
                }
            }
        }

        // Bounding box points, simply a list of 8 points.
        class BBPoints
        {
            public List<UnityEngine.Vector3> points;
            public SGModel associatedModel;
            public BBPoints(){
                points = new List<UnityEngine.Vector3>();
                for (int i = 0; i < 8; i++){
                    points.Add(Vector3.zero);
                }
            }
            public void SetModel(SGModel model){
                associatedModel = model;
            }
        }
    } 
}
