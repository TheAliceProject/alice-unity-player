using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;
using Alice.Tweedle.Interop;
using TMPro;

namespace Alice.Player.Unity {

    public class SayThinkControl : MonoBehaviour
    {
        [SerializeField]
        private TMP_FontAsset serifFont = null;
        [SerializeField]
        private TMP_FontAsset sansSerifFont = null;
        [SerializeField]
        private TMP_FontAsset monospaceFont = null;

        [SerializeField]
        private SayThinkBubble sayPrefab = null;
        [SerializeField]
        private SayThinkBubble thinkPrefab = null;

        [SerializeField]
        private RectTransform leftBubbles = null;
        [SerializeField]
        private RectTransform centerBubbles = null;
        [SerializeField]
        private RectTransform rightBubbles = null;

        private Dictionary<SayThinkBubble, AsyncReturn> bubbleReturns = new Dictionary<SayThinkBubble, AsyncReturn>();
        private float m_lastWidth;
        private float m_lastHeight;
        private const float BUBBLE_SPACING = 50f;
        private const float VR_WIDTH = 800f;
        void Update()
        {
            if (Screen.width != m_lastWidth)
            {
                float screenWidth = VRControl.IsLoadedInVR() ? VR_WIDTH : Screen.width;
                Vector2 currDelta = leftBubbles.sizeDelta;
                Vector2 currPos = leftBubbles.anchoredPosition;
                currPos.x = -(screenWidth - BUBBLE_SPACING) / 2f;
                currDelta.x = (screenWidth - BUBBLE_SPACING) / 3f;
                leftBubbles.anchoredPosition = currPos;
                leftBubbles.sizeDelta = currDelta;

                // Center always at 0
                currDelta = centerBubbles.sizeDelta;
                currDelta.x = (screenWidth - BUBBLE_SPACING) / 3f;
                centerBubbles.sizeDelta = currDelta;

                currDelta = rightBubbles.sizeDelta;
                currPos = rightBubbles.anchoredPosition;
                currPos.x = (screenWidth - BUBBLE_SPACING) / 2f;
                currDelta.x = (screenWidth - BUBBLE_SPACING) / 3f;
                rightBubbles.anchoredPosition = currPos;
                rightBubbles.sizeDelta = currDelta;

                m_lastHeight = Screen.height;
                m_lastWidth = Screen.width;
            }
        }

        public void SpawnSayThink(AsyncReturn asyncReturn, Transform parent, SGEntity target, string text, bool isSay, BubblePosition bubblePosition,
                        FontType fontType, TextStyle textStyle, float textScale, UnityEngine.Color bubbleColor, 
                        UnityEngine.Color outlineColor, UnityEngine.Color textColor, double duration)
        {
            TMP_FontAsset font = null;
            switch(fontType){
                case FontType.Default: font = sansSerifFont; break;
                case FontType.Serif: font = serifFont; break;
                case FontType.Sans_Serif: font = sansSerifFont; break;
                case FontType.Monospaced: font = monospaceFont; break;
                default: font = sansSerifFont; break;
            }

            GameObject bubble = InstantiateBubble(isSay ? sayPrefab.gameObject : thinkPrefab.gameObject, bubblePosition, target);
            if (bubble == null) {
                // A restarting world is replacing the main camera so a bubble for the earlier execution can be ignored
                return;
            }
            SayThinkBubble sayThink = bubble.GetComponent<SayThinkBubble>();
            sayThink.SetColor(bubbleColor, outlineColor);
            sayThink.SetText(text, textColor, font, textScale);
            sayThink.SetTextStyle(textStyle);
            sayThink.Spawn(target, (float)duration, this);
            bubbleReturns.Add(sayThink, asyncReturn);
        }

        private GameObject InstantiateBubble(GameObject prefab, BubblePosition pos, SGEntity target){
            if (Camera.main == null) {
                // If the world is restarting this may be reached by a vestigial async process when the camera is null
                return null;
            }
            Transform parent = null;
            if(pos == BubblePosition.Left){
                parent = leftBubbles;
            }
            else if(pos == BubblePosition.Center){
                parent = centerBubbles;
            }
            else if(pos == BubblePosition.Right){
                parent = rightBubbles;
            }
            else if(pos == BubblePosition.Automatic){
                UnityEngine.Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.cachedTransform.localPosition); 
                if(screenPoint.x < Camera.main.pixelWidth / 3)
                    parent = leftBubbles;
                else if(screenPoint.x < ((2 * Camera.main.pixelWidth) / 3))
                    parent = centerBubbles;
                else
                    parent = rightBubbles;
            }

            GameObject bubble = Instantiate(prefab, parent);
            return bubble;
        }

        public void DestroyBubble(SayThinkBubble bubble){
            for(int i = 0; i < 3; i++){
                Transform currBubbles = leftBubbles;
                switch(i){
                    case 0: currBubbles = leftBubbles; break;
                    case 1: currBubbles = centerBubbles; break;
                    case 2: currBubbles = rightBubbles; break;
                }

                for(int j = 0; j < currBubbles.childCount; j++){
                    if(bubble == currBubbles.GetChild(j).gameObject.GetComponent<SayThinkBubble>()){
                        bubbleReturns[bubble].Return();
                        bubbleReturns.Remove(bubble);
                        Destroy(bubble.gameObject);
                        return;
                    }
                }
            }
            Debug.LogWarning("Couldn't find bubble to destroy");
        }
    }
}