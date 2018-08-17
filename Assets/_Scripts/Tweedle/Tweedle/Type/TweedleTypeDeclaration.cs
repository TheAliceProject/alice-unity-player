﻿using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle
{
	public abstract class TweedleTypeDeclaration : TweedleType
	{
		protected List<TweedleField> properties;
		protected List<TweedleMethod> methods;
		protected List<TweedleConstructor> constructors;

		public List<TweedleField> Properties
		{
			get { return properties; }
		}

		public List<TweedleMethod> Methods
		{
			get { return methods; }
		}

		public List<TweedleConstructor> Constructors
		{
			get { return constructors; }
		}

		public TweedleTypeDeclaration(string name,
			List<TweedleField> properties,
			List<TweedleMethod> methods,
			List<TweedleConstructor> constructors)
			: base(name)
		{
			DeclarationListInitializer(properties, methods, constructors);
		}

		public TweedleTypeDeclaration(string name, TweedleTypeReference super,
			List<TweedleField> properties,
			List<TweedleMethod> methods,
			List<TweedleConstructor> constructors)
			: base(name, super)
		{
			DeclarationListInitializer(properties, methods, constructors);
		}

		public TweedleTypeDeclaration(string name, string super,
			List<TweedleField> properties,
			List<TweedleMethod> methods,
			List<TweedleConstructor> constructors)
			: base(name, new TweedleTypeReference(super))
		{
			DeclarationListInitializer(properties, methods, constructors);
		}

		private void DeclarationListInitializer(
			List<TweedleField> properties,
			List<TweedleMethod> methods,
			List<TweedleConstructor> constructors)
		{
			this.properties = properties;
			this.methods = methods;
			this.constructors = constructors;
		}

		internal virtual TweedleField Field(ExecutionScope scope, string fieldName)
		{
			return properties.Where(prop => prop.Name.Equals(fieldName))
							 .DefaultIfEmpty(null)
							 .First();
		}

		public virtual TweedleMethod MethodNamed(ExecutionScope scope, string methodName)
		{
			return Methods.Find((TweedleMethod method) => method.Name.Equals(methodName));
		}
	}
}