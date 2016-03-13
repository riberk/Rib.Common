namespace Rib.Common.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Ninject.Modules;
    using global::Ninject.Syntax;
    using JetBrains.Annotations;

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

        private void BindInterfaceMetadata([NotNull] IReadOnlyCollection<Type> assemblyTypes)
        {
            var bindings = BinderHelper.ReadFromAssemblyTypes(assemblyTypes);
            foreach (var bindInfo in bindings)
            {
                var b = Bind(bindInfo.From.ToArray()).To(bindInfo.To);
                IBindingNamedWithOrOnSyntax<object> scoped;
                if (bindInfo.Scope == BindingScope.SingletonScope)
                {
                    scoped = b.InSingletonScope();
                }
                else if (bindInfo.Scope == BindingScope.ThreadScope)
                {
                    scoped = b.InThreadScope();
                }
                else if (bindInfo.Scope == BindingScope.TransientScope)
                {
                    scoped = b.InTransientScope();
                }
                else
                {
                    scoped = b.InSingletonScope();
                }
                if (!string.IsNullOrWhiteSpace(bindInfo.Name))
                {
                    scoped.Named(bindInfo.Name);
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