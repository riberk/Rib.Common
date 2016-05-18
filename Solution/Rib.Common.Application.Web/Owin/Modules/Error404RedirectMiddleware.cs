namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.Owin;

    public class Error404RedirectMiddleware : OwinMiddleware
    {
        [NotNull] private readonly string _redirectToPage;

        /// <summary>
        /// Instantiates the middleware with an optional pointer to the next component.
        /// </summary>
        /// <param name="next"/>
        /// <param name="redirectToPage"></param>
        public Error404RedirectMiddleware(OwinMiddleware next, [NotNull] string redirectToPage) : base(next)
        {
            if (redirectToPage == null) throw new ArgumentNullException(nameof(redirectToPage));
            _redirectToPage = redirectToPage;
        }

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context"/>
        /// <returns/>
        public override Task Invoke(IOwinContext context)
        {
            context.Response.Redirect(_redirectToPage);
            return Task.CompletedTask;
        }
    }
}