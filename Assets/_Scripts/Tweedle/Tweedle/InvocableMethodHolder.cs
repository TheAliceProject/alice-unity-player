namespace Alice.Tweedle
{
	public interface InvocableMethodHolder
	{
		void Invoke(TweedleFrame frame, TweedleObject target, TweedleMethod method, TweedleValue[] arguments);

		string Name
		{
			get;
		}
	}
}