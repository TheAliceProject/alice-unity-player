using Alice.Tweedle.Interop;
using Alice.Tweedle.Primitives;

namespace Alice.Tweedle.Modules {
    [PInteropType]
    public class DirectionProperty : PropertyBase<Direction> {
        [PInteropConstructor]
        public DirectionProperty(TValue owner, Direction value) : base(owner, value) {}

        public override Direction Interpolate(Direction a, Direction b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}