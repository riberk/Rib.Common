namespace Rib.Common.Application.Web.WebApi.Filters
{
    using System.Web.Http.Filters;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(AutoPropertiesSetter))]
    public interface IAutoPropertiesSetter : IActionFilter
    {
        
    }
}