using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Unity;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class ImageSource : Paint
    {
        private readonly UnityEngine.Texture2D Value;

        public ImageSource(UnityEngine.Texture2D inTexture)
        {
            Value = inTexture;
        }

        #region Interop Interfaces
        [PInteropConstructor]
        public ImageSource(string resource)
        {
            Value = SceneGraph.Current.TweedleSystem.TextureNamed(resource);
        }

        [PInteropField]
        public override int width { get { return Value.width; } }
        
        [PInteropField]
        public override int height { get { return Value.height; } }

        [PInteropMethod]
        public override bool equals(Paint other) 
        {
            return Equals(other);
        }

        [PInteropMethod]
        public override Paint interpolatePortion(Paint end, Portion portion) 
        {   
            if (end.PaintType == PaintTypeID.Color) {
                return portion == 0 ? (Paint)this : end;
            }

            if (end.PaintType == PaintTypeID.ImageSource) {
                return portion == 0 ? (Paint)this : end;
            }

            throw new TweedleRuntimeException("Could not interpolate paint type");

        }
        #endregion // Interop Interfaces

        public override PaintTypeID PaintType { get { return PaintTypeID.ImageSource; } }

        public override void Apply(UnityEngine.MaterialPropertyBlock inPropertyBlock, float inOpacity, string inTextureName, BaseMaterial baseMaterial = BaseMaterial.Opaque) {
            inPropertyBlock.SetTexture(inTextureName, Value);
            inPropertyBlock.SetColor(SGModel.COLOR_SHADER_NAME, new UnityEngine.Color(1, 1, 1, inOpacity));
        }

        public override string ToString() {
            return $"ImageSource({Value.name})";
        }

        public override bool Equals(object obj) 
        {
            if (obj is ImageSource) {
                return ((ImageSource)obj).Value == Value;
            }
            return false;
        }

        public override int GetHashCode() 
        {
            return Value != null ? Value.GetHashCode() : base.GetHashCode();
        }
    }
}