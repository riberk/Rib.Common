namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Threading.Tasks;
    using global::Common.Logging;
    using JetBrains.Annotations;
    using Microsoft.Owin;

    public class ExceptionHandlerMiddleware : OwinMiddleware
    {
        [NotNull] private readonly ILog _logger;

        public ExceptionHandlerMiddleware([NotNull] OwinMiddleware next, [NotNull] ILog logger) : base(next)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        /// <summary>Process an individual request.</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Invoke(IOwinContext context)
        {
            var task = Next.Invoke(context);
            if (task == null)
            {
                return;
            }
            try
            {
                await task;
            }
            catch (Exception e)
            {
                _logger.Fatal("Непредвиденная ошибка в цепочке", e);
                throw;
            }
        }
    }
}