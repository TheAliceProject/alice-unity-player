using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class LocalVariableDeclaration : TweedleStatement
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

        public LocalVariableDeclaration(bool isConstant, TweedleLocalVariable variable)
		{
            this.isConstant = isConstant;
            this.variable = variable;
		}

		public override void Execute(TweedleFrame frame)
		{
			throw new System.NotImplementedException();
		}
	}
}