namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Threading.Tasks;
    using global::Common.Logging;
    using JetBrains.Annotations;
    using Microsoft.Owin;

    public class LoggerMiddleware : OwinMiddleware
    {
        [NotNull] private readonly ILog _logger;

        /// <summary>
        ///     Instantiates the middleware with an optional pointer to the next component.
        /// </summary>
        /// <param name="next" />
        /// <param name="logger"></param>
        public LoggerMiddleware(OwinMiddleware next, [NotNull] ILog logger) : base(next)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        /// <summary>
        ///     Process an individual request.
        /// </summary>
        /// <param name="context" />
        /// <returns />
        public override Task Invoke(IOwinContext context)
        {
            try
            {
                _logger.Info($"{context.Request.Method} {context.Request.Uri}\r\n" +
                              $"Remote: {context.Request.RemoteIpAddress}:{context.Request.RemotePort}");
            }
            catch (Exception e)
            {
                var tcs = new TaskCompletionSource<object>();
                tcs.SetException(e);
                return tcs.Task;
            }
            return Next.Invoke(context);
        }
    }
}