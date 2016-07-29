namespace Rib.Common.Application.Hierarchy
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Models.Interfaces;
    using Rib.Common.Models.Metadata;
    using TsSoft.EntityRepository.Interfaces;

    /// <summary>
    ///     Сортировщик
    /// </summary>
    /// <typeparam name="T">Тип сортируемой сущности</typeparam>
    /// <typeparam name="TId">Тип идентификатора сортируемой сущности</typeparam>
    [BindTo(typeof(Reorderer<,>))]
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IReorderer<T, TId>
            where T : class, IOrderedEntity, IEntityWithId<TId>
            where TId : struct, IComparable<TId>, IEquatable<TId>
    {
        /// <summary>
        ///     Передвинуть сущность с идентификатором в группе,
        ///     полученной по предикату, на место после сущности с idBefore
        ///     Если idBefore == null - то сущность окажется первой
        /// </summary>
        /// <param name="orderingEntitiesPredicate">Предикат группы</param>
        /// <param name="entityId">Идентификатор сортируемой сущности</param>
        /// <param name="idBefore">Идентификатор сущности, после которой нужно поставить сортируемую</param>
        [NotNull]
        Task ReorderAsync([NotNull] Expression<Func<T, bool>> orderingEntitiesPredicate, TId entityId, TId? idBefore);

        /// <summary>
        ///     Нормализовать порядок в группе
        /// </summary>
        /// <param name="orderingEntitiesPredicate">Предикат получения сортируемых сущностей</param>
        [NotNull]
        Task NormalizeAsync([NotNull] Expression<Func<T, bool>> orderingEntitiesPredicate);
    }
}