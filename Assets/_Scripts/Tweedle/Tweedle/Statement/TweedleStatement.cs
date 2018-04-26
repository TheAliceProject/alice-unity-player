using System;

namespace Alice.Tweedle
{
    abstract public class TweedleStatement
    {
        private bool enabled = true;

        public bool IsEnabled
        {
            get
            {
                return enabled;
            }
        }

        public void Execute(TweedleFrame frame)
        {

        }

		internal void Disable()
		{
            enabled = false;
		}
	}
}