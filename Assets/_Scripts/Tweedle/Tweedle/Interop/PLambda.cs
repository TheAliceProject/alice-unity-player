using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.VM;

namespace Alice.Tweedle.Interop
{
    public abstract class PLambdaBase
    {
        private TLambda m_Lambda;
        private ExecutionScope m_Scope;

        protected PLambdaBase(TLambda inLambda, ExecutionScope inScope)
        {
            m_Lambda = inLambda;
            m_Scope = inScope;
        }

        static private ITweedleExpression[] BuildLambdaArgs(ExecutionScope inScope, object[] inArgs)
        {
            ITweedleExpression[] args = new ITweedleExpression[inArgs.Length];
            for (int i = 0; i < args.Length; ++i)
                args[i] = TInterop.ToTValue(inArgs[i], inScope);
            return args;
        }

        protected AsyncReturn InvokeNoReturnType(params object[] inArgs)
        {
            ITweedleExpression[] argsAsExpressions = BuildLambdaArgs(m_Scope, inArgs);
            var lambdaRes = m_Lambda.QueueEvaluation(argsAsExpressions);
            
            AsyncReturn returnVal = new AsyncReturn();
            lambdaRes.OnReturn((tv) => returnVal.Return());
            return returnVal;
        }

        protected AsyncReturn<T> InvokeWithReturnType<T>(params object[] inArgs)
        {
            ITweedleExpression[] argsAsExpressions = BuildLambdaArgs(m_Scope, inArgs);
            var lambdaRes = m_Lambda.QueueEvaluation(argsAsExpressions);
            
            AsyncReturn<T> returnVal = new AsyncReturn<T>();
            lambdaRes.OnReturn((tv) => returnVal.Return((T)TInterop.ToPObject(tv, typeof(T), m_Scope)));
            return returnVal;
        }
    }

    #region PAction (no return type)

    public sealed class PAction : PLambdaBase
    {
        public PAction(TLambda inLambda, ExecutionScope inScope)
            : base(inLambda, inScope)
        {
        }

        public AsyncReturn Call()
        {
            return InvokeNoReturnType();
        }
    }

    public sealed class PAction<T> : PLambdaBase
    {
        public PAction(TLambda inLambda, ExecutionScope inScope)
            : base(inLambda, inScope)
        {
        }

        public AsyncReturn Call(T inArg1)
        {
            return InvokeNoReturnType(inArg1);
        }
    }

    public sealed class PAction<T, U> : PLambdaBase
    {
        public PAction(TLambda inLambda, ExecutionScope inScope)
            : base(inLambda, inScope)
        {
        }

        public AsyncReturn Call(T inArg1, U inArg2)
        {
            return InvokeNoReturnType(inArg1, inArg2);
        }
    }

    public sealed class PAction<T, U, V> : PLambdaBase
    {
        public PAction(TLambda inLambda, ExecutionScope inScope)
            : base(inLambda, inScope)
        {
        }

        public AsyncReturn Call(T inArg1, U inArg2, V inArg3)
        {
            return InvokeNoReturnType(inArg1, inArg2, inArg3);
        }
    }

    public sealed class PAction<T, U, V, W> : PLambdaBase
    {
        public PAction(TLambda inLambda, ExecutionScope inScope)
            : base(inLambda, inScope)
        {
        }

        public AsyncReturn Call(T inArg1, U inArg2, V inArg3, W inArg4)
        {
            return InvokeNoReturnType(inArg1, inArg2, inArg3, inArg4);
        }
    }
    
    #endregion // PAction (no return type)

    #region PFunc (return type)

    public sealed class PFunc<R> : PLambdaBase
    {
        public PFunc(TLambda inLambda, ExecutionScope inScope)
            : base(inLambda, inScope)
        {
        }

        public AsyncReturn<R> Call()
        {
            return InvokeWithReturnType<R>();
        }
    }

    public sealed class PFunc<R, T> : PLambdaBase
    {
        public PFunc(TLambda inLambda, ExecutionScope inScope)
            : base(inLambda, inScope)
        {
        }

        public AsyncReturn<R> Call(T inArg1)
        {
            return InvokeWithReturnType<R>(inArg1);
        }
    }

    public sealed class PFunc<R, T, U> : PLambdaBase
    {
        public PFunc(TLambda inLambda, ExecutionScope inScope)
            : base(inLambda, inScope)
        {
        }

        public AsyncReturn<R> Call(T inArg1, U inArg2)
        {
            return InvokeWithReturnType<R>(inArg1, inArg2);
        }
    }

    public sealed class PFunc<R, T, U, V> : PLambdaBase
    {
        public PFunc(TLambda inLambda, ExecutionScope inScope)
            : base(inLambda, inScope)
        {
        }

        public AsyncReturn<R> Call(T inArg1, U inArg2, V inArg3)
        {
            return InvokeWithReturnType<R>(inArg1, inArg2, inArg3);
        }
    }

    public sealed class PFunc<R, T, U, V, W> : PLambdaBase
    {
        public PFunc(TLambda inLambda, ExecutionScope inScope)
            : base(inLambda, inScope)
        {
        }

        public AsyncReturn<R> Call(T inArg1, U inArg2, V inArg3, W inArg4)
        {
            return InvokeWithReturnType<R>(inArg1, inArg2, inArg3, inArg4);
        }
    }

    #endregion // PFunc (return type)
}