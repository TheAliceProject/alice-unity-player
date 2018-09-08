using Alice.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Field on an object.
    /// </summary>
    public sealed class TObjectField : TField
    {
        private ITweedleExpression m_Initializer;

        public TObjectField(string inName, TTypeRef inType, MemberFlags inFlags, ITweedleExpression inInitializer = null)
        {
            m_Initializer = inInitializer;
            SetupMember(inName, inType, inFlags | MemberFlags.Instance);
        }

        #region TField

        public override TValue Get(ExecutionScope inScope, ref TValue inValue)
        {
            TObject obj = inValue.Object();
            return obj.Get(Name);
        }

        public override ExecutionStep InitializeStep(ExecutionScope inScope, ref TValue inValue)
        {
            if (m_Initializer != null)
            {
                TObject obj = inValue.Object();
                return m_Initializer.AsStep(inScope)
                    .OnCompletionNotify(
                        new ValueOperationStep("", inScope,
                        (value) =>
                        {
                            CheckSet(inScope, ref value);
                            obj.Set(Name, value);
                        })
                    );
            }

            return null;
        }

        public override void Set(ExecutionScope inScope, ref TValue inValue, TValue inNewValue)
        {
            CheckSet(inScope, ref inNewValue);
            
            TObject obj = inValue.Object();
            obj.Set(Name, inNewValue);
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