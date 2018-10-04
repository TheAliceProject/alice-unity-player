using System;

namespace Alice.Tweedle
{
    public class TweedleUninitializedFieldException : SystemException
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