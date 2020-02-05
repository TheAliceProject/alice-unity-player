using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Key-value pair of name to argument.
    /// </summary>
    public struct NamedArgument
    {
        public readonly string Name;
        public readonly ITweedleExpression Argument;

        public NamedArgument(string inName, ITweedleExpression inArgument)
        {
            Name = inName;
            Argument = inArgument;
        }

        static public readonly NamedArgument[] EMPTY_ARGS = new NamedArgument[0];
    }
}