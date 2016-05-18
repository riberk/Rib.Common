namespace Rib.Common.Application.Hierarchy
{
    using System.Collections.Generic;
    using Rib.Common.Application.Models.Hierarchy;

    public interface IHierarchyCollection<out TModel, in TId>
        : IReadOnlyCollection<TModel>
        where TModel : class, IHierarchycalCacheModel<TModel, TId>
        where TId : struct
    {
        TModel Leaf(TId id);
        IEnumerable<TModel> FlatEnumerable();
    }
}