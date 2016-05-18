namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.Owin;

    public class SetOwinContextToCallContextMiddleware : OwinMiddleware
    {
        [NotNull]
        private readonly string _key;

        /// <summary>
        ///     Instantiates the middleware with an optional pointer to the next component.
        /// </summary>
        /// <param name="next" />
        /// <param name="key">Ключ контекста</param>
        public SetOwinContextToCallContextMiddleware(OwinMiddleware next, [NotNull] string key) : base(next)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            _key = key;
        }

        /// <summary>
        ///     Process an individual request.
        /// </summary>
        /// <param name="context" />
        /// <returns />
        public override async Task Invoke(IOwinContext context)
        {
            CallContext.LogicalSetData(_key, context);
            await Next.Invoke(context);
        }
    }
}