namespace Rib.Common.Binding.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Ninject.Modules;
    using JetBrains.Annotations;
    using Rib.Common.DependencyInjection;
    using Rib.Common.Models.Binding;

    public abstract class RibNinjectModule : NinjectModule
    {
        [NotNull] private readonly IBinder _binder;
        [NotNull] private readonly IBinderHelper _binderHelper;

        protected RibNinjectModule() : this(new BinderHelper())
        {
        }

        protected RibNinjectModule([NotNull] IBinderHelper binderHelper)
        {
            if (binderHelper == null) throw new ArgumentNullException(nameof(binderHelper));
            _binderHelper = binderHelper;
            _binder = new NinjectBinder(Bind);
        }

        protected RibNinjectModule([NotNull] IBinderHelper binderHelper, [NotNull] IBinder binder)
        {
            if (binderHelper == null) throw new ArgumentNullException(nameof(binderHelper));
            if (binder == null) throw new ArgumentNullException(nameof(binder));
            _binderHelper = binderHelper;
            _binder = binder;
        }

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
            var bindings = _binderHelper.ReadFromTypes(assemblyTypes, BindingScope.Singleton);
            _binder.Bind(bindings);
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