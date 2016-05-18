namespace Rib.Common.Application.Services.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Application.Models.Rest;
    using Rib.Common.Application.Services.Rest.Infrastructure;
    using Rib.Common.Models.Contract;
    using TsSoft.EntityService;
    using TsSoft.Expressions.Helpers;
    using TsSoft.Expressions.OrderBy;

    public class RestGetService<TEntity, TTableModel> : IRestGetService<TEntity, TTableModel>
            where TEntity : class
            where TTableModel : class
    {
        [NotNull] private readonly IOrderCreator<TEntity> _orderCreator;

        public RestGetService([NotNull] IReadDatabaseService<TEntity, TTableModel> dbService,
                              [NotNull] IOrderCreator<TEntity> orderCreator)
        {
            if (dbService == null) throw new ArgumentNullException(nameof(dbService));
            if (orderCreator == null) throw new ArgumentNullException(nameof(orderCreator));
            ReadService = dbService;
            _orderCreator = orderCreator;
        }

        /// <summary>
        ///     Сервис чтения сущностей
        /// </summary>
        public IReadDatabaseService<TEntity, TTableModel> ReadService { get; }

        /// <summary>
        ///     Получить одну строку для таблицы
        /// </summary>
        /// <param name="filter">Предикат</param>
        /// <returns>Модель строки таблицы</returns>
        public async Task<TTableModel> GetSingleRowAsync(Expression<Func<TEntity, bool>> filter)
        {
            var res = await ReadService.GetSingleAsync(filter);
            if (res == null)
            {
                throw new ObjectNotFoundException("Entity not found");
            }
            return res;
        }

        /// <summary>
        ///     Получить перечисление моделей для таблицы
        /// </summary>
        /// <param name="filter">Предикат</param>
        /// <param name="order">Порядок сортировки</param>
        /// <returns>Перечисление табличных моделей</returns>
        public async Task<IReadOnlyCollection<TTableModel>> GetTableAsync(Expression<Func<TEntity, bool>> filter,
                                                                          IEnumerable<IOrderByClause<TEntity>> order = null)
        {
            return (await ReadService.GetAsync(filter, order)).ToList();
        }

        /// <summary>
        ///     Отдает страницу записей
        /// </summary>
        public async Task<IPagedResponse<TTableModel>> GetPagedAsync(IPaginator paginator,
                                                                     Expression<Func<TEntity, bool>> filter,
                                                                     IEnumerable<IOrderByClause<TEntity>> order = null)
        {
            if (paginator == null) throw new ArgumentNullException(nameof(paginator));
            var res = await ReadService.GetPagedAsync(filter, paginator.PageNumber, paginator.PageSize, order);
            return new PagedResponse<TTableModel>(res.Result, res.TotalRecords);
        }

        /// <summary>
        ///     Отдает страницу записей
        /// </summary>
        public Task<IPagedResponse<TTableModel>> GetPagedAsync(Expression<Func<TEntity, bool>> filter, IOrderedPaginationRequest request)
        {
            return GetPagedAsync(request?.Pagination ?? Paginator.Full, filter ?? PredicateBuilder.True<TEntity>(),
                                 _orderCreator.Create(request?.Order));
        }

        /// <summary>
        ///     Отдает страницу записей
        /// </summary>
        public Task<IPagedResponse<TTableModel>> GetPagedAsync(IOrderedPaginationPredicateRequest<TEntity> request)
        {
            var paginator = request?.Pagination ?? Paginator.Full;
            var filter = request?.Predicate() ?? PredicateBuilder.True<TEntity>();
            var order = _orderCreator.Create(request?.Order);
            return GetPagedAsync(paginator, filter, order);
        }
    }
}