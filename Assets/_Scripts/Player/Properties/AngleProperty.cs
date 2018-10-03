using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class AngleProperty : PropertyBase<Angle> {
        [PInteropConstructor]
        public AngleProperty(Angle value) : base(value) {}

        public override Angle Interpolate(Angle a, Angle b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}