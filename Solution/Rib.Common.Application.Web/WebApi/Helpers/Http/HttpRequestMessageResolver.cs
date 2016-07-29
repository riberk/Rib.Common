namespace Rib.Common.Application.Web.WebApi.Helpers.Http
{
    using System;
    using System.Net.Http;
    using JetBrains.Annotations;
    using Rib.Common.Application.Web.Owin;

    public class HttpRequestMessageResolver : IHttpRequestMessageResolver
    {
        [NotNull] private readonly IOwinContextResolver _owinContextResolver;
        [NotNull] private readonly IWebApiConfigurationFactory _webApiConfigurationFactory;

        public HttpRequestMessageResolver([NotNull] IOwinContextResolver owinContextResolver,
                                             [NotNull] IWebApiConfigurationFactory webApiConfigurationFactory)
        {
            if (owinContextResolver == null) throw new ArgumentNullException(nameof(owinContextResolver));
            if (webApiConfigurationFactory == null) throw new ArgumentNullException(nameof(webApiConfigurationFactory));
            _owinContextResolver = owinContextResolver;
            _webApiConfigurationFactory = webApiConfigurationFactory;
        }

        public HttpRequestMessage Current
        {
            get
            {
                var request = _owinContextResolver.Current?.Request;

                var res = request != null
                                  ? new HttpRequestMessage(new HttpMethod(request.Method), request.Uri)
                                  : new HttpRequestMessage(new HttpMethod("GET"), "http://localhost:18224/");
                res.SetConfiguration(_webApiConfigurationFactory.Create());
                return res;
            }
        }
    }
}