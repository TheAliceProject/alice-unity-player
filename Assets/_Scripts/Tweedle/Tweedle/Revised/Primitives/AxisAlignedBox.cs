using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primitives
{


    [PInteropType]
    public sealed class AxisAlignedBox
    {

        public readonly Vector3 minimum = Vector3.Zero;
        public readonly Vector3 maximum = Vector3.Zero;

        public AxisAlignedBox(Vector3 inMin, Vector3 inMax)
        {
            minimum = inMin;
            maximum = inMax;
        }

        public AxisAlignedBox(AxisAlignedBox clone)
        {
            minimum = clone.minimum;
            minimum = clone.maximum;
        }

        [PInteropMethod]
        public Position getMinimum()
        {
	        return new Position(minimum);
	    }

        [PInteropMethod]
        public Position getMaximum()
        {
            return new Position(maximum);
        }

        [PInteropMethod]
        public bool equals(AxisAlignedBox box) 
        {
            return maximum == box.maximum && minimum == box.minimum;
        }
    }
}