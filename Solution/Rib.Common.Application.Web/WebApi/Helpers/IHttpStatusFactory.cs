namespace Rib.Common.Application.Web.WebApi.Helpers
{
    using System;
    using System.Net;

    public interface IHttpStatusFactory
    {
        HttpStatusCode Create(Exception ex);
    }
}