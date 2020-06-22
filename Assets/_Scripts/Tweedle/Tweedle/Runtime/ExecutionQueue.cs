using System;
using System.Collections.Generic;

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
            if (isProcessing) return;

            isProcessing = true;
            try
            {
                BeginProcessing();
            }
            finally
            {
                isProcessing = false;
            }
        }

        void BeginProcessing()
        {
            while (IsTimeLeftInFrame() && (stepsForThisFrame.Count > 0))
            {
                ProcessStep(stepsForThisFrame.Dequeue());
            }
            PrepForNextFrame();
        }

        bool IsTimeLeftInFrame()
        {
            // TODO check Unity engine for frame timing
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
                UnityEngine.Debug.LogErrorFormat(TweedleExceptionFormat, tre.Message, step.CallStack(), tre.StackTrace);
                throw;
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException != e) {
                    UnityEngine.Debug.LogErrorFormat(SecondaryExceptionFormat, e.Message, step.CallStack(), e.StackTrace, e.InnerException.StackTrace);
                } else {
                    UnityEngine.Debug.LogErrorFormat(SystemExceptionFormat, e.Message, step.CallStack(), e.StackTrace);
                }
                throw new TweedleRuntimeException(e);
            }
        }
        
        const string TweedleExceptionFormat = "*----------------------Tweedle Exception------------------*\n{0}\n" +
                                              "*----------------------Tweedle Stack----------------------*\n{1}\n" +
                                              "*----------------------System Stack-----------------------*\n{2}\n" +
                                              "*---------------------------------------------------------*";
        const string SystemExceptionFormat = "*----------------------System Exception-------------------*\n{0}\n" +
                                             "*----------------------Tweedle Stack----------------------*\n{1}\n" +
                                             "*----------------------System Stack-----------------------*\n{2}\n" +
                                             "*---------------------------------------------------------*";
        const string SecondaryExceptionFormat = "*----------------------System Exception-------------------*\n{0}\n" +
                                                "*----------------------Tweedle Stack----------------------*\n{1}\n" +
                                                "*----------------------System Stack-----------------------*\n{2}\n" +
                                                "*----------------------Inner Stack------------------------*\n{3}\n" +
                                                "*---------------------------------------------------------*";
    }

}
