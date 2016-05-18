namespace Rib.Common.Application.Web.WebApi.Helpers.Html
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Helpers.Metadata;

    internal class HtmlActionResolver : IHtmlActionResolver
    {
        [NotNull] private readonly IAttributesReader _attributesReader;
        [NotNull] private readonly ICacherFactory _cacherFactory;

        public HtmlActionResolver([NotNull] IAttributesReader attributesReader,
                                  [NotNull] ICacherFactory cacherFactory)
        {
            if (attributesReader == null) throw new ArgumentNullException(nameof(attributesReader));
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _attributesReader = attributesReader;
            _cacherFactory = cacherFactory;
        }

        public IReadOnlyCollection<MethodInfo> Resolve(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            return _cacherFactory
                    .Create<IReadOnlyCollection<MethodInfo>>(GetType().FullName)
                    .GetOrAdd("HtmlActions", s => Read(assembly));
        }

        [NotNull, ItemNotNull]
        private IReadOnlyCollection<MethodInfo> Read([NotNull] Assembly assembly)
        {
            var allMethods = assembly.GetTypes()
                                     .Where(x => typeof (ApiController).IsAssignableFrom(x))
                                     .SelectMany(x => x.GetMethods())
                                     .Where(x => _attributesReader.Read<HtmlAttribute>(x) != null)
                                     .ToList();
            return allMethods;
        }
    }
}