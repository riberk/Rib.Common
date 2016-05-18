namespace Rib.Common.Metadata.ClientEnums
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ClientEnumTypesResolver))]
    public interface IClientEnumTypesResolver
    {
        [NotNull]
        IDictionary<string, Type> Resolve();
    }
}