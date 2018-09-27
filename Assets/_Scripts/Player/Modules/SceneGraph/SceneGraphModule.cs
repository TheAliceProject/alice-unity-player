using Alice.Tweedle;
using Alice.Tweedle.Interop;

namespace Alice.Player.Modules {
    [PInteropType("SceneGraph")]
    static public class SceneGraphModule
    {
        [PInteropMethod]
        public static SGEntity createEntity(ShapeModelEmum shape) {
            switch (shape) {
                case ShapeModelEmum.BOX:
                    return SGBox.Create<SGBox>("BoxEntity");
                default:
                    throw new SceneGraphException("No model found for shape enum: " + shape);
            }
        }
    }
}