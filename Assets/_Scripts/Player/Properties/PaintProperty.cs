using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class PaintProperty : PropertyBase<Paint> {
        [PInteropConstructor]
        public PaintProperty(Paint value) : base(value) {}

        public override Paint Interpolate(Paint a, Paint b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}