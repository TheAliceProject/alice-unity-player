using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Static field on a TType.
    /// </summary>
    public sealed class TStaticField : TField
    {
        private ITweedleExpression m_Initializer;
        private TObject m_StaticStorage;

        public TStaticField(string inName, TTypeRef inType, MemberFlags inFlags)
        {
            SetupMember(inName, inType, inFlags | MemberFlags.Static);
        }

        public TStaticField(string inName, TTypeRef inType, MemberFlags inFlags, ITweedleExpression inInitializer)
            : this(inName, inType, inFlags)
        {
            m_Initializer = inInitializer;
        }

        #region TField

        public override void Link(TAssemblyLinkContext inContext, TType inOwnerType)
        {
            base.Link(inContext, inOwnerType);
            
            // This is stored here instead of being retrieved from a TTypeRef
            // so that static fields accessed through subclasses are still
            // retrieved from the correct location
            // For example:
            //      class A { static int staticVal; }
            //      class B { }
            //      B.staticVal is just an alias for A.staticVal.
            //      We need to be sure we're retrieving the value from A's static storage, and not B's.
            m_StaticStorage = inOwnerType.StaticStorage();
        }

        public override TValue Get(ExecutionScope inScope, ref TValue inValue)
        {
            return m_StaticStorage.Get(Name);
        }

        public override bool HasInitializer()
        {
            return m_Initializer != null;
        }

        public override ExecutionStep InitializeStep(ExecutionScope inScope, ref TValue inValue)
        {
            if (m_Initializer != null)
            {
                TValue _this = inValue;
                return m_Initializer.AsStep(inScope)
                    .OnCompletionNotify(
                        new ValueOperationStep("", inScope,
                        (value) =>
                        {
                            CheckSet(inScope, ref _this, ref value);
                            m_StaticStorage.Set(Name, value);
                        })
                    );
            }

            return null;
        }

        public override void Set(ExecutionScope inScope, ref TValue inValue, TValue inNewValue)
        {
            CheckSet(inScope, ref inValue, ref inNewValue);
            m_StaticStorage.Set(Name, inNewValue);
        }

        #endregion // TField

        public override string ToTweedle()
        {
            string twe = base.ToTweedle();
            if (m_Initializer != null)
            {
                twe += " <- " + m_Initializer.ToTweedle();
            }
            return twe;
        }
    }
}