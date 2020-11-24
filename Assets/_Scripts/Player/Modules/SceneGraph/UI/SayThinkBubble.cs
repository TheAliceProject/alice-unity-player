using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using TMPro;
using BeauRoutine;

namespace Alice.Player.Unity {

    public class SayThinkBubble : MonoBehaviour
    {
        // Maybe don't need these things since the say / think stuff can't change dynamically?
        public const float BASE_FONT_SIZE = 12f;
        private const float FONT_WIDTH_SCALE = 750f; // Font size is scaled based off this screen width
        [SerializeField]
        private bool isSay = false;
        [SerializeField]
        private TextMeshProUGUI bubbleText = null;
        [SerializeField]
        private Image bubbleBackground = null;
        [SerializeField]
        private Image bubbleOutline = null;
        [SerializeField]
        private Image[] tailBackground = null;
        [SerializeField]
        private Image[] tailOutline = null;
        [SerializeField]
        private RectTransform tailPivot = null;

        private SayThinkControl sayThinkControlRef;
        private Routine spawnRoutine = Routine.Null;
        private Routine tailRoutine = Routine.Null;
        private Transform speechOrigin = null;
        private float fontSizeUnscaled = 12f;

        void Start()
        {
            RectTransform rectTrans = gameObject.transform as RectTransform;
            rectTrans.localScale = Vector2.zero;
        }
        public void SetColor(UnityEngine.Color bubbleColor, UnityEngine.Color outlineColor){
            bubbleBackground.color = bubbleColor;
            bubbleOutline.color = outlineColor;
            if(tailBackground.Length != tailOutline.Length){
                Debug.LogError("Tail and outline not the same length!");
                return;
            }

            for(int i = 0; i < tailBackground.Length; i++){
                tailBackground[i].color = bubbleColor;
                tailOutline[i].color = outlineColor;
            }
            
        }

        public void SetText(string text, UnityEngine.Color c, TMP_FontAsset font, float scale){
            bubbleText.text = text;
            bubbleText.color = c;
            bubbleText.font = font;
            fontSizeUnscaled = BASE_FONT_SIZE * scale;
            if(!VRControl.IsLoadedInVR())
                bubbleText.fontSize = fontSizeUnscaled * (Screen.width / FONT_WIDTH_SCALE);
        }

        public void SetTextStyle(TextStyle textStyle){
            if(textStyle == TextStyle.Plain)
                bubbleText.fontStyle = FontStyles.Normal;
            else if(textStyle == TextStyle.Bold)
                bubbleText.fontStyle = FontStyles.Bold;
            else if(textStyle == TextStyle.Italic)
                bubbleText.fontStyle = FontStyles.Italic;
        }


        public void Spawn(SGEntity entity, float duration, SayThinkControl sayThink){
            sayThinkControlRef = sayThink;
            spawnRoutine.Replace(this, SayThinkRoutine(gameObject.transform as RectTransform, entity, duration));
        }

        private IEnumerator SayThinkRoutine(RectTransform trans, SGEntity entity, float duration){
            float startEndAnimationTime;
            if (duration < 1.0f)
            {
                startEndAnimationTime = 0.2f * duration;
            }
            else {
                startEndAnimationTime = 0.2f;
            }
            bubbleText.transform.SetScale(1f, Axis.X);
            tailRoutine.Replace(this, AlignTailRoutine(entity));
            yield return (trans as Transform).ScaleTo(new UnityEngine.Vector3(1f, 1f, 1f), startEndAnimationTime, Axis.XYZ);
            yield return duration - 2 * startEndAnimationTime;
            yield return trans.ScaleTo(0f, startEndAnimationTime, Axis.XY);
            sayThinkControlRef.DestroyBubble(this);
        }

        private IEnumerator AlignTailRoutine(SGEntity entity){
            while(true){
                if(speechOrigin == null)
                    GetMouthPosition(entity.cachedTransform);
                UnityEngine.Vector3 objectPos = UnityEngine.Vector3.zero;
                if (isSay)
                    objectPos = GetSpeechBubbleOffset(entity.cachedTransform);
                else
                    objectPos = GetThoughtBubbleOffset(entity.cachedTransform);
                Debug.DrawRay(objectPos, UnityEngine.Vector3.forward);
                float tailRotation = 0f;
                float tailLength = 0f;

                if(VRControl.IsLoadedInVR()){
                    var screenPoint = Camera.main.WorldToScreenPoint(objectPos); // convert target's world space position to a screen position
                    UnityEngine.Vector2 objectScreenPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle((this.transform as RectTransform), screenPoint, Camera.main, out objectScreenPoint);
                    tailLength = Vector2.Distance(objectScreenPoint, tailPivot.position);
                    tailRotation = 180f + (Mathf.Rad2Deg * Mathf.Atan((objectScreenPoint.x - tailPivot.position.x) / (tailPivot.position.y - objectScreenPoint.y))) + (objectScreenPoint.y < tailPivot.position.y ? 180f : 0f);
                }
                else{
                    bubbleText.fontSize = fontSizeUnscaled * (Screen.width / FONT_WIDTH_SCALE);
                    var objectScreenPoint = Camera.main.WorldToScreenPoint(objectPos); // convert target's world space position to a screen position
                    tailLength = Vector2.Distance(objectScreenPoint, tailPivot.position); // -10f for some buffer space away from mouth 
                    tailRotation = 180f + (Mathf.Rad2Deg * Mathf.Atan((objectScreenPoint.x - tailPivot.position.x) / (tailPivot.position.y - objectScreenPoint.y))) + (objectScreenPoint.y < tailPivot.position.y ? 180f : 0f);
                }

                tailPivot.SetRotation(tailRotation, Axis.Z, Space.Self);
                if(isSay){
                    // Stretch tail to object
                    Vector2 tailSizeDelta = tailOutline[0].rectTransform.sizeDelta;
                    Vector2 tailPos = tailOutline[0].rectTransform.anchoredPosition;
                    tailSizeDelta.y = tailLength;
                    tailOutline[0].rectTransform.sizeDelta = tailSizeDelta;
                }
                else{
                    var objectScreenPoint = Camera.main.WorldToScreenPoint(objectPos);
                    for (int i = 0; i < tailOutline.Length; i++)
                    {
                        Debug.Log(objectScreenPoint);
                        tailOutline[i].rectTransform.position = tailPivot.position + (objectScreenPoint - tailPivot.position) * ((float)(i + 1) / (float)tailOutline.Length);
                    }
                    //float lastTailPos = 150f;
                    //if(tailPivot.localEulerAngles.z < 35f || (tailPivot.localEulerAngles.z > 145f && tailPivot.localEulerAngles.z < 225f) || tailPivot.localEulerAngles.z > 315f)
                    //    lastTailPos = 80f;
                    //for(int i = 0; i < tailOutline.Length; i++){
                    //    Vector2 tailPos = tailOutline[i].rectTransform.anchoredPosition;
                    //    tailPos.y = -(lastTailPos + ((tailLength * (i + 1)) / tailOutline.Length));
                    //    tailOutline[i].rectTransform.anchoredPosition = tailPos;
                    //}
                }
                yield return null;
            }
        }

        private void GetMouthPosition(Transform parent)
        {
            // Best case scenario, we find a lower lip bone
            speechOrigin = FindDeepChild(parent, "LOWER_LIP");
            if(speechOrigin != null)
                return;
            
            speechOrigin = FindDeepChild(parent, "MOUTH");
            if(speechOrigin != null)
                return;

            // Default to parent 
            speechOrigin = parent;
            return;
        }

        private Transform FindDeepChild(Transform parent, string aName)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(parent);
            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                if (c.name == aName)
                    return c;
                foreach(Transform t in c)
                    queue.Enqueue(t);
            }
            return null;
        }

        private UnityEngine.Vector3 GetThoughtBubbleOffset(Transform parent)
        {
            UnityEngine.Vector3 topOffset = UnityEngine.Vector3.zero;

            // Find the valuable mesh
            // The biggest skinned mesh
            // or the whole mesh
            GameObject mesh = null;
            SkinnedMeshRenderer maxMesh = parent.GetComponentInChildren<SkinnedMeshRenderer>();
            if (maxMesh != null)
            {
                foreach (SkinnedMeshRenderer smr in parent.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    if (smr.bounds.size.magnitude > maxMesh.bounds.size.magnitude)
                    {
                        maxMesh = smr;
                    }
                }
                mesh = maxMesh.gameObject;
            }
            else if (parent.GetComponentInChildren<MeshFilter>() != null)
            {
                mesh = parent.GetComponentInChildren<MeshFilter>().gameObject;
            }
            else
            {
                mesh = parent.GetComponentInChildren<MeshRenderer>().gameObject;
            }


            Transform headJoint = FindDeepChild(parent, "HEAD");

            // Jointed model
            if (headJoint != null)
            {
                float estimatedBoundingSphereRaduis = 1.0f;
                Transform mouthJoint = FindDeepChild(parent, "LOWER_LIP");
                if (mouthJoint == null)
                {
                    mouthJoint = FindDeepChild(parent, "MOUTH");
                    estimatedBoundingSphereRaduis = UnityEngine.Vector3.Distance(headJoint.position, mouthJoint.position);
                }
                
                topOffset = headJoint.position + new UnityEngine.Vector3(0, 1.5f, 0) * estimatedBoundingSphereRaduis;
                //topOffset = headJoint.TransformPoint(topOffset);
                //Debug.Log(parent.name);

                maxMesh.gameObject.AddComponent<MeshCollider>();
                maxMesh.gameObject.GetComponent<MeshCollider>().sharedMesh = maxMesh.sharedMesh;
                maxMesh.gameObject.GetComponent<MeshCollider>().convex = true;
                //Debug.DrawRay(topOffset, -UnityEngine.Vector3.up);
                //Debug.Break();
                RaycastHit hit;
                if (Physics.Raycast(topOffset, -UnityEngine.Vector3.up, out hit, Mathf.Infinity))
                {
                    maxMesh.gameObject.GetComponent<MeshCollider>().enabled = false;
                    return hit.point;
                }
                else
                {
                    return topOffset;
                }
            }

            // Use box collider since it is aligned with local axises
            bool hasBoxCollider = (mesh.GetComponent<BoxCollider>() != null);
            if (!hasBoxCollider)
            {
                mesh.AddComponent<BoxCollider>();
            }
            UnityEngine.Vector3 center = mesh.GetComponent<BoxCollider>().center;
            UnityEngine.Vector3 size = mesh.GetComponent<BoxCollider>().size;
            UnityEngine.Vector3 min = center - size * 0.5f;
            UnityEngine.Vector3 max = center + size * 0.5f;
            Debug.Log(center);
            Debug.Log(size);
            if (!hasBoxCollider)
            {
                Destroy(mesh.GetComponent<BoxCollider>());
            }

            // Assume the pivot of the model is on the center
            topOffset.y = max.y; 
            topOffset.x = (max.x + min.x) / 2;
            topOffset.z = (max.z + min.z) / 2;
            topOffset = parent.TransformPoint(topOffset);
            return topOffset;
        }

        private UnityEngine.Vector3 GetSpeechBubbleOffset(Transform parent)
        {
            UnityEngine.Vector3 topOffset = UnityEngine.Vector3.zero;
            Transform mouthJoint = FindDeepChild(parent, "LOWER_LIP");

            // Jointed model
            if (mouthJoint != null)
            {
                return mouthJoint.position;
            }

            Transform headJoint = FindDeepChild(parent, "HEAD");
            if (headJoint != null)
            {
                return headJoint.position;
            }

            // Find the valuable mesh
            // It could be the biggest skinned mesh or the whole mesh
            GameObject mesh = null;
            SkinnedMeshRenderer maxMesh = parent.GetComponentInChildren<SkinnedMeshRenderer>();
            if (maxMesh != null)
            {
                foreach (SkinnedMeshRenderer smr in parent.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    if (smr.bounds.size.magnitude > maxMesh.bounds.size.magnitude)
                    {
                        maxMesh = smr;
                    }
                }
                mesh = maxMesh.gameObject;
            }
            else if (parent.GetComponentInChildren<MeshFilter>() != null)
            {
                mesh = parent.GetComponentInChildren<MeshFilter>().gameObject;
            }
            else
            {
                mesh = parent.GetComponentInChildren<MeshRenderer>().gameObject;
            }

            // Use box collider since it is aligned with local axises
            bool hasBoxCollider = (mesh.GetComponent<BoxCollider>() != null);
            if (!hasBoxCollider)
            {
                mesh.AddComponent<BoxCollider>();
            }
            UnityEngine.Vector3 center = mesh.GetComponent<BoxCollider>().center;
            UnityEngine.Vector3 size = mesh.GetComponent<BoxCollider>().size;
            UnityEngine.Vector3 min = center - size * 0.5f;
            UnityEngine.Vector3 max = center + size * 0.5f;
            Debug.Log(center);
            Debug.Log(size);
            if (!hasBoxCollider)
            {
                Destroy(mesh.GetComponent<BoxCollider>());
            }

            // Assume the pivot of the model is on the center
            topOffset.z = min.z;
            topOffset.y = (max.y + min.y) / 2;
            topOffset.x = (max.x + min.x) / 2;
            topOffset = parent.TransformPoint(topOffset);
            return topOffset;
        }
    }
}
