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
                UnityEngine.Vector3 objectPos = speechOrigin.position;
                float tailRotation = 0f;
                float tailLength = 0f;

                if(VRControl.IsLoadedInVR()){
                    var screenPoint = Camera.main.WorldToScreenPoint(objectPos); // convert target's world space position to a screen position
                    UnityEngine.Vector2 objectScreenPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle((this.transform as RectTransform), screenPoint, Camera.main, out objectScreenPoint);
                    tailLength = Vector2.Distance(objectScreenPoint, tailPivot.position) - 10f;
                    tailRotation = 180f + (Mathf.Rad2Deg * Mathf.Atan((objectScreenPoint.x - tailPivot.position.x) / (tailPivot.position.y - objectScreenPoint.y))) + (objectScreenPoint.y < tailPivot.position.y ? 180f : 0f);
                }
                else{
                    bubbleText.fontSize = fontSizeUnscaled * (Screen.width / FONT_WIDTH_SCALE);
                    var objectScreenPoint = Camera.main.WorldToScreenPoint(objectPos); // convert target's world space position to a screen position
                    tailLength = Vector2.Distance(objectScreenPoint, tailPivot.position) - 10f; // -10f for some buffer space away from mouth 
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
                    float lastTailPos = 150f;
                    if(tailPivot.localEulerAngles.z < 35f || (tailPivot.localEulerAngles.z > 145f && tailPivot.localEulerAngles.z < 225f) || tailPivot.localEulerAngles.z > 315f)
                        lastTailPos = 80f;
                    for(int i = 0; i < tailOutline.Length; i++){
                        Vector2 tailPos = tailOutline[i].rectTransform.anchoredPosition;
                        tailPos.y = -(lastTailPos + ((tailLength * i) / tailOutline.Length));
                        tailOutline[i].rectTransform.anchoredPosition = tailPos;
                    }
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
    }
}
