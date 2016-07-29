namespace Rib.Common.Application.Models.Hierarchy
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public interface IHierarchycalCacheModel<TModel, TId> : IHierarchyOrderedEntity<TModel, TId>
            where TId : struct
            where TModel : IHierarchycalCacheModel<TModel, TId>
    {
        [NotNull, ItemNotNull]
        IEnumerable<TModel> Parents { get; }
    }
}