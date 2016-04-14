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

        public NinjectBinder([NotNull] Func<Type[], IBindingToSyntax<object>> binder)
        {
            if (binder == null) throw new ArgumentNullException(nameof(binder));
            _binder = binder;
        }

        public void Bind(IEnumerable<IBindInfo> bindings)
        {
            if (bindings == null) throw new ArgumentNullException(nameof(bindings));
            foreach (var bindInfo in bindings)
            {
                var b = _binder(bindInfo.From.ToArray()).To(bindInfo.To);
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
                    throw new NotSupportedException($"Undefined scope {bindInfo.Scope}");
                }
                if (!string.IsNullOrWhiteSpace(bindInfo.Name))
                {
                    scoped.Named(bindInfo.Name);
                }
            }
        }
    }
}