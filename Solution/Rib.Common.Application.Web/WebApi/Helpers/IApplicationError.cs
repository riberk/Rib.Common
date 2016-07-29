namespace Rib.Common.Application.Web.WebApi.Helpers
{
    using System;
    using System.Net;
    using Rib.Common.Models.Helpers;

    public interface IApplicationError
    {
        Exception Exception { get; }

        string Message { get; }

        HttpStatusCode Code { get; }

        CorrelationId CorrelationId { get; }

        bool IncludeErrorDetails { get; }
    }
}