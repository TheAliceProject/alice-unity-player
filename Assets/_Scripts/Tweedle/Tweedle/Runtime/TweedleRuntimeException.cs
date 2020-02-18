using System;

namespace Alice.Tweedle
{
    public class TweedleRuntimeException : SystemException
    {
        public TweedleRuntimeException(string message)
            :base(message)
        {
        }
        public TweedleRuntimeException(Exception cause)
            :base("System Exception while executing tweedle: " + cause.Message, cause)
        {
        }

        public TweedleRuntimeException(string messageFormat, params object[] args)
            : base(String.Format(messageFormat, args))
        {
        }

    }
}