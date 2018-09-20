using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class AxisAlignedBox
    {
        public readonly Vector3 minValue = Vector3.Zero;
        public readonly Vector3 maxValue = Vector3.Zero;

        public AxisAlignedBox(Vector3 inMin, Vector3 inMax)
        {
            this.minValue = inMin;
            this.maxValue = inMax;
        }

        #region Interop Interfaces
        [PInteropField]
        public Position minimum { get { return new Position(minValue);} }
        [PInteropField]
        public Position maximum { get { return new Position(minValue);} }

        [PInteropConstructor]
        public AxisAlignedBox(Position minimum, Position maximum)
        {
            minValue = minimum.value;
            maxValue = maximum.value;
        }

        [PInteropConstructor]
        public AxisAlignedBox(AxisAlignedBox clone)
        {
            minValue = clone.minValue;
            maxValue = clone.maxValue;
        }

        [PInteropMethod]
        public bool equals(AxisAlignedBox box) 
        {
            return maxValue == box.maxValue && minValue == box.minValue;
        }
        #endregion // Interop Interfaces

        public override string ToString() {
            return string.Format("AABB[min({0:0.##},{1:0.##},{2:0.##}),max({3:0.##},{4:0.##},{5:0.##})]", minValue.X, minValue.Y, minValue.Z, maxValue.X, maxValue.Y, maxValue.Z);
        }
    }
}