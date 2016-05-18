namespace Rib.Common.Application.Services.Rest
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using TsSoft.EntityRepository;

    /// <summary>
    ///     Сервис обновления сущностей
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TChangeModel">Тип модели</typeparam>
    public interface IRestUpdateService<TEntity, in TChangeModel>
            where TEntity : class
    {
        /// <summary>
        ///     Репозиторий
        /// </summary>
        [NotNull]
        IUpdateRepository<TEntity> UpdateRepository { get; }

        /// <summary>
        ///     Обновить сущность
        /// </summary>
        /// <param name="filter">Предикат</param>
        /// <param name="changeModel">Модель для обновления</param>
        [NotNull]
        Task UpdateSingleAsync(Expression<Func<TEntity, bool>> filter, TChangeModel changeModel);
    }
}