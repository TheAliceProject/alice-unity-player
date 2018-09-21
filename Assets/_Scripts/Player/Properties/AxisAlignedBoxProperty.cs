using Alice.Tweedle.Interop;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    [PInteropType]
    public class AxisAlignedBoxProperty : PropertyBase<AxisAlignedBox> {
        [PInteropConstructor]
        public AxisAlignedBoxProperty(TValue owner, AxisAlignedBox value) : base(owner, value) {}

        public override AxisAlignedBox Interpolate(AxisAlignedBox a, AxisAlignedBox b, double t) {
            return a.interpolatePortion(b, t);
        }
    }
}