using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public abstract class SGShape : SGModel {
        protected override void OnPaintPropertyChanged(TValue inValue) {
            SetPaint(inValue, MAIN_TEXTURE_SHADER_NAME);
        }
    }
}