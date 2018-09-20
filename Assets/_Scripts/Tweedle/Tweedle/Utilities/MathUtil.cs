using System.Linq;

namespace Alice.Tweedle {
    public static class MathUtil {
        public static double Min(params double[] values) {
            return Enumerable.Min(values);
        }

        public static double Max(params double[] values) {
            return Enumerable.Max(values);
        }
    }
}