namespace Rib.Common.Application.Web.WebApi.Helpers.Html
{
    using System;
    using System.Net;
    using System.Net.Http;
    using JetBrains.Annotations;

    internal class HtmlResponseFactory : IHtmlResponseFactory
    {
        [NotNull] private readonly IHtmlContentFactory _contentFactory;

        public HtmlResponseFactory([NotNull] IHtmlContentFactory contentFactory)
        {
            if (contentFactory == null) throw new ArgumentNullException(nameof(contentFactory));
            _contentFactory = contentFactory;
        }

        public HttpResponseMessage Create(string content, HttpRequestMessage requestMessage, HttpStatusCode code = HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentNullException(nameof(content));
            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));
            var response = requestMessage.CreateResponse(code);
            if (response == null)
            {
                throw new InvalidOperationException("Не удалось получить ответ");
            }
            response.Content = _contentFactory.Create(content);
            return response;
        }
    }
}