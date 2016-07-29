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

        public ClientEnumTypesResolver([NotNull] IClientEnumAssembliesResolver assembliesResolver, [NotNull] IAttributesReader attributesReader)
        {
            if (assembliesResolver == null) throw new ArgumentNullException(nameof(assembliesResolver));
            if (attributesReader == null) throw new ArgumentNullException(nameof(attributesReader));
            _assembliesResolver = assembliesResolver;
            _attributesReader = attributesReader;
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
                    .ToDictionary(x => x.A.FriendlyName, x => x.T);
        }
    }
}