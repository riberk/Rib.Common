namespace Rib.Common.Application.Web.WebApi.Helpers.Http
{
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(HttpActionContextReader))]
    public interface IHttpActionContextReader
    {
        [NotNull]
        IActionInfo Read([NotNull] HttpActionExecutedContext actionExecutedContext);

        [NotNull]
        IActionInfo Read([NotNull] HttpActionContext actionContext);
    }
}