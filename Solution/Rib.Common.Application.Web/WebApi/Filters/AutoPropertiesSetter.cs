namespace Rib.Common.Application.Web.WebApi.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using JetBrains.Annotations;
    using Rib.Common.DependencyInjection;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Helpers.Expressions;
    using Rib.Common.Helpers.Metadata;
    using Rib.Common.Models.Metadata;

    internal class AutoPropertiesSetter : ActionFilterAttribute, IAutoPropertiesSetter
    {
        [NotNull] private readonly IAttributesReader _attributesReader;
        [NotNull] private readonly ICacherFactory _cacherFactory;
        [NotNull] private readonly IPropertyHelper _propertyHelper;
        [NotNull] private readonly IResolver _resolver;

        public AutoPropertiesSetter([NotNull] IAttributesReader attributesReader,
                                    [NotNull] IResolver resolver,
                                    [NotNull] IPropertyHelper propertyHelper,
                                    [NotNull] ICacherFactory cacherFactory)
        {
            if (attributesReader == null) throw new ArgumentNullException(nameof(attributesReader));
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            if (propertyHelper == null) throw new ArgumentNullException(nameof(propertyHelper));
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _attributesReader = attributesReader;
            _resolver = resolver;
            _propertyHelper = propertyHelper;
            _cacherFactory = cacherFactory;
        }

        /// <summary>Occurs before the action method is invoked.</summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            foreach (var actionArgument in actionContext?.ActionArguments ?? new Dictionary<string, object>())
            {
                if (actionArgument.Value == null)
                {
                    return;
                }
                IReadOnlyCollection<ContextPropertyInfo> infos;
                if (!TryGetInfosFromCache(actionArgument.Value.GetType(), out infos))
                {
                    return;
                }
                foreach (var contextPropertyInfo in infos)
                {
                    contextPropertyInfo.Setter(actionArgument.Value);
                }
            }
            base.OnActionExecuting(actionContext);
        }

        private bool TryGetInfosFromCache(Type t, [NotNull] out IReadOnlyCollection<ContextPropertyInfo> infos)
        {
            var res = _cacherFactory
                    .Create<IReadOnlyCollection<ContextPropertyInfo>>(GetType().FullName).GetOrAdd(t.AssemblyQualifiedName, s =>
                    {
                        IReadOnlyCollection<ContextPropertyInfo> i;
                        return TryGetInfos(t, out i) ? i : null;
                    });
            infos = res;
            return infos != null;
        }

        private bool TryGetInfos(Type t, [NotNull] out IReadOnlyCollection<ContextPropertyInfo> infos)
        {
            var props = t.GetProperties().Select(x => new
            {
                Attr = _attributesReader.Read<ContextPropertyAttribute>(x),
                Property = x
            }).Where(x => x.Attr != null);
            var res = new List<ContextPropertyInfo>();
            foreach (var prop in props)
            {
                var contextPropertyResolver = _resolver.Get(prop.Attr.ResolverType) as IContextPropertyResolver;
                if (contextPropertyResolver == null)
                {
                    throw new InvalidCastException($"{prop.Attr.ResolverType} could ot be cast to IContextPropertyResolver");
                }
                res.Add(new ContextPropertyInfo(o =>
                {
                    var curValue = _propertyHelper.Get(prop.Property, o);
                    var resolved = contextPropertyResolver.Resolve(curValue);
                    _propertyHelper.Set(prop.Property, o, resolved);
                }));
            }
            infos = res;
            return res.Any();
        }

        private struct ContextPropertyInfo
        {
            /// <summary>
            ///     »нициализирует новый экземпл€р класса <see cref="T:System.Object" />.
            /// </summary>
            public ContextPropertyInfo([NotNull] Action<object> setter)
            {
                Setter = setter;
            }

            [NotNull]
            public Action<object> Setter { get; }
        }
    }
}