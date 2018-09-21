using Alice.Tweedle.Interop;
using Alice.Tweedle.Primitives;

namespace Alice.Tweedle.Modules {
    [PInteropType]
    public class VantagePointProperty : PropertyBase<VantagePoint> {
        [PInteropConstructor]
        public VantagePointProperty(TValue owner, VantagePoint value) : base(owner, value) {}

        public override VantagePoint Interpolate(VantagePoint a, VantagePoint b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}