namespace Rib.Common.Metadata.ClientEnums
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    internal class ClientEnumPermanentStore : IClientEnumPermanentStore
    {
        [NotNull] private readonly ConcurrentDictionary<string, Type> _dict =new ConcurrentDictionary<string, Type>();
        public IReadOnlyDictionary<string, Type> Data => _dict;
        public IClientEnumPermanentStore Add(string name, Type type)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type is not enum");
            }
            _dict.TryAdd(name, type);
            return this;
        }
    }
}