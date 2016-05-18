namespace Rib.Common.Metadata.ClientEnums
{
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof (ClientEnumAssembliesResolver))]
    public interface IClientEnumAssembliesResolver
    {
        [NotNull]
        IReadOnlyCollection<Assembly> Resolve();
    }
}