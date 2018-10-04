using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class OrientationProperty : PropertyBase<Orientation> {
        [PInteropConstructor]
        public OrientationProperty(TValue owner, Orientation value) : base(owner, value) {}

        public override Orientation Interpolate(Orientation a, Orientation b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}