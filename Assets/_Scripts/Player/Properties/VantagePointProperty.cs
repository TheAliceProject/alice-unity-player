using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;
namespace Alice.Player.Modules {
    [PInteropType]
    public class VantagePointProperty : PropertyBase<VantagePoint> {
        [PInteropConstructor]
        public VantagePointProperty(VantagePoint value) : base(value) {}

        public override VantagePoint Interpolate(VantagePoint a, VantagePoint b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}