using System.Diagnostics;
using Alice.Tweedle.Interop;
using System; 
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
        public bool equals(AxisAlignedBox other) 
        {
            return maxValue == other.maxValue && minValue == other.minValue;
        }

        [PInteropMethod]
        public AxisAlignedBox transform(VantagePoint vantagePoint) {      
            // transformed all corner points
            Vector3[] verts = {
                // bottom
                Vector3.Transform(minValue, vantagePoint.value),
                Vector3.Transform(new Vector3(minValue.X, maxValue.Y, minValue.Z), vantagePoint.value),
                Vector3.Transform(new Vector3(maxValue.X, minValue.Y, minValue.Z), vantagePoint.value),
                Vector3.Transform(new Vector3(maxValue.X, maxValue.Y, minValue.Z), vantagePoint.value),
                // top
                Vector3.Transform(new Vector3(minValue.X, minValue.Y, maxValue.Z), vantagePoint.value),
                Vector3.Transform(new Vector3(minValue.X, maxValue.Y, maxValue.Z), vantagePoint.value),
                Vector3.Transform(new Vector3(maxValue.X, minValue.Y, maxValue.Z), vantagePoint.value),
                Vector3.Transform(maxValue, vantagePoint.value)
            };

            Vector3 min = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
            Vector3 max = new Vector3(double.MinValue, double.MinValue, double.MinValue);
            for (int i = 0; i < verts.Length; ++i) {
                Vector3 p = verts[i];
                min.X = Math.Min(min.X, p.X);
                min.Y = Math.Min(min.Y, p.Y);
                min.Z = Math.Min(min.Z, p.Z);
                max.X = Math.Max(max.X, p.X);
                max.Y = Math.Max(max.Y, p.Y);
                max.Z = Math.Max(max.Z, p.Z);
            }

            return new AxisAlignedBox(min, max);
        }

        #endregion // Interop Interfaces

        public override string ToString() {
            return string.Format("AABB[min({0:0.##},{1:0.##},{2:0.##}),max({3:0.##},{4:0.##},{5:0.##})]", minValue.X, minValue.Y, minValue.Z, maxValue.X, maxValue.Y, maxValue.Z);
        }
    }
}