using System.Collections.Generic;

namespace Alice.Tweedle.Linker
{
    public class Linker
    {
		public Tweedle.TweedleProgram Link(Alice.Tweedle.Parsed.TweedleSystem unlinkedSystem)
        {
            // (paritally) order classes
            // link the class heirarchy
            // then go through each class method
            // if depends on child class throw error (gracefully)
            return null;
        }
    }
}