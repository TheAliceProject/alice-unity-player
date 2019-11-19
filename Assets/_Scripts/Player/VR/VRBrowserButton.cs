using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Alice.Tweedle.Parse
{
    public class VRBrowserButton : MonoBehaviour
    {
        public Button button;
        public Image labelImage;
        public TextMeshProUGUI label;
        public VRBrowser parentBrowser;

        void Start()
        {
            button.onClick.AddListener(() =>
            {
                if (parentBrowser != null)
                    parentBrowser.ChooseFileOrDirectory(label.text);
            });
        }

        public void SetBrowser(VRBrowser browser)
        {
            parentBrowser = browser;
        }

        public void SetSpriteLabel(Sprite s)
        {
            labelImage.sprite = s;
        }
    }
}
