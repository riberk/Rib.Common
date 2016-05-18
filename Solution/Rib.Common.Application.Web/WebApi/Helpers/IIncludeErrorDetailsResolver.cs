namespace Rib.Common.Application.Web.WebApi.Helpers
{
    public interface IIncludeErrorDetailsResolver
    {
        bool? IncludeErrorDetails { get; }
    }
}