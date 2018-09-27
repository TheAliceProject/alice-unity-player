using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class DirectionProperty : PropertyBase<Direction> {
        [PInteropConstructor]
        public DirectionProperty(Direction value) : base(value) {}

        public override Direction Interpolate(Direction a, Direction b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}