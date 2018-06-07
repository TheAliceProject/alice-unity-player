using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleMethod
    {
		private BlockStatement body;
		private List<string> modifiers;
		private TweedleType resultType;
        private string name;
        private List<TweedleRequiredParameter> required;
        private List<TweedleOptionalParameter> optional;

		public string Name
		{
			get { return name; }
		}

        public TweedleType Type
        {
			get { return resultType; }
        }

        public List<TweedleRequiredParameter> RequiredParameters
        {
            get { return required; }
        }

        public List<TweedleOptionalParameter> OptionalParameters
        {
            get { return optional; }
        }

		public BlockStatement Body
        {
            get { return body; }
        }

		public TweedleMethod(TweedleType resultType, string name, List<TweedleRequiredParameter> required, List<TweedleOptionalParameter> optional, List<TweedleStatement> statements)
		{
			this.resultType = resultType;
			this.name = name;
			this.required = required;
			this.optional = optional;
			this.body = new BlockStatement(statements);
		}

		public TweedleMethod(List<string> modifiers, TweedleType type, string name, List<TweedleRequiredParameter> required, List<TweedleOptionalParameter> optional, List<TweedleStatement> statements)
			: this(type, name, required, optional, statements)
		{
			this.modifiers = modifiers;
		}

        internal TweedleValue Invoke(TweedleFrame frame, Dictionary<string, TweedleExpression> arguments)
        {
            TweedleFrame staticFrame = frame.MethodCallFrame(this);
            EvaluateArguments(staticFrame, arguments);
            //staticFrame.Execute(body);
            //return staticFrame.Result();
            Dictionary<string, TweedleValue> argValues = new Dictionary<string, TweedleValue>();
            foreach (KeyValuePair<string, TweedleExpression> pair in arguments)
            {
				pair.Value.Evaluate(frame.ExecutionFrame(value => argValues.Add(pair.Key, value)));
                //argValues.Add(pair.Key, pair.Value.Evaluate(frame));
            }
            throw new NotImplementedException();
        }

		private void EvaluateArguments(TweedleFrame frame, Dictionary<string, TweedleExpression> arguments)
		{
			foreach(TweedleRequiredParameter req in RequiredParameters)
			{
				TweedleExpression argExp;
				if (arguments.TryGetValue(req.Name, out argExp))
				{
					argExp.Evaluate(frame.ExecutionFrame(value => frame.SetParameterValue(req.GetType(), value)));
					//frame.SetParameterValue(req.GetType(), argExp.Evaluate(frame));
				}
				else
				{
					throw new TweedleLinkException("Invalid method call on " + Name + ". Missing value for required parameter " + req.Name);
					//Missing required arg
				}
			}
			foreach (TweedleOptionalParameter opt in OptionalParameters)
            {
                TweedleExpression argExp;
                if (arguments.TryGetValue(opt.Name, out argExp))
                {
					argExp.Evaluate(frame.ExecutionFrame(value => frame.SetParameterValue(opt.GetType(), value)));
                    //frame.SetParameterValue(opt.GetType(), argExp.Evaluate(frame));
                }
                else
				{
					opt.GetInitializer().Evaluate(frame.ExecutionFrame(value => frame.SetParameterValue(opt.GetType(), value)));
					//frame.SetParameterValue(opt.GetType(), opt.EvaluateDefault(frame));
                }
            }
		}

		public TweedleValue Invoke(TweedleFrame frame, TweedleObject target, Dictionary<string, TweedleValue> arguments)
		{
			//TweedleFrame thisFrame = frame.InstanceMethodFrame(target);
			//EvaluateArguments(thisFrame, arguments);
			//thisFrame.Execute(body);
			//return thisFrame.Result();
            throw new System.NotImplementedException();
		}

		public TweedleValue Invoke(TweedleFrame frame, Dictionary<string, TweedleValue> arguments)
        {
			//TweedleFrame staticFrame = frame.StaticMethodFrame();
			//CheckArguments(staticFrame, arguments);
			//staticFrame.Execute(body);
			//return staticFrame.Result();
            throw new System.NotImplementedException();
        }

		public bool IsStatic()
		{
			return modifiers.Contains("static");
		}
	}
}