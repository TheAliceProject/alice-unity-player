using Alice.Tweedle.Interop;
using Alice.Tweedle.Primitives;

namespace Alice.Tweedle.Modules {
    [PInteropType]
    public class PositionProperty : PropertyBase<Position> {
        [PInteropConstructor]
        public PositionProperty(TValue owner, Position value) : base(owner, value) {}

        public override Position Interpolate(Position a, Position b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}