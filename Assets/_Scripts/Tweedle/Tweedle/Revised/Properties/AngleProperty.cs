using Alice.Tweedle.Interop;
using Alice.Tweedle.Primitives;

namespace Alice.Tweedle.Modules {
    [PInteropType]
    public class AngleProperty : PropertyBase<Angle> {
        [PInteropConstructor]
        public AngleProperty(TValue owner, Angle value) : base(owner, value) {}

        public override Angle Interpolate(Angle a, Angle b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}