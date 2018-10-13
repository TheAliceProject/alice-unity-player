using System;
using Alice.Tweedle;
using Alice.Tweedle.Interop;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    class DelayedAsyncOperationStep : ExecutionStep
    {
        Func<IAsyncReturn> body;
        private IAsyncReturn m_Result;

        public DelayedAsyncOperationStep(string callStackEntry, ExecutionScope scope, Func<IAsyncReturn> body)
            : base(scope)
        {
            using (PooledStringBuilder stackBuilder = PooledStringBuilder.Alloc()) {
                scope.StackWith(stackBuilder.Builder);
                this.callStack = stackBuilder.ToString();
            }
            this.body = body;
        }

        internal override void Execute()
        {
            if (m_Result == null)
            {
                m_Result = body.Invoke();
                if (m_Result != null)
                {
                    status = StepStatus.WaitingOnFrame;
                    m_Result.OnReturn(this.OnResultCompleted);
                }
                else
                {
                    base.Execute();
                }
            }
        }

        private void OnResultCompleted(object inObject)
        {
            base.Execute();
        }
    }
}