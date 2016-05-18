namespace Rib.Common.Application.Web.WebApi.Helpers.Http
{
    using System;
    using System.Net.Http;

    public interface IHttpClientBuildResult : IDisposable
    {
        HttpClient Client { get; }
        string RelativeUri { get; }
    }
}