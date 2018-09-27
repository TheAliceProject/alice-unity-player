using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class ColorProperty : PropertyBase<Color> {
        [PInteropConstructor]
        public ColorProperty(Color value) : base(value) {}

        public override Color Interpolate(Color a, Color b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}