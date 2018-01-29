using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI.Util
{
    /// <summary>Simple FPS-Counter.</summary>
    public class FPSDisplay : MonoBehaviour {
        public Text FPS;

        private float deltaTime = 0f;
        private float elapsedTime = 0f;

        private float msec;
        private float fps;
        private string text;

        public void Update() {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            elapsedTime += Time.deltaTime;

            if (elapsedTime > 1f) {
                if (Time.frameCount % 3 == 0 && FPS != null) {
                    msec = deltaTime * 1000f;
                    fps = 1f / deltaTime;

                    text = string.Format ("<b>FPS: {0:0.}</b> ({1:0.0} ms)", fps, msec);

                    if (fps < 15) {
                        FPS.text = "<color=red>" + text + "</color>";
                    } else if (fps < 29) {
                        FPS.text = "<color=orange>" + text + "</color>";
                    } else {
                        FPS.text = "<color=green>" + text + "</color>";
                    }
                }
            } else {
                FPS.text = "<i>...calculating <b>FPS</b>...</i>";
            }
        }
    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)