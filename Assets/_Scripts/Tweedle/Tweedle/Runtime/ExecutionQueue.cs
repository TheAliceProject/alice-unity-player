using System.Collections.Generic;
using Alice.Tweedle;
using Alice.Tweedle.Parsed;
using UnityEngine;

namespace Alice.VM
{
	public class ExecutionQueue
	{
		Queue<ExecutionStep> stepsForThisFrame = new Queue<ExecutionStep>();
		Queue<ExecutionStep> blockedSteps = new Queue<ExecutionStep>();
		Queue<ExecutionStep> stepsForNextFrame = new Queue<ExecutionStep>();
		List<ExecutionStep> allQueuedSteps = new List<ExecutionStep>();

		internal void AddToQueue(ExecutionStep step)
		{
			if (step == null || step.IsComplete())
			{
				return;
			}
			if (allQueuedSteps.Contains(step))
			{
				// This may happen if a task blocks multiple other tasks. Do not queue it again.
				//UnityEngine.Debug.Log("Tried to queue execution step twice " + step);
				return;
			}
			allQueuedSteps.Add(step);
			//UnityEngine.Debug.Log("Step count " + allQueuedSteps.Count);
			if (step.CanRunThisFrame())
			{
				if (step.IsBlocked())
				{
					AddToQueue(step.IncompleteBlockingSteps());
					//UnityEngine.Debug.Log("Queueing blocked step " + step);
					blockedSteps.Enqueue(step);
				}
				else
				{
					//UnityEngine.Debug.Log("Queueing immediate step " + step);
					stepsForThisFrame.Enqueue(step);
				}
			}
			else
			{
				//UnityEngine.Debug.Log("Queueing next frame step " + step);
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

		internal IEnumerator<ExecutionStep> ProcessQueues()
		{
			// loop count is temporary hack to prevent run away code
			var loopCount = 0;
			while (IsRunning())
			{
				while (IsTimeLeftInFrame() && (stepsForThisFrame.Count > 0 || blockedSteps.Count > 0))
				{
					loopCount++;
					if (loopCount > 5000)
						return null;
					while (stepsForThisFrame.Count > 0)
					{
						//UnityEngine.Debug.Log("Dequeueing step ");
						ProcessStep(stepsForThisFrame.Dequeue());
					}
					var blockedCount = blockedSteps.Count;
					for (int i = 0; i < blockedCount; i++)
					//while (blockedSteps.Count > 0)
					{
						var step = blockedSteps.Dequeue();
						//UnityEngine.Debug.Log("Moving blocked step to this frame " + step);
						if (step.CanRunThisFrame())
						{
							if (step.IsBlocked())
							{
								blockedSteps.Enqueue(step);
							}
							else
							{
								stepsForThisFrame.Enqueue(step);
							}
						}
						else
						{
							//UnityEngine.Debug.Log("Requeueing next frame");
							stepsForNextFrame.Enqueue(step);
						}
					}
				}
				PrepForNextFrame();
				//yield return null;
			}
			return null;
		}

		bool IsRunning()
		{
			// stop running and wait for event to start up again
			return stepsForThisFrame.Count > 0 ||
									blockedSteps.Count > 0 ||
									stepsForNextFrame.Count > 0;
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
				ExecutionStep step = stepsForThisFrame.Dequeue();
				allQueuedSteps.Remove(step);
				step.PrepForFrame();
				AddToQueue(step);
			}
		}

		void ProcessStep(ExecutionStep step)
		{
			if (step.IsBlocked())
			{
				//UnityEngine.Debug.Log("Blocked. Requeueing " + step);
				blockedSteps.Enqueue(step);
			}
			else
			{
				allQueuedSteps.Remove(step);
				if (!step.Execute())
				{
					//UnityEngine.Debug.Log("Requeueing");
					AddToQueue(step);
				}
			}
		}
	}

}
