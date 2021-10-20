using System.Collections.Generic;
using System.Linq;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class ArrayInitializer : TweedleExpression
    {
        private ITweedleExpression m_Size;
        private ITweedleExpression[] m_Elements;

        private static IStackFrame storeValueStackFrame = new StaticStackFrame("Store Value");

        public ArrayInitializer(TArrayType arrayType, ITweedleExpression[] elements)
            : base(arrayType)
        {
            this.m_Elements = elements;
        }

        public ArrayInitializer(TArrayType arrayType, ITweedleExpression initializeSize)
            : base(arrayType)
        {
            this.m_Size = initializeSize;
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            TArrayType arrayType = (TArrayType)this.m_TypeRef;
            StepSequence main = new StepSequence(this, scope);

            if (m_Size != null)
            {
                int desiredSize = 0;
                ExecutionStep sizeStep = m_Size.AsStep(scope);
                ExecutionStep storeStep = new ValueComputationStep(storeValueStackFrame, scope, (res) =>
                {
                    desiredSize = (int)res.RawNumber();
                    return res;
                });
                sizeStep.OnCompletionNotify(storeStep);
                main.AddStep(sizeStep);

                main.AddStep(new ValueGenerationStep(
                    this, scope,
                    () =>
                    {
                        return arrayType.Instantiate(desiredSize);
                    }));
            }
            else
            {
                TValue[] elements = AddElementSteps(main, scope);
                main.AddStep(new ValueGenerationStep(
                    this, scope,
                    () =>
                    {
                        return arrayType.Instantiate(elements);
                    }));
            }

            return main;
        }

        private TValue[] AddElementSteps(StepSequence ioMain, ExecutionScope scope)
        {
            TValue[] results = new TValue[m_Elements.Length];
            for (int i = 0; i < m_Elements.Length; ++i)
            {
                int idx = i;
                ITweedleExpression element = m_Elements[i];
                ExecutionStep computeStep = element.AsStep(scope);
                ExecutionStep storeStep = new ValueComputationStep(storeValueStackFrame, scope, (res) =>
                {
                    results[idx] = res;
                    return res;
                });
                computeStep.OnCompletionNotify(storeStep);
                ioMain.AddStep(computeStep);
            }

            return results;
        }

        public override string ToString() {
            return "new Array";
        }
    }
}