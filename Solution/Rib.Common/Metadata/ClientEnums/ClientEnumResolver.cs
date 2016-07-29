namespace Rib.Common.Metadata.ClientEnums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Metadata.Enums;
    using Rib.Common.Models.Exceptions;
    using Rib.Common.Models.Metadata;

    internal class ClientEnumResolver : IClientEnumResolver
    {
        [NotNull] private readonly IEnumReader _enumReader;
        [NotNull] private readonly IClientEnumTypesResolver _typesResolver;

        public ClientEnumResolver([NotNull] IClientEnumTypesResolver typesResolver,
            [NotNull] IEnumReader enumReader)
        {
            if (typesResolver == null) throw new ArgumentNullException(nameof(typesResolver));
            if (enumReader == null) throw new ArgumentNullException(nameof(enumReader));
            _typesResolver = typesResolver;
            _enumReader = enumReader;
        }

        public IReadOnlyCollection<IEnumModel> Find(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            IReadOnlyCollection<IEnumModel> res;
            if (!AllEnumsWithValues().TryGetValue(name, out res))
            {
                throw new MetadataException($"Client enum with name {name} could not be found");
            }
            return res;
        }

        [NotNull]
        private IDictionary<string, IReadOnlyCollection<IEnumModel>> AllEnumsWithValues()
        {
            return _typesResolver.Resolve().ToDictionary(x => x.Key, x => (IReadOnlyCollection<IEnumModel>) _enumReader.ReadMany(x.Value).ToList());
        }
    }
}