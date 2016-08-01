namespace Rib.Common.Application.Rest
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;
    using TsSoft.EntityRepository;

    /// <summary>
    ///     Сервис удаления сущностей
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    [BindTo(typeof(RestDeleteService<>))]
    public interface IRestDeleteService<TEntity>
            where TEntity : class
    {
        /// <summary>
        ///     Репозиторий удаления
        /// </summary>
        [NotNull]
        IDeleteRepository<TEntity> DeleteRepository { get; }

        /// <summary>
        ///     Удалить сущность
        /// </summary>
        /// <param name="predicate">Предикат</param>
        [NotNull]
        Task DeleteSingleAsync(Expression<Func<TEntity, bool>> predicate);
    }
}