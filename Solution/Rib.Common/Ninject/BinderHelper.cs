namespace Rib.Common.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Exceptions;
    using Rib.Common.Models.Metadata;

    public static class BinderHelper
    {
        [NotNull,ItemNotNull]
        public static IEnumerable<IBindInfo> ReadFromAssemblyTypes([NotNull] IReadOnlyCollection<Type> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));
            return ReadBindFrom(types).Union(ReadBindTo(types));
        }

        [NotNull,ItemNotNull]
        private static IEnumerable<IBindInfo> ReadBindFrom([NotNull] IEnumerable<Type> assemblyTypes)
        {
            return assemblyTypes.Where(x => x.IsClass && !x.IsAbstract).Select(
                x => new
                {
                    Class = x,
                    Attr = x.GetCustomAttribute<BindFromAttribute>()
                })
                .Where(x => x.Attr != null)
                .Select(@class => new BindInfo(
                    new BindingScope(@class.Attr.Scope ?? BindingScope.Singleton),
                    @class.Attr.From,
                    @class.Class,
                    @class.Attr.Name));
        }

        [NotNull,ItemNotNull]
        private static IEnumerable<IBindInfo> ReadBindTo([NotNull] IEnumerable<Type> assemblyTypes)
        {
            var unbindedInterfaces = assemblyTypes.Where(x => x.IsInterface).Select(x => new
            {
                Interface = x,
                Attrs = x.GetCustomAttributes<BindToAttribute>().ToList()
            }).Where(x => x.Attrs != null && x.Attrs.Any()).ToList();

            foreach (var unbindedInterface in unbindedInterfaces)
            {
                if (unbindedInterface.Attrs.Count != 1 && unbindedInterface.Attrs.Any(a => string.IsNullOrWhiteSpace(a.Name)))
                {
                    throw new MetadataException(
                        $"Interface {unbindedInterface.Interface} contains {unbindedInterface.Attrs.Count} BindToAttribute, but not all attrs has Name property");
                }
                foreach (var bindToAttribute in unbindedInterface.Attrs)
                {
                    yield return
                        new BindInfo(
                            new BindingScope(bindToAttribute.Scope ?? BindingScope.Singleton),
                            new[] {unbindedInterface.Interface},
                            bindToAttribute.ToType,
                            bindToAttribute.Name);
                }
            }
        }
    }
}