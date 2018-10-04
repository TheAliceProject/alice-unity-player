using System;

namespace Alice.Tweedle
{
    public class TweedleNoMembersException : SystemException
    {
        public TweedleNoMembersException(string message)
			:base(message)
        {
        }

        public TweedleNoMembersException(TType type, string memberType)
			:base("No members of type " + memberType + " on " + type.Name)
        {
        }
    }
}