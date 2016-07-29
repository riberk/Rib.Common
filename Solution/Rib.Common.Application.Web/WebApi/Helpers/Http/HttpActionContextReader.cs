namespace Rib.Common.Application.Web.WebApi.Helpers.Http
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using JetBrains.Annotations;

    internal class HttpActionContextReader : IHttpActionContextReader
    {
        public IActionInfo Read(HttpActionExecutedContext actionExecutedContext)
        {
            return Read(actionExecutedContext.ActionContext);
        }

        public IActionInfo Read(HttpActionContext actionContext)
        {
            var controllerType = actionContext.ControllerContext.ControllerDescriptor.ControllerType;
            var actionName = actionContext.ActionDescriptor.ActionName;
            var actionMethod = controllerType.GetMethod(actionName, actionContext.ActionDescriptor.GetParameters().Select(p => p.ParameterType).ToArray());
            var returnType = actionContext.ActionDescriptor.ReturnType;
            var ctrlName = controllerType.Name.EndsWith("Controller")
                ? controllerType.Name.Substring(0, controllerType.Name.Length - 10)
                : controllerType.Name;
            return new ActionInfo(actionContext.Response?.Content, actionName, actionMethod, controllerType, returnType, ctrlName);
        }

        private class ActionInfo : IActionInfo
        {
            private readonly HttpContent _content;

            /// <summary>
            ///     Инициализирует новый экземпляр класса <see cref="T:System.Object" />.
            /// </summary>
            public ActionInfo(HttpContent content,
                [NotNull] string name,
                [NotNull] MethodInfo actionMethod,
                [NotNull] Type controllerType,
                Type returnType,
                [NotNull] string controllerName)
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                if (actionMethod == null) throw new ArgumentNullException(nameof(actionMethod));
                if (controllerType == null) throw new ArgumentNullException(nameof(controllerType));
                if (controllerName == null) throw new ArgumentNullException(nameof(controllerName));
                _content = content;
                Name = name;
                ActionMethod = actionMethod;
                ControllerType = controllerType;
                ReturnType = returnType;
                ControllerName = controllerName;
            }

            public string Name { get; }
            public MethodInfo ActionMethod { get; }
            public Type ControllerType { get; }
            public Type ReturnType { get; }
            public string ControllerName { get; }

            public Task<object> ReadAsync(CancellationToken cancellationToken)
            {
                return _content == null ? Task.FromResult((object) null) : _content.ReadAsAsync(ReturnType, cancellationToken);
            }
        }
    }
}