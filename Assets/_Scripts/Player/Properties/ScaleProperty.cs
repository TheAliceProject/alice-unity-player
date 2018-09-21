using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class ScaleProperty : PropertyBase<Scale> {
        [PInteropConstructor]
        public ScaleProperty(TValue owner, Scale value) : base(owner, value) {}

        public override Scale Interpolate(Scale a, Scale b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}