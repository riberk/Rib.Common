namespace Rib.Common.Binding.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Ninject.Syntax;
    using JetBrains.Annotations;
    using Rib.Common.DependencyInjection;
    using Rib.Common.Models.Binding;

    public class NinjectBinder : IBinder
    {
        [NotNull] private readonly Func<Type[], IBindingToSyntax<object>> _binder;
        private readonly IScopeBinder _scopeBinder;

        public NinjectBinder([NotNull] Func<Type[], IBindingToSyntax<object>> binder, IScopeBinder scopeBinder = null)
        {
            if (binder == null) throw new ArgumentNullException(nameof(binder));
            _binder = binder;
            _scopeBinder = scopeBinder ?? new DefaultScopeBinder();
        }

        public void Bind(IEnumerable<IBindInfo> bindings)
        {
            if (bindings == null) throw new ArgumentNullException(nameof(bindings));
            foreach (var bindInfo in bindings)
            {
                var b = _binder(bindInfo.From.ToArray()).To(bindInfo.To);
                var scoped = _scopeBinder.InScope(b, bindInfo.Scope);
                if (!string.IsNullOrWhiteSpace(bindInfo.Name))
                {
                    scoped.Named(bindInfo.Name);
                }
            }
        }

        public class DefaultScopeBinder : IScopeBinder
        {
            public IBindingNamedWithOrOnSyntax<object> InScope(IBindingWhenInNamedWithOrOnSyntax<object> binded, BindingScope scope)
            {
                if (binded == null) throw new ArgumentNullException(nameof(binded));
                if (scope == null) throw new ArgumentNullException(nameof(scope));
                IBindingNamedWithOrOnSyntax<object> scoped;
                if (scope == BindingScope.SingletonScope)
                {
                    scoped = binded.InSingletonScope();
                }
                else if (scope == BindingScope.ThreadScope)
                {
                    scoped = binded.InThreadScope();
                }
                else if (scope == BindingScope.TransientScope)
                {
                    scoped = binded.InTransientScope();
                }
                else
                {
                    throw new NotSupportedException($"Undefined scope {scope}");
                }
                return scoped;
            }
        }
    }
}