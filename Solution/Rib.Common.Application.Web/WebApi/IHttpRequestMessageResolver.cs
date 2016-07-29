namespace Rib.Common.Application.Web.WebApi
{
    using System.Net.Http;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(HttpRequestMessageResolver))]
    public interface IHttpRequestMessageResolver
    {
        [NotNull]
        HttpRequestMessage Current { get; }
    }
}