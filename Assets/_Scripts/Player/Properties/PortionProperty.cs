using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class PortionProperty : PropertyBase<Portion> {
        [PInteropConstructor]
        public PortionProperty(Portion value) : base(value) {}

        public override Portion Interpolate(Portion a, Portion b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}