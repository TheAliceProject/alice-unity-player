using System;

namespace Alice.Tweedle
{
    public class TweedleUnsupportedException : SystemException
    {
        public TweedleUnsupportedException(string message)
            :base(message)
        {
        }

        public TweedleUnsupportedException(TType type, string operation)
            :base(operation + " is not supported on " + type.Name)
        {
        }
    }
}