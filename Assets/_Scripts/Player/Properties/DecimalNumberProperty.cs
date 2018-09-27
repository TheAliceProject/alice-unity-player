using Alice.Tweedle.Interop;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class DecimalNumberProperty : PropertyBase<double> {
        [PInteropConstructor]
        public DecimalNumberProperty(double value) : base(value) {}

        public override double Interpolate(double a, double b, double t) {
            return (b-a)*t + a;
        }
    }
}