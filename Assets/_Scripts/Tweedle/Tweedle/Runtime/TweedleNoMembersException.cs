namespace Alice.Tweedle
{
    public class TweedleNoMembersException : TweedleRuntimeException
    {
        public TweedleNoMembersException(string name, TType type, string memberType)
            :base("No members of type " + memberType + " on " + type.Name + ". Cannot access " + memberType + " " + name + ".")
        {
        }
    }
}