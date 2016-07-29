namespace Rib.Common.Application.Wrappers
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.CorrelationId;
    using Rib.Common.Models.Helpers;
    using Rib.Common.Models.Metadata;
    using TsSoft.ContextWrapper;

    [BindFrom(typeof(IWrappersManager), typeof(IWrappersDisposer), typeof(IWrappersInitializer))]
    internal class WrappersManager : IWrappersManager
    {
        [NotNull] private readonly ICorrelationIdStore _correlationIdStore;
        [NotNull] private readonly IItemStore _itemStore;
        [NotNull] private readonly IWrappersFactory _wrappersFactory;
        [NotNull] private readonly IWrapperTypesResolver _wrapperTypesResolver;

        public WrappersManager([NotNull] IWrapperTypesResolver wrapperTypesResolver,
                               [NotNull] IWrappersFactory wrappersFactory,
                               [NotNull] ICorrelationIdStore correlationIdStore,
                               [NotNull] IItemStore itemStore)
        {
            if (wrapperTypesResolver == null) throw new ArgumentNullException(nameof(wrapperTypesResolver));
            if (wrappersFactory == null) throw new ArgumentNullException(nameof(wrappersFactory));
            if (correlationIdStore == null) throw new ArgumentNullException(nameof(correlationIdStore));
            if (itemStore == null) throw new ArgumentNullException(nameof(itemStore));
            _wrapperTypesResolver = wrapperTypesResolver;
            _wrappersFactory = wrappersFactory;
            _correlationIdStore = correlationIdStore;
            _itemStore = itemStore;
        }

        public void InitializeAll()
        {
            var wrappers = _wrapperTypesResolver.Resolve().Select(_wrappersFactory.Get);
            foreach (var wrapper in wrappers)
            {
                wrapper.Initialize();
                var correlationId = (wrapper as IItemWrapper<CorrelationId>)?.Current;
                if (correlationId != null)
                {
                    _correlationIdStore.Save(correlationId);
                }
            }
        }

        public void DisposeAll()
        {
            var wrappers = _wrapperTypesResolver.Resolve().Reverse().Select(_wrappersFactory.Get);
            foreach (var wrapper in wrappers)
            {
                wrapper.Dispose();
            }
        }

        public void ClearAll()
        {
            _itemStore.Clear();
        }
    }
}