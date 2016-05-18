namespace Rib.Common.Application.Web.WebApi.Helpers.Html
{
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    /// <summary>
    ///     Резолвит методы контроллеров, возвращающие Html
    /// </summary>
    [BindTo(typeof(HtmlActionResolver))]
    public interface IHtmlActionResolver
    {
        /// <summary>
        ///     Получить все методы web api, возвращающие html
        /// </summary>
        /// <param name="assembly">Сборка, из которой надо их получить</param>
        [NotNull, ItemNotNull]
        IReadOnlyCollection<MethodInfo> Resolve([NotNull] Assembly assembly);
    }
}