namespace Alice.Tweedle
{
    public class TweedleStatement
    {
        public bool isEnabled()
        {
            return true;
        }

        public void execute(VM.TweedleFrame frame)
        {

        }

		// TEMPORARY
		private string name;

		public TweedleStatement(string name)
		{
			this.name = name;
		}
	}
}