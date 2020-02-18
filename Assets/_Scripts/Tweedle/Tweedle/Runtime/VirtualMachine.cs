using System;
using System.Collections;
using Alice.Player.Modules;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.VM
{
    public class VirtualMachine // : MonoBehaviour
    {
        ExecutionScope staticScope;
        public TweedleSystem Library { get; private set; }
        private ExecutionQueue executionQueue = new ExecutionQueue();
        public Action<TweedleRuntimeException> ErrorHandler;

        public VirtualMachine()
        {
            staticScope = new ExecutionScope("VM", this);
        }

        public VirtualMachine(TweedleSystem tweedleSystem)
        {
            staticScope = new ExecutionScope("VM", this);
            Initialize(tweedleSystem);
        }

        internal void Initialize(TweedleSystem tweedleSystem)
        {
            executionQueue.ClearQueue();
            
            Library = tweedleSystem;
            Library?.Prep(this);
        }

        public void Queue(ITweedleExpression exp)
        {
            Queue(new ExpressionStatement(exp));
        }

        private void Queue(TweedleStatement statement)
        {
            statement.QueueStepToNotify(staticScope, new ExecutionStep(staticScope));
        }

        protected internal void AddStep(ExecutionStep step)
        {
            executionQueue.AddToQueue(step);
            ProcessQueueSafely();
        }
        
        internal void ProcessQueueSafely()
        {
            try {
                executionQueue.ProcessOneFrame();
            } catch (TweedleRuntimeException tre) {
                if (ErrorHandler != null) {
                    ErrorHandler.Invoke(tre);
                } else {
                    throw;
                }
            }
        }

        internal IEnumerator ProcessQueue()
        {
            ProcessQueueSafely();
            yield return null;
        }

        public void Resume()
        {
            ProcessQueueSafely();
        }
    }

}
