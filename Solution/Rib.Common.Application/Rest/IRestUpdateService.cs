namespace Rib.Common.Application.Rest
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;
    using TsSoft.EntityRepository;

    /// <summary>
    ///     Сервис обновления сущностей
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TChangeModel">Тип модели</typeparam>
    [BindTo(typeof(RestUpdateService<,>))]
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