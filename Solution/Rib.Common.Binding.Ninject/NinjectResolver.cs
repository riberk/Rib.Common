namespace Rib.Common.Binding.Ninject
{
    using System;
    using global::Ninject;
    using JetBrains.Annotations;
    using Rib.Common.Ninject;

    internal class NinjectResolver : IResolver
    {
        [NotNull] private readonly IKernel _kernel;

        public NinjectResolver([NotNull] IKernel kernel)
        {
            if (kernel == null) throw new ArgumentNullException(nameof(kernel));
            _kernel = kernel;
        }

        public T Get<T>(string name = null)
        {
            return name == null ? _kernel.Get<T>() : _kernel.Get<T>(name);
        }

        public T TryGet<T>(string name = null)
        {
            return name == null ? _kernel.TryGet<T>() : _kernel.TryGet<T>(name);
        }

        public object Get(Type t, string name = null)
        {
            return name == null ? _kernel.Get(t) : _kernel.Get(t, name);
        }

        public object TryGet(Type t, string name = null)
        {
            return name == null ? _kernel.TryGet(t) : _kernel.TryGet(t, name);
        }
    }
}