namespace Rib.Common.Ef.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    /// <summary>Init enum tables</summary>
    [BindTo(typeof(EnumEntityInitializer))]
    public interface IEnumEntityInitializer
    {
        /// <summary>Fill enum tables</summary>
        Task FillEnumTablesAsync([NotNull] DbContext ctx, [NotNull] IEnumerable<Type> entityTypes);
    }
}