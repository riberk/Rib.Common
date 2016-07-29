namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.Owin;
    using Rib.Common.Application.Wrappers;

    /// <summary>
    /// Инициализирует врапперы
    /// </summary>
    public class InitializeWrappersMiddleware : OwinMiddleware
    {
        [NotNull] private readonly IWrappersManager _wrappersManager;

        /// <summary>
        ///     Instantiates the middleware with an optional pointer to the next component.
        /// </summary>
        /// <param name="next" />
        /// <param name="wrappersManager"></param>
        public InitializeWrappersMiddleware(OwinMiddleware next, [NotNull] IWrappersManager wrappersManager) : base(next)
        {
            if (wrappersManager == null) throw new ArgumentNullException(nameof(wrappersManager));
            _wrappersManager = wrappersManager;
        }

        /// <summary>
        ///     Process an individual request.
        /// </summary>
        /// <param name="context" />
        /// <returns />
        public override async Task Invoke(IOwinContext context)
        {
            _wrappersManager.InitializeAll();
            await Next.Invoke(context);
            _wrappersManager.DisposeAll();
        }
    }
}