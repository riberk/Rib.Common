namespace Rib.Common.Application.Web.WebApi.Filters
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;
    using JetBrains.Annotations;
    using Rib.Common.Application.Web.WebApi.Helpers;
    using Rib.Common.Application.Web.WebApi.Helpers.Html;
    using Rib.Common.Application.Web.WebApi.Helpers.Http;
    using Rib.Common.Helpers.Metadata;
    using TsSoft.Expressions.Helpers.Async;

    public class HtmlActionFilterAttribute : ActionFilterAttribute, IHtmlActionFilter
    {
        [NotNull] private readonly IHttpActionContextReader _actionContextReader;
        [NotNull] private readonly IAttributesReader _attributesReader;
        [NotNull] private readonly IRazorResponseFactory _razorResponseFactory;

        public HtmlActionFilterAttribute([NotNull] IAttributesReader attributesReader,
            [NotNull] IHttpActionContextReader actionContextReader,
            [NotNull] IRazorResponseFactory razorResponseFactory)
        {
            if (attributesReader == null) throw new ArgumentNullException(nameof(attributesReader));
            if (actionContextReader == null) throw new ArgumentNullException(nameof(actionContextReader));
            if (razorResponseFactory == null) throw new ArgumentNullException(nameof(razorResponseFactory));
            _attributesReader = attributesReader;
            _actionContextReader = actionContextReader;
            _razorResponseFactory = razorResponseFactory;
        }

        /// <summary>
        ///     Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            AsyncHelper.RunSync(() => OnActionExecutedAsync(actionExecutedContext, CancellationToken.None));
        }

        /// <summary>
        ///     Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        /// <param name="cancellationToken"></param>
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {
            if (actionExecutedContext == null) throw new ArgumentNullException(nameof(actionExecutedContext));
            var ctx = _actionContextReader.Read(actionExecutedContext);
            var attr = _attributesReader.Read<HtmlAttribute>(ctx.ActionMethod);
            if (attr == null)
            {
                return;
            }
            var result = await ctx.ReadAsync(cancellationToken);
            var templateKey = GetKey(attr, ctx);
            var httpResponseMessage = _razorResponseFactory.Create(templateKey, attr.Title, actionExecutedContext.Request, ctx.ReturnType, result);
            actionExecutedContext.Response = httpResponseMessage;
        }

        [NotNull]
        private static string GetKey([NotNull] HtmlAttribute attr, [NotNull] IActionInfo actionInfo)
        {
            return !string.IsNullOrWhiteSpace(attr.Path) ? attr.Path : $"{actionInfo.ControllerName}/{actionInfo.Name}.html";
        }
    }
}