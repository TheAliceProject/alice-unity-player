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
            bubbleText.fontSize = BASE_FONT_SIZE * scale;
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
            bubbleText.transform.SetScale(1f, Axis.X);
            tailRoutine.Replace(this, AlignTailRoutine(entity));
            yield return trans.ScaleTo(new Vector2(1f, 1f), 0.25f, Axis.XYZ);
            yield return duration;
            yield return trans.ScaleTo(0f, 0.25f, Axis.XYZ);
            sayThinkControlRef.DestroyBubble(this);
        }

        private IEnumerator AlignTailRoutine(SGEntity entity){
            while(true){
                UnityEngine.Vector3 objectPos = entity.cachedTransform.localPosition;
                var objectScreenPoint = Camera.main.WorldToScreenPoint(objectPos); // convert target's world space position to a screen position
        
                float tailLength = Vector2.Distance(objectScreenPoint, tailPivot.position) - 50f; // -50f for some buffer 
                //Debug.DrawLine(tailPivot.position, objectScreenPoint, UnityEngine.Color.green, 4f);
                float tailRotation = 180f + (Mathf.Rad2Deg * Mathf.Atan((objectScreenPoint.x - tailPivot.position.x) / (tailPivot.position.y - objectScreenPoint.y))) + (objectScreenPoint.y < tailPivot.position.y ? 180f : 0f);
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
    }
}
