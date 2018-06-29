﻿using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	abstract public class TweedlePrimitive : TweedleTypeDeclaration
	{
		public TweedlePrimitive(String name, List<TweedleField> properties, List<TweedleMethod> methods)
			: base(name, properties, methods, new List<TweedleConstructor>())
		{
		}

		public override TweedleMethod MethodNamed(TweedleFrame frame, string methodName)
		{
			TweedleMethod method = base.MethodNamed(frame, methodName);
			if (method != null)
			{
				return method;
			}
			return new AbsentMethodStub(methodName);
		}
	}
}
