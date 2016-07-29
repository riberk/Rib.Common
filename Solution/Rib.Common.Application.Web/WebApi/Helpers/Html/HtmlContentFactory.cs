namespace Rib.Common.Application.Web.WebApi.Helpers.Html
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    internal class HtmlContentFactory : IHtmlContentFactory
    {
        public HttpContent Create(string page)
        {
            var content = new StringContent(page, Encoding.UTF8);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/html; charset=utf8");
            return content;
        }
    }
}