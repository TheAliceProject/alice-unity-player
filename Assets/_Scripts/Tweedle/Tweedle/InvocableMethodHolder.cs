namespace Alice.Tweedle
{
	public interface InvocableMethodHolder
	{      
		TweedleMethod MethodNamed(string methodName);
      
		string Name { get; }
	}
}