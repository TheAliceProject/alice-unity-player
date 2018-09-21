using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Modules {
    [PInteropType]
    public class DecimalNumberProperty : PropertyBase<double> {
        [PInteropConstructor]
        public DecimalNumberProperty(TValue owner, double value) : base(owner, value) {}

        public override double Interpolate(double a, double b, double t) {
            return (b-a)*t + a;
        }
    }
}