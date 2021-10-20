using System;
using System.Collections.Generic;
using System.Linq;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class LambdaExpression : TweedleExpression
    {
        private TLambdaType m_LambdaType;
        private TParameter[] m_Parameters;
        private BlockStatement m_Body;

        public TParameter[] Parameters
        {
            get { return m_Parameters; }
        }

        public BlockStatement Body
        {
            get { return m_Body; }
        }

        public LambdaExpression(TLambdaType type, TParameter[] parameters, TweedleStatement[] statements)
            : base(type)
        {
            m_LambdaType = type;
            m_Parameters = parameters;
            m_Body = new BlockStatement(statements);
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            return new ValueStep(this, scope, m_LambdaType.Instantiate(this, scope));
        }
    }
}