namespace Rib.Common.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Binding;
    using Rib.Common.Models.Metadata;


    public class BinderHelper : IBinderHelper
    {
        public IEnumerable<IBindInfo> ReadFromTypes(IReadOnlyCollection<Type> types)
        {
            return ReadFromAssemblyTypes(types);
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<IBindInfo> ReadFromAssemblyTypes([NotNull] IReadOnlyCollection<Type> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));
            return ReadBindFrom(types).Union(ReadBindTo(types));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<IBindInfo> ReadBindFrom([NotNull] IEnumerable<Type> assemblyTypes)
        {
            return assemblyTypes.Where(x => x.IsClass && !x.IsAbstract).Select(
                x => new
                {
                    Class = x,
                    Attr = x.GetCustomAttribute<BindFromAttribute>()
                })
                .Where(x => x.Attr != null)
                .Select(
                    @class => new BindInfo(
                        new BindingScope(@class.Attr.Scope ?? BindingScope.Singleton),
                        @class.Attr.From,
                        @class.Class,
                        @class.Attr.Name));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<IBindInfo> ReadBindTo([NotNull] IEnumerable<Type> assemblyTypes, string defaultScope = BindingScope.Transient)
        {
            var unbindedInterfaces = assemblyTypes.Where(x => x.IsInterface).Select(
                x => new
                {
                    Interface = x,
                    Attrs = x.GetCustomAttributes<BindToAttribute>().ToList()
                }).Where(x => x.Attrs != null && x.Attrs.Any()).ToList();

            foreach (var unbindedInterface in unbindedInterfaces)
            {
                foreach (var bindToAttribute in unbindedInterface.Attrs)
                {
                    yield return
                        new BindInfo(
                            new BindingScope(bindToAttribute.Scope ?? defaultScope),
                            new[]
                            {
                                unbindedInterface.Interface
                            },
                            bindToAttribute.ToType,
                            bindToAttribute.Name);
                }
            }
        }
    }
}