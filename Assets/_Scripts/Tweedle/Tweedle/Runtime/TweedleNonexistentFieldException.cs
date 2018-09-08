using System;

namespace Alice.Tweedle
{
    public class TweedleNonexistentFieldException : SystemException
    {
        public TweedleNonexistentFieldException(string message)
			:base(message)
        {
        }

        public TweedleNonexistentFieldException(TObject obj, string fieldName)
			:base("Attempt to read nonexistent field " + fieldName + " on " + obj)
        {
        }
    }
}