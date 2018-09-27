using Alice.Tweedle.Interop;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class WholeNumberProperty : PropertyBase<double> {
        [PInteropConstructor]
        public WholeNumberProperty(double value) : base(value) {}

        public override double Interpolate(double a, double b, double t) {
            return System.Math.Round((b-a)*t + a);
        }
    }
}