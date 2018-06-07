using System.Collections.Generic;

namespace Alice.Tweedle
{
	public interface InvocableMethodHolder
	{
		void Invoke(TweedleFrame frame, TweedleObject target, string methodName, Dictionary<string, TweedleValue> arguments);
      
		TweedleMethod MethodNamed(string methodName);
      
		string Name
		{
			get;
		}
	}
}