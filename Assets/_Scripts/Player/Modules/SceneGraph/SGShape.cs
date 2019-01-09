using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public abstract class SGShape : SGModel {
        public const string RADIUS_PROPERTY_NAME = "Radius";
        public const string INNER_RADIUS_PROPERTY_NAME = "InnerRadius";
        public const string OUTER_RADIUS_PROPERTY_NAME = "OuterRadius";
        public const string LENGTH_PROPERTY_NAME = "Length";

        protected override string ShaderTextureName { get { return MAIN_TEXTURE_SHADER_NAME; } }

        protected abstract Mesh ShapeMesh { get; } 

        protected override void Awake() {
            base.Awake();

            var go = new GameObject("Model");
            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = ShapeMesh;
            var rend = go.AddComponent<MeshRenderer>();

            Init(go.transform, rend, filter);
        }
    }
}