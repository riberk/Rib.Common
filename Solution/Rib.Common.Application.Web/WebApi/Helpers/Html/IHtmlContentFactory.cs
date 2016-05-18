namespace Rib.Common.Application.Web.WebApi.Helpers.Html
{
    using System.Net.Http;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(HtmlContentFactory))]
    public interface IHtmlContentFactory
    {
        [NotNull]
        HttpContent Create(string page);
    }
}