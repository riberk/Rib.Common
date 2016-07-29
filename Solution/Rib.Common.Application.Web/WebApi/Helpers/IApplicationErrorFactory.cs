namespace Rib.Common.Application.Web.WebApi.Helpers
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ApplicationErrorFactory))]
    public interface IApplicationErrorFactory
    {
        [NotNull]
        IApplicationError CreateAndLog(Exception ex);
    }
}