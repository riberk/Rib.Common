namespace Rib.Common.Application.Services.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Application.Models.Rest;
    using Rib.Common.Models.Contract;
    using TsSoft.EntityRepository;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.EntityService;
    using TsSoft.Expressions.OrderBy;

    internal class RestService<TEntity, TCreateModel, TUpdateModel, TTableModel, TId> :
        IRestService<TEntity, TCreateModel, TUpdateModel, TTableModel, TId>
        where TEntity : class, IEntityWithId<TId> where TCreateModel : class where TTableModel : class where TId : struct
    {
        [NotNull] private readonly IRestCreateService<TEntity, TCreateModel, TId> _create;
        [NotNull] private readonly IRestDeleteService<TEntity> _delete;
        [NotNull] private readonly IRestGetService<TEntity, TTableModel> _get;
        [NotNull] private readonly IRestUpdateService<TEntity, TUpdateModel> _update;

        public RestService([NotNull] IRestGetService<TEntity, TTableModel> get,
            [NotNull] IRestCreateService<TEntity, TCreateModel, TId> create,
            [NotNull] IRestUpdateService<TEntity, TUpdateModel> update,
            [NotNull] IRestDeleteService<TEntity> delete)
        {
            if (get == null) throw new ArgumentNullException(nameof(get));
            if (create == null) throw new ArgumentNullException(nameof(create));
            if (update == null) throw new ArgumentNullException(nameof(update));
            if (delete == null) throw new ArgumentNullException(nameof(delete));
            _get = get;
            _create = create;
            _update = update;
            _delete = delete;
        }

        /// <summary>
        ///     Создать сущность
        /// </summary>
        /// <param name="createModel">Модель для создания</param>
        /// <returns>Идентификатор созданной сущности</returns>
        public Task<TId> CreateAsync(TCreateModel createModel)
        {
            return _create.CreateAsync(createModel);
        }

        /// <summary>
        ///     Создать сущность
        /// </summary>
        /// <param name="createModel">Модель для создания</param>
        /// <param name="entityAction">Преапдейтер</param>
        /// <returns>Идентификатор созданной сущности</returns>
        public Task<TId> CreateAsync(TCreateModel createModel, Action<TEntity> entityAction)
        {
            return _create.CreateAsync(createModel, entityAction);
        }

        public ICreateRepository<TEntity> CreateRepository => _create.CreateRepository;

        /// <summary>
        ///     Репозиторий удаления
        /// </summary>
        public IDeleteRepository<TEntity> DeleteRepository => _delete.DeleteRepository;

        /// <summary>
        ///     Удалить сущность
        /// </summary>
        /// <param name="predicate">Предикат</param>
        public Task DeleteSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _delete.DeleteSingleAsync(predicate);
        }

        /// <summary>
        ///     Сервис чтения сущностей
        /// </summary>
        public IReadDatabaseService<TEntity, TTableModel> ReadService => _get.ReadService;

        /// <summary>
        ///     Получить одну строку для таблицы
        /// </summary>
        /// <param name="filter">Предикат</param>
        /// <returns>Модель строки таблицы</returns>
        public Task<TTableModel> GetSingleRowAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _get.GetSingleRowAsync(filter);
        }

        /// <summary>
        ///     Получить перечисление моделей для таблицы
        /// </summary>
        /// <param name="filter">Предикат</param>
        /// <param name="order">Порядок сортировки</param>
        /// <returns>Перечисление табличных моделей</returns>
        public Task<IReadOnlyCollection<TTableModel>> GetTableAsync(Expression<Func<TEntity, bool>> filter,
            IEnumerable<IOrderByClause<TEntity>> order = null)
        {
            return _get.GetTableAsync(filter, order);
        }

        /// <summary>
        ///     Отдает страницу записей
        /// </summary>
        public Task<IPagedResponse<TTableModel>> GetPagedAsync(IPaginator paginator,
            Expression<Func<TEntity, bool>> filter,
            IEnumerable<IOrderByClause<TEntity>> order = null)
        {
            return _get.GetPagedAsync(paginator, filter, order);
        }

        /// <summary>
        ///     Отдает страницу записей
        /// </summary>
        public Task<IPagedResponse<TTableModel>> GetPagedAsync(Expression<Func<TEntity, bool>> filter, IOrderedPaginationRequest request)
        {
            return _get.GetPagedAsync(filter, request);
        }

        /// <summary>
        ///     Отдает страницу записей
        /// </summary>
        public Task<IPagedResponse<TTableModel>> GetPagedAsync(IOrderedPaginationPredicateRequest<TEntity> request)
        {
            return _get.GetPagedAsync(request);
        }

        /// <summary>
        ///     Репозиторий
        /// </summary>
        public IUpdateRepository<TEntity> UpdateRepository => _update.UpdateRepository;

        /// <summary>
        ///     Обновить сущность
        /// </summary>
        /// <param name="filter">Предикат</param>
        /// <param name="changeModel">Модель для обновления</param>
        public Task UpdateSingleAsync(Expression<Func<TEntity, bool>> filter, TUpdateModel changeModel)
        {
            return _update.UpdateSingleAsync(filter, changeModel);
        }
    }
}