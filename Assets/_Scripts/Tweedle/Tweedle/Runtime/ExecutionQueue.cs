using System.Collections.Generic;
using Alice.Tweedle;

namespace Alice.Tweedle.VM
{

    public class ExecutionQueue
    {
        // Steps are parallelizable operations. A single sequence of operations will generally have one queued step at a time
        Queue<ExecutionStep> stepsForThisFrame = new Queue<ExecutionStep>();
        Queue<ExecutionStep> stepsForNextFrame = new Queue<ExecutionStep>();
        bool isProcessing = false;

        // This adds an independent thread of work.
        // It may be triggered by
        // * an event
        // * code
        // * a just processed step with more work to do
        // * a previously process step that has been notified all its children are complete
        internal void AddToQueue(ExecutionStep step)
        {
            if (step == null || step.IsComplete())
            {
                return;
            }
            AddToCorrectQueue(step);
        }

        internal void ClearQueue()
        {
            stepsForNextFrame.Clear();
            stepsForThisFrame.Clear();
            isProcessing = false;
        }

        private void AddToCorrectQueue(ExecutionStep step)
        {
            if (step.CanRunThisFrame())
            {
                stepsForThisFrame.Enqueue(step);
            }
            else
            {
                stepsForNextFrame.Enqueue(step);
            }
        }

        void AddToQueue(List<ExecutionStep> steps)
        {
            foreach (ExecutionStep step in steps)
            {
                AddToQueue(step);
            }
        }

        internal void ProcessOneFrame()
        {
            if (!isProcessing)
            {
                isProcessing = true;
                BeginProcessing();
                isProcessing = false;
            }
        }

        void BeginProcessing()
        {
            // loop count is temporary hack to prevent run away code
            var loopCount = 0;
            while (IsTimeLeftInFrame() && (stepsForThisFrame.Count > 0))
            {
                loopCount++;
                if (loopCount > 100000)
                {
                    throw new System.Exception("Exceeded loop count limit");
                }
                ProcessStep(stepsForThisFrame.Dequeue());
            }
            PrepForNextFrame();
            if (loopCount > 1000)
            {
                UnityEngine.Debug.Log("Loop count " + loopCount);
            }
        }

        bool IsTimeLeftInFrame()
        {
            // Ever hopeful for now
            return true;
        }

        void PrepForNextFrame()
        {
            while (stepsForNextFrame.Count > 0)
            {
                ExecutionStep step = stepsForNextFrame.Dequeue();
                step.PrepForFrame();
                AddToQueue(step);
            }
        }

        void ProcessStep(ExecutionStep step)
        {
            try
            {
                // step can do some work and then:
                // * Finish and notify its next step
                // * Add itself back to the queue
                // * Create one or more children to continue work
                //  * Add a child step to the queue (single threaded)
                //  * Add multiple child steps to the queue (doTogether)
                step.Execute();
            }
            catch (TweedleRuntimeException tre)
            {
                UnityEngine.Debug.Log(
                    "*------------------------Exception------------------------*\n" + tre.Message +
                    "\n*----------------------System Stack-----------------------*\n" + tre.StackTrace +
                    "\n*----------------------Tweedle Stack----------------------*\n" + step.CallStack() +
                    "\n*---------------------------------------------------------*");
                // TODO decide how best to handle errors in steps
                throw tre;
            }
        }
    }

}
