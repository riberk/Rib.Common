namespace Rib.Common.Application.Web.WebApi.Helpers
{
    using System;
    using System.Data.Entity.Validation;
    using System.Net;
    using System.Reflection;
    using global::Common.Logging;
    using JetBrains.Annotations;
    using Rib.Common.Models.Helpers;
    using TsSoft.ContextWrapper;

    internal class ApplicationErrorFactory : IApplicationErrorFactory
    {
        [NotNull] private readonly IItemWrapper<CorrelationId> _correlationIdWrapper;
        [NotNull] private readonly IHttpStatusFactory _httpStatusFactory;
        [NotNull] private readonly IIncludeErrorDetailsResolver _includeErrorDetailsResolver;
        [NotNull] private readonly ILog _logger;

        public ApplicationErrorFactory([NotNull] IItemWrapper<CorrelationId> correlationIdWrapper,
                                       [NotNull] IIncludeErrorDetailsResolver includeErrorDetailsResolver,
                                       [NotNull] IHttpStatusFactory httpStatusFactory,
                                       [NotNull] ILog logger)
        {
            if (correlationIdWrapper == null) throw new ArgumentNullException(nameof(correlationIdWrapper));
            if (includeErrorDetailsResolver == null) throw new ArgumentNullException(nameof(includeErrorDetailsResolver));
            if (httpStatusFactory == null) throw new ArgumentNullException(nameof(httpStatusFactory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _correlationIdWrapper = correlationIdWrapper;
            _includeErrorDetailsResolver = includeErrorDetailsResolver;
            _httpStatusFactory = httpStatusFactory;
            _logger = logger;
        }

        public IApplicationError CreateAndLog(Exception ex)
        {
            return OnException(ex, _correlationIdWrapper.Current);
        }

        private ApplicationError OnException(Exception ex, CorrelationId correlationId)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));
            var unwrapped = UnwrapTargetInvocation(ex);
            var reflectionTypeLoadException = unwrapped as ReflectionTypeLoadException;
            if (reflectionTypeLoadException?.LoaderExceptions != null)
            {
                foreach (var loadException in reflectionTypeLoadException.LoaderExceptions)
                {
                    _logger.Error("Could not load types", loadException);
                }
            }
            var dbEntityValidationException = unwrapped as DbEntityValidationException;
            _logger.Error("Request throws exception", unwrapped);
            if (dbEntityValidationException?.EntityValidationErrors != null)
            {
                foreach (var dbEntityValidationResult in dbEntityValidationException.EntityValidationErrors)
                {
                    foreach (var dbValidationError in dbEntityValidationResult.ValidationErrors)
                    {
                        _logger.Error(
                                      $"Entity {dbEntityValidationResult.Entry.Entity.GetType()} error property {dbValidationError.PropertyName}. Error message {dbValidationError.ErrorMessage}");
                    }
                }
            }
            var res = new ApplicationError
            {
                Exception = unwrapped,
                CorrelationId = correlationId,
                Code = GetStatusCodeByException(unwrapped),
                Message = unwrapped.Message,
                IncludeErrorDetails = _includeErrorDetailsResolver.IncludeErrorDetails ?? false
            };
            return res;
        }

        [NotNull]
        private static Exception UnwrapTargetInvocation([NotNull] Exception exception)
        {
            while (true)
            {
                var targetInvocation = exception as TargetInvocationException;
                if (targetInvocation?.InnerException == null)
                {
                    var aggregateException = exception as AggregateException;
                    if (aggregateException?.InnerException == null || aggregateException.InnerExceptions == null ||
                        aggregateException.InnerExceptions.Count > 1)
                    {
                        return exception;
                    }
                    exception = aggregateException.InnerException;
                }
                else
                {
                    exception = targetInvocation.InnerException;
                }
            }
        }

        private HttpStatusCode GetStatusCodeByException(Exception ex)
        {
            return _httpStatusFactory.Create(ex);
        }

        internal class ApplicationError : IApplicationError
        {
            public Exception Exception { get; set; }

            public string Message { get; set; }

            public HttpStatusCode Code { get; set; }

            public CorrelationId CorrelationId { get; set; }

            public bool IncludeErrorDetails { get; set; }
        }
    }
}