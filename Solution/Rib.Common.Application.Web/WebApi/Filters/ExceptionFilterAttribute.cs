namespace Rib.Common.Application.Web.WebApi.Filters
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using JetBrains.Annotations;
    using Rib.Common.Application.Web.WebApi.Helpers;
    using Rib.Common.Application.Web.WebApi.Helpers.Html;
    using Rib.Common.Application.Web.WebApi.Helpers.Http;
    using Rib.Common.Helpers.Metadata;
    using Rib.Common.Models.Helpers;
    using TsSoft.Expressions.Helpers.Async;

    public class AppExceptionFilterAttribute : ExceptionFilterAttribute, IAppExceptionFilterAttribute
    {
        [NotNull] private readonly IHttpActionContextReader _actionContextReader;
        [NotNull] private readonly IApplicationErrorFactory _applicationErrorFactory;
        [NotNull] private readonly IAttributesReader _attributesReader;
        [NotNull] private readonly IRazorResponseFactory _razorResponseFactory;

        public AppExceptionFilterAttribute([NotNull] IAttributesReader attributesReader,
            [NotNull] IHttpActionContextReader actionContextReader,
            [NotNull] IApplicationErrorFactory applicationErrorFactory,
            [NotNull] IRazorResponseFactory razorResponseFactory)
        {
            if (attributesReader == null) throw new ArgumentNullException(nameof(attributesReader));
            if (actionContextReader == null) throw new ArgumentNullException(nameof(actionContextReader));
            if (applicationErrorFactory == null) throw new ArgumentNullException(nameof(applicationErrorFactory));
            if (razorResponseFactory == null) throw new ArgumentNullException(nameof(razorResponseFactory));
            _attributesReader = attributesReader;
            _actionContextReader = actionContextReader;
            _applicationErrorFactory = applicationErrorFactory;
            _razorResponseFactory = razorResponseFactory;
        }

        /// <summary>
        ///     Порождает событие исключения.
        /// </summary>
        /// <param name="actionExecutedContext">Контекст для действия.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            AsyncHelper.RunSync(() => OnExceptionAsync(actionExecutedContext, CancellationToken.None));
        }

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext == null) throw new ArgumentNullException(nameof(actionExecutedContext));
            var info = _actionContextReader.Read(actionExecutedContext);
            var attr = _attributesReader.Read<HtmlAttribute>(info.ActionMethod);
            var exception = actionExecutedContext.Exception;
            if (exception == null)
            {
                return Task.FromResult(0);
            }
            var error = _applicationErrorFactory.CreateAndLog(exception);
            if (attr == null && !(exception is HttpResponseException))
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(error.Code,
                    new HttpError(error.Exception, error.IncludeErrorDetails)
                    {
                        {"CorrelationId", error.CorrelationId.ToDatedString()},
                        {"TextMessage", error.Message }
                    });
                return Task.FromResult(0);
            }
            if(exception is HttpResponseException)
            {
                actionExecutedContext.Response = (exception as HttpResponseException).Response;
                return Task.FromResult(0);
            }
            const string errorRazorTemplate = "Error.html";
            var response = _razorResponseFactory.Create(errorRazorTemplate, "Ошибка в приложении", actionExecutedContext.Request,
                typeof (IApplicationError), error, error.Code);
            actionExecutedContext.Response = response;
            return Task.FromResult(0);
        }
    }
}