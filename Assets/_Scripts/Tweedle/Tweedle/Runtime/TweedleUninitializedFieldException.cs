using System;

namespace Alice.Tweedle
{
    public class TweedleUninitializedFieldException : TweedleRuntimeException
    {
        public TweedleUninitializedFieldException(string message)
            :base(message)
        {
        }

        public TweedleUninitializedFieldException(TObject obj, string fieldName)
            :base("Attempt to read uninitialized field " + fieldName + " on " + obj)
        {
        }
    }
}