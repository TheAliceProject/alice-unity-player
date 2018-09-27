using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class AxisAlignedBoxProperty : PropertyBase<AxisAlignedBox> {
        [PInteropConstructor]
        public AxisAlignedBoxProperty(AxisAlignedBox value) : base(value) {}

        public override AxisAlignedBox Interpolate(AxisAlignedBox a, AxisAlignedBox b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}