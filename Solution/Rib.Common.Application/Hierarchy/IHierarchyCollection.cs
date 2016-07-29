namespace Rib.Common.Application.Hierarchy
{
    using System.Collections.Generic;
    using Rib.Common.Application.Models.Hierarchy;

    public interface IHierarchyCollection<TModel, in TId>
        : IReadOnlyCollection<TModel>
        where TModel : class, IHierarchycalCacheModel<TModel, TId>
        where TId : struct
    {
        TModel this[TId id] { get; }
        IEnumerable<TModel> FlatEnumerable();

        bool TryGetValue(TId id, out TModel model);
    }
}