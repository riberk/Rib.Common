namespace Rib.Common.Application.Web.WebApi.Helpers.Html
{
    using System.Net;
    using System.Net.Http;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(HtmlResponseFactory))]
    public interface IHtmlResponseFactory
    {
        [NotNull]
        HttpResponseMessage Create([NotNull] string content, [NotNull] HttpRequestMessage requestMessage, HttpStatusCode code = HttpStatusCode.OK);
    }
}