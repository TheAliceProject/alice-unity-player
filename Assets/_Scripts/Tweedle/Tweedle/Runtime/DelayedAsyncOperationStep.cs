using System;
using Alice.Tweedle;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.VM
{
	class DelayedAsyncOperationStep : ExecutionStep
	{
		Func<IAsyncReturn> body;
        private IAsyncReturn m_Result;

        public DelayedAsyncOperationStep(string callStackEntry, ExecutionScope scope, Func<IAsyncReturn> body)
			: base(scope)
		{
			this.callStack = scope.StackWith(callStackEntry);
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