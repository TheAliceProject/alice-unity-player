using System;
using System.Collections.Generic;
using Alice.Tweedle.File;
using Alice.VM;

namespace Alice.Tweedle.Parse
{
	public class TweedleSystem
	{
		public HashSet<ProjectIdentifier> LoadedFiles { get; private set; }

		public Dictionary<string, LibraryManifest> Libraries { get; private set; }
		public Dictionary<string, ProgramDescription> Programs { get; private set; }
		public Dictionary<string, ModelManifest> Models { get; private set; }

		public Dictionary<ResourceIdentifier, ResourceReference> Resources { get; private set; }

		// public Dictionary<string, TClassType> Classes { get; private set; }
		// Dictionary<string, TweedlePrimitiveClass> primitives;
		// public Dictionary<string, TweedleEnum> Enums { get; private set; }
		public Dictionary<string, TType> Types { get; private set; }

        public TweedleSystem()
		{
			LoadedFiles = new HashSet<ProjectIdentifier>();

			Libraries = new Dictionary<string, LibraryManifest>();
			Programs = new Dictionary<string, ProgramDescription>();
			Models = new Dictionary<string, ModelManifest>();

			Resources = new Dictionary<ResourceIdentifier, ResourceReference>();

			// Classes = new Dictionary<string, TClassType>();
			// Enums = new Dictionary<string, TweedleEnum>();
			Types = new Dictionary<string, TType>();
            InitializePrimitives();
		}

		private void InitializePrimitives()
		{
			foreach(var prim in TStaticTypes.ALL_PRIMITIVE_TYPES)
                AddClass(prim);

            AddClass(new TPObjectType(typeof(Modules.DebugModule)));
            AddClass(new TPObjectType(typeof(Modules.Portion)));
            // primitives = new Dictionary<string, TweedlePrimitiveClass>();
        }

		public void AddLibrary(LibraryManifest libAsset)
		{
			LoadedFiles.Add(libAsset.Identifier);
			Libraries.Add(libAsset.Identifier.id, libAsset);
		}

		public void AddProgram(ProgramDescription programAsset)
		{
			LoadedFiles.Add(programAsset.Identifier);
			Programs.Add(programAsset.Identifier.id, programAsset);
		}

		public void AddModel(ModelManifest modelAsset)
		{
			LoadedFiles.Add(modelAsset.Identifier);
			Models.Add(modelAsset.Identifier.id, modelAsset);
		}

		public void AddClass(TType tweClass)
		{
			Types.Add(tweClass.Name, tweClass);

		}

		public void Resolve()
		{
            foreach (var typeVal in Types.Values)
            {
                TType.Finalize(this, typeVal);
                // UnityEngine.Debug.Log(TType.Dump(typeVal));
            }
        }

		internal TType ClassNamed(string name)
		{
			return Types[name];
		}

		// internal TweedleEnum EnumNamed(string name)
		// {
		// 	return Enums[name];
		// }

		internal TType TypeNamed(string name)
		{
            TType type;
            Types.TryGetValue(name, out type);
            return type;
            // if (name.StartsWith("$", StringComparison.Ordinal))
			// {
			// 	return PrimitiveDeclaration(name);
			// }
			// if (Classes.ContainsKey(name))
			// {
			// 	return Classes[name];
			// }
			// if (Enums.ContainsKey(name))
			// {
			// 	return Enums[name];
			// }
			// return null;
		}

		// TType PrimitiveDeclaration(string name)
		// {
		// 	if (primitives.ContainsKey(name))
		// 	{
		// 		return primitives[name];
		// 	}
		// 	UnityEngine.Debug.LogError("Attempt to invoke missing primitive namespace " + name);
		// 	return new AbsentPrimitiveClassStub(name);
		// }

		// public void AddEnum(TweedleEnum tweEnum)
		// {
		// 	Enums.Add(tweEnum.Name, tweEnum);
		// 	Types.Add(tweEnum.Name, tweEnum);
		// }

		public void AddResource(ResourceReference resourceAsset)
		{
			ResourceIdentifier identifier = new ResourceIdentifier(resourceAsset.id, resourceAsset.ContentType, resourceAsset.FormatType);
			Resources.Add(identifier, resourceAsset);
		}

		internal void QueueProgramMain(VirtualMachine vm)
		{
			TType prog;
			vm.Initialize(this);
			if (Types.TryGetValue("Program", out prog))
			{
				TValue progVal = TStaticTypes.TYPE_REF.Instantiate(prog);
				NamedArgument[] arguments = NamedArgument.EMPTY_ARGS;
                vm.Queue(new MethodCallExpression(progVal, "main", arguments));
			}
		}
	}
}
