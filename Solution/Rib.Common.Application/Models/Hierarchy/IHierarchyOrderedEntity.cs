namespace Rib.Common.Application.Models.Hierarchy
{
    using Rib.Common.Models.Interfaces;
    using TsSoft.EntityRepository.Interfaces;

    /// <summary>
    ///     Сущность, поддерживающая иерархию и сортировку
    /// </summary>
    /// <typeparam name="TEntity">Тип сушности</typeparam>
    /// <typeparam name="TId">Тип идентификатора сущности</typeparam>
    public interface IHierarchyOrderedEntity<TEntity, TId> : ITreeElement<TEntity, TId>, IOrderedEntity where TId : struct
    {
        /// <summary>
        /// Идентификатор родителя
        /// </summary>
        new TId? ParentId { get; set; }
    }
}