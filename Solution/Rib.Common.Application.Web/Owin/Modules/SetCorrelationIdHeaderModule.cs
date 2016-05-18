namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.Owin;
    using Rib.Common.Helpers.CorrelationId;

    /// <summary>
    /// Проставляет заголовок CorrelationId
    /// </summary>
    public class SetCorrelationIdHeaderMiddleware : OwinMiddleware
    {
        [NotNull] private readonly ICorrelationIdStore _store;

        /// <summary>
        ///     Instantiates the middleware with an optional pointer to the next component.
        /// </summary>
        /// <param name="next" />
        /// <param name="store"></param>
        public SetCorrelationIdHeaderMiddleware(OwinMiddleware next, [NotNull] ICorrelationIdStore store) : base(next)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));
            _store = store;
        }

        /// <summary>
        ///     Process an individual request.
        /// </summary>
        /// <param name="context" />
        /// <returns />
        public override Task Invoke(IOwinContext context)
        {
            context.Response.Headers.Append("X-CORRELATIONID", _store.Read()?.ToString());
            return Next.Invoke(context);
        }
    }
}