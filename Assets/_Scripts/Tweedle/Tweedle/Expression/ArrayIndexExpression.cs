using System;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class ArrayIndexExpression : TweedleExpression
    {
        ITweedleExpression array;
        ITweedleExpression index;

        public ArrayIndexExpression(ITweedleExpression array, ITweedleExpression index)
            : base()
        {
            this.array = array;
            this.index = index;
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            return new TwoValueComputationStep(
                this,
                scope,
                array,
                index,
                (arr, val) => (arr.Array())[val.ToInt()]);
        }


        public ExecutionStep AsAssignmentStep(ExecutionScope scope, ITweedleExpression valueExp)
        {

            return new ThreeValueComputationStep(
               this, 
               scope, 
               array, 
               index, 
               valueExp, 
               (arr, idx, val) => {
                   (arr.Array())[idx.ToInt()] = val;
                   return val;
               });
        }

        public override string ToTweedle()
        {
            return array.ToTweedle() + "[" + index.ToTweedle() + "]";
        }
    }
}