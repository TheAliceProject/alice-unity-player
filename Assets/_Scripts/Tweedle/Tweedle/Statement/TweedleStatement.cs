namespace Alice.Tweedle
{
    abstract public class TweedleStatement
    {
        public bool isEnabled()
        {
            return true;
        }

        public void execute(VM.TweedleFrame frame)
        {

        }
	}
}