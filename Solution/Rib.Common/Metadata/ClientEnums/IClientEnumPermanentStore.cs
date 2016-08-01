namespace Rib.Common.Metadata.ClientEnums
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ClientEnumPermanentStore))]
    public interface IClientEnumPermanentStore
    {
        [NotNull]
        IReadOnlyDictionary<string, Type> Data { get; }

        [NotNull]
        IClientEnumPermanentStore Add([NotNull]string name, [NotNull]Type type);
    }
}