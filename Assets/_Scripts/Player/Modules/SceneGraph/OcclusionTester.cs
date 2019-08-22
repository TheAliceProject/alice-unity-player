using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alice.Player.Unity
{
    public class OcclusionTester : MonoBehaviour
    {
        public SGModel[] objectsToWatch;

        private Plane castPlane;
        private float planeDistance = 20f;
        private UnityEngine.Vector3 planePos;

        private List<BBPoints> bbPoints = new List<BBPoints>();
        private List<BBPoints> planePoints = new List<BBPoints>();
        private List<GameObject> planeColliderObjects = new List<GameObject>();
        // Start is called before the first frame update
        void Start()
        {
            planePos = transform.position + transform.forward * planeDistance;
            castPlane = new Plane(-transform.forward, planePos);
        }

        // Update is called once per frame
        void Update()
        {
            planePos = transform.position + transform.forward * planeDistance;
            castPlane.SetNormalAndPosition(-transform.forward, planePos);

            if(objectsToWatch != null)
            {
                while (bbPoints.Count < objectsToWatch.Length)
                {
                    bbPoints.Add(new BBPoints());
                }

                for (int i = 0; i < objectsToWatch.Length; i++)
                {
                    Bounds bounds = objectsToWatch[i].GetBounds(true);
                    bounds.center = objectsToWatch[i].transform.position + new Vector3(0f, 0.5f, 0f);

                    bbPoints[i].points[0] = bounds.min;
                    bbPoints[i].points[1] = bounds.max;
                    bbPoints[i].points[2] = new UnityEngine.Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
                    bbPoints[i].points[3] = new UnityEngine.Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
                    bbPoints[i].points[4] = new UnityEngine.Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
                    bbPoints[i].points[5] = new UnityEngine.Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
                    bbPoints[i].points[6] = new UnityEngine.Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
                    bbPoints[i].points[7] = new UnityEngine.Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
                }
            }

            planePoints.Clear();
            for (int i = 0; i < bbPoints.Count; i++)
            {
                BBPoints thisPoints = new BBPoints();
                for (int j = 0; j < bbPoints[i].points.Count; j++)
                {
                    Ray ray = new Ray(transform.position, transform.position - bbPoints[i].points[j]);
                    float enter = 0;
                    castPlane.Raycast(ray, out enter);
                    thisPoints.points[j] = ray.GetPoint(enter); // So points are not coplanar
                }
                planePoints.Add(thisPoints);
            }

            //DrawPlane(castPlane, planePoints);
            CheckPointIntersections();
        }

        void CheckPointIntersections(){
            // For every object
            for(int i = 0; i < planePoints.Count; i++)
            {
                // Compare to every other object
                for(int j = 0; j < planePoints.Count; j++)
                {
                    if(j <= i)  // We've already compared these
                        continue;
                    
                    // For every point / line segment in object 1
                    for(int k = 0; k < planePoints[i].points.Count; k++) // Always 8
                    {
                        for(int l = 0; l < planePoints[i].points.Count; l++)
                        {
                            if(k == l)
                                continue;

                            // For every point in object 2
                            for(int m = 0; m < planePoints[j].points.Count; m++)
                            {
                                for(int n = 0; n < planePoints[j].points.Count; n++)
                                {
                                    if(m == n)
                                        continue;
                                    
                                    Vector3 intersection;
                                    LineLineIntersection(out intersection, planePoints[i].points[k], planePoints[i].points[l],
                                                                    planePoints[j].points[m], planePoints[j].points[n]);
                                    if((SqDistancePtSegment(planePoints[i].points[k], planePoints[i].points[l], intersection) < 0.01f) && 
                                        (SqDistancePtSegment(planePoints[j].points[m], planePoints[j].points[n], intersection) < 0.01f)) 
                                    {
                                        Debug.DrawLine(planePoints[i].points[k], planePoints[i].points[l], Color.red);
                                        Debug.DrawLine(planePoints[j].points[m], planePoints[j].points[n], Color.red);
                                    }
                                    //else
                                   // {
                                    //    Debug.DrawLine(planePoints[i].points[k], planePoints[i].points[l], Color.yellow);
                                    //    Debug.DrawLine(planePoints[j].points[m], planePoints[j].points[n], Color.yellow);
                                    //}
                                    
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

            //is coplanar, and not parrallel
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

            public BBPoints()
            {
                points = new List<UnityEngine.Vector3>();
                for (int i = 0; i < 8; i++)
                {
                    points.Add(Vector3.zero);
                }
            }

            public void AddPoint(UnityEngine.Vector3 point)
            {
                points.Add(point);
            }
        }

    } 
}
