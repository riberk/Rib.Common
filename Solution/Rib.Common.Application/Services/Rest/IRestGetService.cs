namespace Rib.Common.Application.Services.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Application.Models.Rest;
    using Rib.Common.Models.Contract;
    using TsSoft.EntityService;
    using TsSoft.Expressions.OrderBy;

    public interface IRestGetService<TEntity, TTableModel>
            where TEntity : class
            where TTableModel : class
    {
        /// <summary>
        ///     Сервис чтения сущностей
        /// </summary>
        [NotNull]
        IReadDatabaseService<TEntity, TTableModel> ReadService { get; }

        /// <summary>
        ///     Получить одну строку для таблицы
        /// </summary>
        /// <param name="filter">Предикат</param>
        /// <returns>Модель строки таблицы</returns>
        [NotNull]
        Task<TTableModel> GetSingleRowAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        ///     Получить перечисление моделей для таблицы
        /// </summary>
        /// <param name="filter">Предикат</param>
        /// <param name="order">Порядок сортировки</param>
        /// <returns>Перечисление табличных моделей</returns>
        [NotNull, ItemNotNull]
        Task<IReadOnlyCollection<TTableModel>> GetTableAsync(Expression<Func<TEntity, bool>> filter,
                                                             IEnumerable<IOrderByClause<TEntity>> order = null);

        /// <summary>
        ///     Отдает страницу записей
        /// </summary>
        [NotNull]
        Task<IPagedResponse<TTableModel>> GetPagedAsync([NotNull] IPaginator paginator,
                                                        [NotNull] Expression<Func<TEntity, bool>> filter,
                                                        IEnumerable<IOrderByClause<TEntity>> order = null);

        /// <summary>
        ///     Отдает страницу записей
        /// </summary>
        [NotNull]
        Task<IPagedResponse<TTableModel>> GetPagedAsync(Expression<Func<TEntity, bool>> filter, IOrderedPaginationRequest request);

        /// <summary>
        ///     Отдает страницу записей
        /// </summary>
        [NotNull]
        Task<IPagedResponse<TTableModel>> GetPagedAsync(IOrderedPaginationPredicateRequest<TEntity> request);
    }
}