namespace Rib.Common.Application.Web.WebApi.Helpers
{
    using System;
    using System.Web.Http.Routing;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;
    using TsSoft.ContextWrapper;

    [BindFrom(typeof(IItemFactory<UrlHelper>))]
    internal class UrlHelperItemFactory : IItemFactory<UrlHelper>
    {
        [NotNull] private readonly IHttpRequestMessageResolver _requestMessageResolver;

        public UrlHelperItemFactory([NotNull] IHttpRequestMessageResolver requestMessageResolver)
        {
            if (requestMessageResolver == null) throw new ArgumentNullException(nameof(requestMessageResolver));
            _requestMessageResolver = requestMessageResolver;
        }

        /// <summary>
        ///     Создать объект
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>
        ///     Объект
        /// </returns>
        public UrlHelper Create(string key)
        {
            return new UrlHelper(_requestMessageResolver.Current);
        }
    }
}