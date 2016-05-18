namespace Rib.Common.Application.Hierarchy
{
    using JetBrains.Annotations;
    using Rib.Common.Application.Models.Hierarchy;
    using Rib.Common.Models.Metadata;
    using TsSoft.EntityRepository.Interfaces;

    [BindTo(typeof(HierarchyCache<,,>))]
    public interface IHierarchyCache<TEntity, out TModel, in TId>
            where TEntity : class , IEntityWithId<TId>, IHierarchyOrderedEntity<TEntity, TId>
            where TModel : class, IHierarchycalCacheModel<TModel, TId> 
            where TId : struct
    {
        [NotNull]
        IHierarchyCollection<TModel, TId> Data { get; }

        void Reload();
    }
}