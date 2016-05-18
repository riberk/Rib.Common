namespace Rib.Common.Application.Wrappers
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.DependencyInjection;
    using TsSoft.ContextWrapper;

    internal class WrappersFactory : IWrappersFactory
    {
        [NotNull] private readonly IResolver _resolver;

        public WrappersFactory([NotNull] IResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            _resolver = resolver;
        }

        public IItemWrapper Get(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var wrapper = _resolver.TryGet(type);
            if (wrapper == null)
            {
                throw new InvalidOperationException($"Не удалось активировать тип {type.FullName}");
            }
            var typedWrapper = wrapper as IItemWrapper;
            if (typedWrapper == null)
            {
                throw new InvalidCastException($"Тип {type.FullName} должен реализовывать IItemWrapper");
            }
            return typedWrapper;
        }
    }
}