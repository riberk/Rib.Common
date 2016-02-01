namespace Rib.Common.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Ninject.Modules;
    using Rib.Common.Models.Exceptions;
    using Rib.Common.Models.Metadata;

    public abstract class RibNinjectModule : NinjectModule
    {
        private void BindModules(IEnumerable<Type> assemblyTypes)
        {
            var currentType = GetType();
            var innerModules = assemblyTypes
                    .Where(x => !x.IsAbstract && !x.IsInterface && x != currentType && x.GetInterfaces().Any(i => i == typeof (INinjectModule)))
                    .Select(x => Activator.CreateInstance(x) as INinjectModule);

            Kernel.Load(innerModules);
        }

        private void BindInterfaceMetadata(IEnumerable<Type> assemblyTypes)
        {
            var unbindedInterfaces = assemblyTypes.Where(x => x.IsInterface).Select(x => new
            {
                Interface = x,
                Attrs = x.GetCustomAttributes<BindToAttribute>().ToList()
            }).Where(x => x.Attrs != null && x.Attrs.Any()).Where(x => !Kernel.GetBindings(x.Interface).Any()).ToList();

            foreach (var unbindedInterface in unbindedInterfaces)
            {
                if (unbindedInterface.Attrs.Count != 1 && unbindedInterface.Attrs.Any(a => string.IsNullOrWhiteSpace(a.Name)))
                {
                    throw new MetadataException(
                            $"Interface {unbindedInterface.Interface} contains {unbindedInterface.Attrs.Count} BindToAttribute, but not all attrs has Name property");
                }
                foreach (var bindToAttribute in unbindedInterface.Attrs)
                {
                    var binder = Bind(unbindedInterface.Interface).To(bindToAttribute.ToType).InSingletonScope();
                    if (!string.IsNullOrWhiteSpace(bindToAttribute.Name))
                    {
                        binder.Named(bindToAttribute.Name);
                    }
                }
            }
        }


        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            var assemblyTypes = GetType()
                    .Assembly
                    .GetTypes();
            BindModules(assemblyTypes);
            BindInterfaceMetadata(assemblyTypes);
        }
    }
}