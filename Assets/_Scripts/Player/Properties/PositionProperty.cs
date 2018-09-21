using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class PositionProperty : PropertyBase<Position> {
        [PInteropConstructor]
        public PositionProperty(TValue owner, Position value) : base(owner, value) {}

        public override Position Interpolate(Position a, Position b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}