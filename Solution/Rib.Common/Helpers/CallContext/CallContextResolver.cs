namespace Rib.Common.Helpers.CallContext
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using JetBrains.Annotations;

    public abstract class CallContextResolver<T>
        where T : class
    {
        [NotNull]
        public abstract string ContextKey { get; }

        public T Current
        {
            get
            {
                var res = CallContext.LogicalGetData(ContextKey);
                if (res == null)
                {
                    return null;
                }
                var ctx = res as T;
                if (ctx == null)
                {
                    throw new InvalidCastException($"{res.GetType()} could not be cast to {typeof(T)}. Context key: {ContextKey}");
                }
                return ctx;
            }
            set { CallContext.LogicalSetData(ContextKey, value); }
        }
    }
}