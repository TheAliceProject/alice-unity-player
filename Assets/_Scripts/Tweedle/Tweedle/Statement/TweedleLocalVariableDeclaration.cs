using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleLocalVariableDeclaration : TweedleStatement
	{
        private TweedleLocalVariable variable;

        public TweedleLocalVariable Variable
        {
            get
            {
                return variable;
            }
        }

        private bool isConstant;

        public bool IsConstant
        {
            get
            {
                return isConstant;
            }
        }

        public TweedleLocalVariableDeclaration(bool isConstant, TweedleLocalVariable variable)
		{
            this.isConstant = isConstant;
            this.variable = variable;
		}
	}
}