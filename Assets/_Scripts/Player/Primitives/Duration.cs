using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using System;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public struct Duration
    {
        // base value is in seconds
        public readonly double Value;

        #region Interop Interfaces
    [PInteropField]
        public static readonly Duration ZERO = new Duration(0);

        [PInteropField]
        public static readonly Duration QUARTER_SECOND = new Duration(0.25);

        [PInteropField]
        public static readonly Duration HALF_SECOND = new Duration(0.5);

        [PInteropField]
        public static readonly Duration ONE_SECOND = new Duration(1);

        [PInteropField]
        public static readonly Duration TWO_SECONDS = new Duration(2);

        [PInteropField]
        public double seconds 
        { 
            get {
                return Value; 
            }
        }

        [PInteropField]
        public double minutes 
        { 
            get {
                return Value/60;
            }
        }

        [PInteropField]
        public double milliseconds 
        {
            get {
                return Value*1000;
            }
        }

        [PInteropField]
        public double microseconds 
        {
            get {
                return Value*1000000;
            }
        }

        [PInteropConstructor]
        public Duration(double seconds)
        {
            this.Value = seconds;
        }

        [PInteropMethod]
        public bool equals(Duration other) 
        {
            return this.Value == other.Value;
        }

        [PInteropMethod]
        public Duration add(Duration other) 
        {
            return new Duration(Value + other.Value);
        }

        [PInteropMethod]
        public Duration subtract(Duration other)
        {
            return new Duration(Value - other.Value);
        }

        [PInteropMethod]
        public Duration scaledBy(double factor) 
        {
            return new Duration(Value * factor);
        }

        [PInteropMethod]
        public Duration dividedBy(double divisor)
        {
            return new Duration(Value / divisor);
        }

        [PInteropMethod]
        public Duration interpolatePortion(Duration end, Portion portion) {
            return new Duration((end.Value - Value)*portion.Value + Value);
        }
        #endregion // Interop Interfaces

        static public implicit operator double(Duration inDuration)
        {
            return inDuration.Value;
        }

        static public implicit operator float(Duration inDuration)
        {
            return (float)inDuration.Value;
        }

        public override string ToString() {
            return string.Format("Duration({0:0.##})", Value);
        }

        public override bool Equals(object obj) {
            if (obj is Duration) {
                return equals((Duration)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}