using System;
using System.Collections.Generic;
using Alice.Tweedle.File;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;
using Alice.Player.Primitives;
using Alice.Player.Modules;

namespace Alice.Tweedle.Parse
{
    public class TweedleSystem
    {
        public HashSet<ProjectIdentifier> LoadedFiles { get; private set; }

        public Dictionary<string, LibraryManifest> Libraries { get; private set; }
        public Dictionary<string, ProgramDescription> Programs { get; private set; }
        public Dictionary<string, ModelManifest> Models { get; private set; }

        public Dictionary<ResourceIdentifier, ResourceReference> Resources { get; private set; }

        private List<TType> m_AllTypes = new List<TType>();
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
            foreach (var prim in TStaticTypes.ALL_PRIMITIVE_TYPES)
                AddType(prim);

            AddType(TInterop.GenerateType(typeof(DebugModule)));
            
            AddType(TInterop.GenerateType(typeof(AnimationStyleEnum)));
            AddType(TInterop.GenerateType(typeof(DimensionPolicyEnum)));
            AddType(TInterop.GenerateType(typeof(ClockModule)));

            // interop primitives
            AddType(TInterop.GenerateType(typeof(Portion)));
            AddType(TInterop.GenerateType(typeof(Position)));
            AddType(TInterop.GenerateType(typeof(Direction)));
            AddType(TInterop.GenerateType(typeof(Orientation)));
            AddType(TInterop.GenerateType(typeof(VantagePoint)));
            AddType(TInterop.GenerateType(typeof(Angle)));
            AddType(TInterop.GenerateType(typeof(AxisAlignedBox)));
            AddType(TInterop.GenerateType(typeof(Size)));
            AddType(TInterop.GenerateType(typeof(Scale)));
            AddType(TInterop.GenerateType(typeof(Paint)));
            AddType(TInterop.GenerateType(typeof(Color)));

            // properties
            AddType(TInterop.GenerateType(typeof(DecimalNumberProperty)));
            AddType(TInterop.GenerateType(typeof(WholeNumberProperty)));
            AddType(TInterop.GenerateType(typeof(AngleProperty)));
            AddType(TInterop.GenerateType(typeof(PortionProperty)));
            AddType(TInterop.GenerateType(typeof(PositionProperty)));
            AddType(TInterop.GenerateType(typeof(DirectionProperty)));
            AddType(TInterop.GenerateType(typeof(SizeProperty)));
            AddType(TInterop.GenerateType(typeof(ScaleProperty)));
            AddType(TInterop.GenerateType(typeof(OrientationProperty)));
            AddType(TInterop.GenerateType(typeof(AxisAlignedBoxProperty)));
            AddType(TInterop.GenerateType(typeof(VantagePointProperty)));
            AddType(TInterop.GenerateType(typeof(PaintProperty)));
            AddType(TInterop.GenerateType(typeof(ColorProperty)));

            // SceneGraph
            AddType(TInterop.GenerateType(typeof(SceneGraphModule)));
            AddType(TInterop.GenerateType(typeof(SGEntity)));
            AddType(TInterop.GenerateType(typeof(SGBox)));
            AddType(TInterop.GenerateType(typeof(SGSphere)));

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

        public void AddType(TType tweClass)
        {
            Types.Add(tweClass.Name, tweClass);
            m_AllTypes.Add(tweClass);
        }

        public void Link()
        {
            for (int i = 0; i < m_AllTypes.Count; ++i)
            {
                m_AllTypes[i].Link(this);
            }

            for (int i = 0; i < m_AllTypes.Count; ++i)
            {
                m_AllTypes[i].PostLink(this);
            }

            // Sort based on inheritance depth
            m_AllTypes.Sort();

			foreach(var type in m_AllTypes)
			{
                UnityEngine.Debug.Log(TType.DumpOutline(type));
            }
        }

        public void Prep(VirtualMachine virtualMachine)
        {
            ITweedleExpression staticInitializer;
            for (int i = 0; i < m_AllTypes.Count; ++i)
            {
                staticInitializer = m_AllTypes[i].Prep();
                if (staticInitializer != null)
                    virtualMachine.Queue(staticInitializer);
            }
        }

        internal TType TypeNamed(string name)
        {
            TType type;
            Types.TryGetValue(name, out type);
            return type;
        }

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
