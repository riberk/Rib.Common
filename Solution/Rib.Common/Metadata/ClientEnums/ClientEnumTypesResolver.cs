namespace Rib.Common.Metadata.ClientEnums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Metadata;
    using Rib.Common.Models.Metadata;

    internal class ClientEnumTypesResolver : IClientEnumTypesResolver
    {
        [NotNull] private readonly IClientEnumAssembliesResolver _assembliesResolver;
        [NotNull] private readonly IAttributesReader _attributesReader;
        [NotNull] private readonly IClientEnumPermanentStore _clientEnumPermanentStore;

        public ClientEnumTypesResolver([NotNull] IClientEnumAssembliesResolver assembliesResolver,
                                       [NotNull] IAttributesReader attributesReader,
                                       [NotNull] IClientEnumPermanentStore clientEnumPermanentStore)
        {
            if (assembliesResolver == null) throw new ArgumentNullException(nameof(assembliesResolver));
            if (attributesReader == null) throw new ArgumentNullException(nameof(attributesReader));
            if (clientEnumPermanentStore == null) throw new ArgumentNullException(nameof(clientEnumPermanentStore));
            _assembliesResolver = assembliesResolver;
            _attributesReader = attributesReader;
            _clientEnumPermanentStore = clientEnumPermanentStore;
        }

        public IDictionary<string, Type> Resolve()
        {
            var enums = _assembliesResolver
                    .Resolve()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => x.IsEnum)
                    .Select(x => new
                    {
                        T = x,
                        A = _attributesReader.Read<ClientEnumAttribute>(x)
                    });
            return enums
                    .Where(x => x.A != null)
                    .Select(x => new KeyValuePair<string, Type>(x.A.FriendlyName, x.T))
                    .Union(_clientEnumPermanentStore.Data)
                    .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}