using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;
using Alice.Utils;

namespace Alice.Tweedle
{
    /// <summary>
    /// Generic invokable method.
    /// </summary>
    public abstract class TMethod : ITypeMember
    {
        static public readonly string ConstructorName = ConstructorInfo.ConstructorName;

        #region ITypeMember

        public string Name { get; private set; }
        public TTypeRef Type { get; private set; }
        public MemberFlags Flags { get; private set; }

        public virtual void Link(TAssemblyLinkContext inContext, TType inOwnerType)
        {
            Type.Resolve(inContext);
            ReturnType.Resolve(inContext);

            // Interop methods don't go through the same type checks
            bool bIsInterop = this.HasModifiers(MemberFlags.PInterop);
            
            if (!bIsInterop && !ReturnType.Get().IsValidReturnType())
                throw new TweedleLinkException("Return type " + ReturnType.Name + " is not a valid return type");

            for (int i = 0; i < RequiredParams.Length; ++i)
            {
                RequiredParams[i].Type.Resolve(inContext);
                if (!bIsInterop && !RequiredParams[i].Type.Get().IsValidIdentifier())
                    throw new TweedleLinkException("Parameter type " + RequiredParams[i].Type.Name + " is not a valid identifier type");
            }
            for (int i = 0; i < OptionalParams.Length; ++i)
            {
                OptionalParams[i].Type.Resolve(inContext);
                if (!bIsInterop && !OptionalParams[i].Type.Get().IsValidIdentifier())
                    throw new TweedleLinkException("Parameter type " + OptionalParams[i].Type.Name + " is not a valid identifier type");
            }
        }

        #endregion // ITypeMember

        public TTypeRef ReturnType { get; private set; }
        public TParameter[] RequiredParams { get; private set; }
        public TParameter[] OptionalParams { get; private set; }

        // This is for fast retrieval of parameters by name
        private Dictionary<string, TParameter> m_RecognizedParams;

        #region Construction

        private void BuildParamMap()
        {
            m_RecognizedParams = new Dictionary<string, TParameter>(RequiredParams.Length + OptionalParams.Length);

            for (int i = 0; i < RequiredParams.Length; ++i)
            {
                var param = RequiredParams[i];
                m_RecognizedParams.Add(param.Name, param);
            }

            for (int i = 0; i < OptionalParams.Length; ++i)
            {
                var param = OptionalParams[i];
                m_RecognizedParams.Add(param.Name, param);
            }
        }

        protected void SetupMember(string inName, TTypeRef inType, MemberFlags inFlags)
        {
            Name = inName;
            Type = inType;
            Flags = inFlags | MemberFlags.Method;
        }

        protected void SetupSignature(TTypeRef inReturnType, TParameter[] inRequired = null, TParameter[] inOptional = null)
        {
            ReturnType = inReturnType;
            RequiredParams = inRequired ?? TParameter.EMPTY_PARAMS;
            OptionalParams = inOptional ?? TParameter.EMPTY_PARAMS;

            BuildParamMap();
        }

        #endregion // Construction

        /// <summary>
        /// Returns if this is a static method.
        /// </summary>
        public bool IsStatic()
        {
            return this.HasModifiers(MemberFlags.Static);
        }

        /// <summary>
        /// Returns if the given arguments are valid for invoking this method.
        /// </summary>
        public bool ExpectsArgs(NamedArgument[] inArguments)
        {
            int requiredArgsFound = 0;
            for (int i = 0; i < inArguments.Length; ++i)
            {
                TParameter param;
                if (!m_RecognizedParams.TryGetValue(inArguments[i].Name, out param))
                    return false;
                
                if (param.Required)
                    ++requiredArgsFound;
            }

            return requiredArgsFound == RequiredParams.Length;
        }

        public ExecutionStep AsStep(string inCallstackEntry, InvocationScope inScope, NamedArgument[] inArguments)
        {
            StepSequence main = new StepSequence(inCallstackEntry, inScope);
            AddInvocationSteps(inScope, main, inArguments);
            return main;
        }
        
        #region Invocation Steps

        /// <summary>
        /// Adds invocation steps for calling this method with the given arguments.
        /// </summary>
        public void AddInvocationSteps(InvocationScope inScope, StepSequence ioSteps, NamedArgument[] inArguments)
        {
            AddPrepSteps(inScope, ioSteps, inArguments);
            AddBodyStep(inScope, ioSteps);
            ioSteps.AddStep(ResultsStep(inScope));
        }

        // PREP

        protected virtual void AddPrepSteps(InvocationScope inScope, StepSequence ioMain, NamedArgument[] inArguments)
        {
            AddArgumentSteps(inScope, ioMain, inArguments);
        }

        private void AddArgumentSteps(InvocationScope inScope, StepSequence ioMain, NamedArgument[] inArguments)
        {
            using (PooledSet<string> paramNameSet = PooledSet<string>.Alloc()) {

                for (int i = 0; i < inArguments.Length; ++i)
                {
                    TParameter param;
                    if (!m_RecognizedParams.TryGetValue(inArguments[i].Name, out param))
                    {
                        throw new TweedleLinkException("Invalid method call on " + Name + ". Unrecognized parameter " + inArguments[i].Name);
                    }

                    paramNameSet.Add(param.Name);
                    ioMain.AddStep(ArgumentStep(inScope, param, inArguments[i].Argument));
                }

                // If we've already counted as many parameters as are possible
                // for this method, we can exit out early
                if (paramNameSet.Count >= m_RecognizedParams.Count)
                {
                    return;
                }

                for (int i = 0; i < RequiredParams.Length; ++i)
                {
                    if (!paramNameSet.Contains(RequiredParams[i].Name))
                    {
                        throw new TweedleLinkException("Invalid method call on " + Name + ". Missing value for required parameter " + RequiredParams[i].Name);
                    }
                }

                for (int i = 0; i < OptionalParams.Length; ++i)
                {
                    TParameter optionalParam = OptionalParams[i];
                    if (paramNameSet.Add(optionalParam.Name))
                    {
                        ioMain.AddStep(DefaultValueArgumentStep(inScope, optionalParam, optionalParam.Initializer));
                    }
                }
            }
        }

        private ExecutionStep ArgumentStep(InvocationScope inScope, TValueHolderDeclaration inArgDeclaration, ITweedleExpression inExpression)
        {
            ExecutionStep argStep = inExpression.AsStep(inScope.callingScope);
            ExecutionStep storeStep = new ValueComputationStep(
                Name + " Arg " + inArgDeclaration.Name,
                inScope.callingScope,
                arg => inScope.SetLocalValue(inArgDeclaration, arg)
            );
            argStep.OnCompletionNotify(storeStep);
            return argStep;
        }

        private ExecutionStep DefaultValueArgumentStep(InvocationScope inScope, TValueHolderDeclaration inArgDeclaration, ITweedleExpression inExpression)
        {
            ExecutionStep argStep = inExpression.AsStep(inScope);
            ExecutionStep storeStep = new ValueComputationStep(
                Name + " Default for Arg " + inArgDeclaration.Name,
                inScope,
                arg => inScope.SetLocalValue(inArgDeclaration, arg)
            );
            argStep.OnCompletionNotify(storeStep);
            return argStep;
        }

        // BODY

        protected abstract void AddBodyStep(InvocationScope inScope, StepSequence ioMain);

        // RETURN

        private ExecutionStep ResultsStep(InvocationScope inScope)
        {
            return new ValueComputationStep("call", inScope, arg => inScope.Result);
        }

        #endregion // Invocation Steps

        static public readonly TMethod[] EMPTY_ARRAY = new TMethod[0];
    }
}