using System.Diagnostics;
using Alice.Tweedle.Interop;
using System; 
namespace Alice.Player.Primitives
{
    [PInteropType]
    public struct AxisAlignedBox
    {
        public readonly Vector3 MinValue;
        public readonly Vector3 MaxValue;

        public AxisAlignedBox(Vector3 inMin, Vector3 inMax)
        {
            this.MinValue = inMin;
            this.MaxValue = inMax;
        }

        #region Interop Interfaces
        [PInteropField]
        public Position minimum { get { return new Position(MinValue);} }
        [PInteropField]
        public Position maximum { get { return new Position(MinValue);} }

        [PInteropConstructor]
        public AxisAlignedBox(Position minimum, Position maximum)
        {
            MinValue = minimum.Value;
            MaxValue = maximum.Value;
        }

        [PInteropMethod]
        public bool equals(AxisAlignedBox other) 
        {
            return MaxValue == other.MaxValue && MinValue == other.MinValue;
        }

        [PInteropMethod]
        public AxisAlignedBox transform(VantagePoint vantagePoint) {      

            var m = vantagePoint.GetMatrix();

            // transformed all corner points
            Vector3[] verts = {
                // bottom
                Vector3.Transform(MinValue, m),
                Vector3.Transform(new Vector3(MinValue.X, MaxValue.Y, MinValue.Z), m),
                Vector3.Transform(new Vector3(MaxValue.X, MinValue.Y, MinValue.Z), m),
                Vector3.Transform(new Vector3(MaxValue.X, MaxValue.Y, MinValue.Z), m),
                // top
                Vector3.Transform(new Vector3(MinValue.X, MinValue.Y, MaxValue.Z), m),
                Vector3.Transform(new Vector3(MinValue.X, MaxValue.Y, MaxValue.Z), m),
                Vector3.Transform(new Vector3(MaxValue.X, MinValue.Y, MaxValue.Z), m),
                Vector3.Transform(MaxValue, m)
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

        [PInteropMethod]
        public AxisAlignedBox interpolatePortion(AxisAlignedBox end, Portion portion) {
            return new AxisAlignedBox(Vector3.Lerp(MinValue, end.MinValue, portion), Vector3.Lerp(MaxValue, end.MaxValue, portion.Value));
        }
        #endregion // Interop Interfaces

        public override string ToString() {
            return string.Format("AABB[min({0:0.##},{1:0.##},{2:0.##}),max({3:0.##},{4:0.##},{5:0.##})]", MinValue.X, MinValue.Y, MinValue.Z, MaxValue.X, MaxValue.Y, MaxValue.Z);
        }

        public override bool Equals(object obj) {
            if (obj is AxisAlignedBox) {
                return equals((AxisAlignedBox)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return MinValue.GetHashCode() + MaxValue.GetHashCode();
        }
    }
}