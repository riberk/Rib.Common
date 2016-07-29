namespace Rib.Common.Application.Web.WebApi.Helpers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(RazorResponseFactory))]
    public interface IRazorResponseFactory
    {
        [NotNull]
        HttpResponseMessage Create([NotNull] string view, [NotNull] string title, [NotNull] HttpRequestMessage requestMessage, Type modelType,
            object model, HttpStatusCode code = HttpStatusCode.OK);
    }
}