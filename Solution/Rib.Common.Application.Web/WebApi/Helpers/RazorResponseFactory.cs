namespace Rib.Common.Application.Web.WebApi.Helpers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using JetBrains.Annotations;
    using Rib.Common.Application.Web.Razor;
    using Rib.Common.Application.Web.WebApi.Helpers.Html;

    internal class RazorResponseFactory : IRazorResponseFactory
    {
        [NotNull] private readonly IHtmlResponseFactory _htmlResponseFactory;
        [NotNull] private readonly IRazorTemplateRuner _razorTemplateRuner;

        public RazorResponseFactory([NotNull] IRazorTemplateRuner razorTemplateRuner, [NotNull] IHtmlResponseFactory htmlResponseFactory)
        {
            if (razorTemplateRuner == null) throw new ArgumentNullException(nameof(razorTemplateRuner));
            if (htmlResponseFactory == null) throw new ArgumentNullException(nameof(htmlResponseFactory));
            _razorTemplateRuner = razorTemplateRuner;
            _htmlResponseFactory = htmlResponseFactory;
        }

        public HttpResponseMessage Create(string view,
                                          string title,
                                          HttpRequestMessage requestMessage,
                                          Type modelType,
                                          object model,
                                          HttpStatusCode code = HttpStatusCode.OK)
        {
            var str = _razorTemplateRuner.Run(view, title, modelType, model);
            return _htmlResponseFactory.Create(str, requestMessage, code);
        }
    }
}