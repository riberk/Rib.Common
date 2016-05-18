namespace Rib.Common.Application.Web.WebApi.Filters
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using JetBrains.Annotations;

    public class ValidateModelFilterAttribute : ActionFilterAttribute, IValidationFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            CreateResponseIfInvalidModel(actionContext);
        }

        private static void CreateResponseIfInvalidModel(HttpActionContext actionContext)
        {
            var notNullParameterNames = GetNotNullParameterNames(actionContext);
            foreach (var notNullParameterName in notNullParameterNames)
            {
                object value;
                if (!actionContext.ActionArguments.TryGetValue(notNullParameterName, out value) || value == null)
                {
                    actionContext.ModelState.AddModelError(notNullParameterName, $"Не найден параметр \"{notNullParameterName}\"");
                }
            }
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    actionContext.ModelState);
            }
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            CreateResponseIfInvalidModel(actionContext);
            return Task.FromResult(true);
        }

        [NotNull]
        private static IEnumerable<string> GetNotNullParameterNames(HttpActionContext actionContext)
        {
            return actionContext
                ?.ActionDescriptor
                ?.GetParameters()
                ?.Where(x => x?.GetCustomAttributes<RequiredAttribute>()?.Any() ?? false)
                .Select(p => p.ParameterName)
                .ToList() ?? Enumerable.Empty<string>();
        }
    }
}