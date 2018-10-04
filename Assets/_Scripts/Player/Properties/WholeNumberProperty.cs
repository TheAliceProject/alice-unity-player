using Alice.Tweedle.Interop;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class WholeNumberProperty : PropertyBase<double> {
        [PInteropConstructor]
        public WholeNumberProperty(TValue owner, double value) : base(owner, value) {}

        public override double Interpolate(double a, double b, double t) {
            return System.Math.Round((b-a)*t + a);
        }
    }
}