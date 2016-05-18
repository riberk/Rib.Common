namespace Rib.Common.Metadata.ClientEnums
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ClientEnumResolver))]
    public interface IClientEnumResolver
    {
        IReadOnlyCollection<IEnumModel> Find([NotNull] string name);
    }
}