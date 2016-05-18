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
        ///     ������� ��������
        /// </summary>
        /// <param name="createModel">������ ��� ��������</param>
        /// <returns>������������� ��������� ��������</returns>
        public Task<TId> CreateAsync(TCreateModel createModel)
        {
            return _create.CreateAsync(createModel);
        }

        /// <summary>
        ///     ������� ��������
        /// </summary>
        /// <param name="createModel">������ ��� ��������</param>
        /// <param name="entityAction">�����������</param>
        /// <returns>������������� ��������� ��������</returns>
        public Task<TId> CreateAsync(TCreateModel createModel, Action<TEntity> entityAction)
        {
            return _create.CreateAsync(createModel, entityAction);
        }

        public ICreateRepository<TEntity> CreateRepository => _create.CreateRepository;

        /// <summary>
        ///     ����������� ��������
        /// </summary>
        public IDeleteRepository<TEntity> DeleteRepository => _delete.DeleteRepository;

        /// <summary>
        ///     ������� ��������
        /// </summary>
        /// <param name="predicate">��������</param>
        public Task DeleteSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _delete.DeleteSingleAsync(predicate);
        }

        /// <summary>
        ///     ������ ������ ���������
        /// </summary>
        public IReadDatabaseService<TEntity, TTableModel> ReadService => _get.ReadService;

        /// <summary>
        ///     �������� ���� ������ ��� �������
        /// </summary>
        /// <param name="filter">��������</param>
        /// <returns>������ ������ �������</returns>
        public Task<TTableModel> GetSingleRowAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _get.GetSingleRowAsync(filter);
        }

        /// <summary>
        ///     �������� ������������ ������� ��� �������
        /// </summary>
        /// <param name="filter">��������</param>
        /// <param name="order">������� ����������</param>
        /// <returns>������������ ��������� �������</returns>
        public Task<IReadOnlyCollection<TTableModel>> GetTableAsync(Expression<Func<TEntity, bool>> filter,
            IEnumerable<IOrderByClause<TEntity>> order = null)
        {
            return _get.GetTableAsync(filter, order);
        }

        /// <summary>
        ///     ������ �������� �������
        /// </summary>
        public Task<IPagedResponse<TTableModel>> GetPagedAsync(IPaginator paginator,
            Expression<Func<TEntity, bool>> filter,
            IEnumerable<IOrderByClause<TEntity>> order = null)
        {
            return _get.GetPagedAsync(paginator, filter, order);
        }

        /// <summary>
        ///     ������ �������� �������
        /// </summary>
        public Task<IPagedResponse<TTableModel>> GetPagedAsync(Expression<Func<TEntity, bool>> filter, IOrderedPaginationRequest request)
        {
            return _get.GetPagedAsync(filter, request);
        }

        /// <summary>
        ///     ������ �������� �������
        /// </summary>
        public Task<IPagedResponse<TTableModel>> GetPagedAsync(IOrderedPaginationPredicateRequest<TEntity> request)
        {
            return _get.GetPagedAsync(request);
        }

        /// <summary>
        ///     �����������
        /// </summary>
        public IUpdateRepository<TEntity> UpdateRepository => _update.UpdateRepository;

        /// <summary>
        ///     �������� ��������
        /// </summary>
        /// <param name="filter">��������</param>
        /// <param name="changeModel">������ ��� ����������</param>
        public Task UpdateSingleAsync(Expression<Func<TEntity, bool>> filter, TUpdateModel changeModel)
        {
            return _update.UpdateSingleAsync(filter, changeModel);
        }
    }
}