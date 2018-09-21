using Alice.Tweedle.Interop;
using Alice.Tweedle.Primitives;

namespace Alice.Tweedle.Modules {
    [PInteropType]
    public class PortionProperty : PropertyBase<Portion> {
        [PInteropConstructor]
        public PortionProperty(TValue owner, Portion value) : base(owner, value) {}

        public override Portion Interpolate(Portion a, Portion b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}