using Alice.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Interface for something that can be evaluated as an expression.
    /// </summary>
    public interface ITweedleExpression
    {
        TTypeRef Type { get; }
        string ToTweedle();
        ExecutionStep AsStep(ExecutionScope inScope);
    }
}