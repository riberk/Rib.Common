namespace Rib.Common.Application.Web.WebApi.Helpers.Http
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IActionInfo
    {
        [NotNull]
        string Name { get; }

        [NotNull]
        MethodInfo ActionMethod { get; }

        [NotNull]
        Type ControllerType { get; }

        Type ReturnType { get; }

        [NotNull]
        string ControllerName { get; }

        [NotNull]
        Task<object> ReadAsync(CancellationToken cancellationToken);
    }
}