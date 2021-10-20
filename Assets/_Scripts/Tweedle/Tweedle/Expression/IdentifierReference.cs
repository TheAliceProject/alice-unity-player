using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class IdentifierReference : TweedleExpression
    {
        public string Name { get; }

        public IdentifierReference(string name)
            : base(null)
        {
            Name = name;
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            return new ValueGenerationStep(this, scope, () => scope.GetValue(Name));
        }

        public override string ToTweedle()
        {
            return Name;
        }
    }
}