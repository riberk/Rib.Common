namespace Rib.Common.Application.Web.WebApi.Filters
{
    using System.Web.Http.Filters;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ValidateModelFilterAttribute))]
    public interface IValidationFilter : IActionFilter
    {
        
    }
}