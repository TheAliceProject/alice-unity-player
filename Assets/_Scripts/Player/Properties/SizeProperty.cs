using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class SizeProperty : PropertyBase<Size> {
        [PInteropConstructor]
        public SizeProperty(Size value) : base(value) {}

        public override Size Interpolate(Size a, Size b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}