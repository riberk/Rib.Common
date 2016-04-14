namespace Rib.Common.DependencyInjection
{
    using System;
    using System.Reflection;
    using JetBrains.Annotations;

    public static class BindingEngine
    {
        public static IBinderHelper BinderHelper { get; set; } = new BinderHelper();

        public static IBinder Binder { get; set; }

        [NotNull]
        private static readonly object Lock = new object();

        public static void Bind([NotNull] Assembly assembly)
        {
            lock (Lock)
            {
                EnsureNotNull();
                var bindings = BinderHelper.ReadFromTypes(assembly.GetTypes());
                Binder.Bind(bindings);
            }
            
        }
        private static void EnsureNotNull()
        {
            if (Binder == null)
            {
                throw new InvalidOperationException("Set Binder property to IBinder instance");
            }
            if (BinderHelper == null)
            {
                throw new InvalidOperationException("Set BinderHelper property to IBinderHelper instance");
            }
        }
    }
}