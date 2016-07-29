namespace Rib.Common.Application.Web.WebApi
{
    using System.Web.Http;
    using JetBrains.Annotations;

    public interface IWebApiConfigurationFactory
    {
        [NotNull]
        HttpConfiguration Create();
    }
}