namespace Rib.Common.Application.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using TsSoft.AbstractMapper;
    using TsSoft.EntityRepository;
    using TsSoft.EntityService.UpdateAction;
    using TsSoft.Expressions.IncludeBuilder;

    /// <summary>
    ///     Сервис обновления сущностей
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TChangeModel">Тип модели</typeparam>
    internal class RestUpdateService<TEntity, TChangeModel> : IRestUpdateService<TEntity, TChangeModel>
            where TEntity : class
    {
        [NotNull] private readonly ICollection<Expression<Func<TEntity, object>>> _updateIncludes;
        [NotNull] private readonly IUpdatePathProviderMapper<TChangeModel, TEntity> _updateMapper;
        [NotNull] private readonly Func<TEntity, TEntity, IEnumerable<Func<Task>>> _updateUpdater;

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="T:RestUpdateService" />.
        /// </summary>
        public RestUpdateService([NotNull] IUpdateRepository<TEntity> updateRepository,
                                 [NotNull] IUpdatePathProviderMapper<TChangeModel, TEntity> updateMapper,
                                 [NotNull] IUpdateEntityActionFactory updateEntityActionFactory,
                                 [NotNull] IPathToDbIncludeConverter pathToDbIncludeConverter)
        {
            if (updateRepository == null) throw new ArgumentNullException(nameof(updateRepository));
            if (updateMapper == null) throw new ArgumentNullException(nameof(updateMapper));
            if (updateEntityActionFactory == null) throw new ArgumentNullException(nameof(updateEntityActionFactory));
            if (pathToDbIncludeConverter == null) throw new ArgumentNullException(nameof(pathToDbIncludeConverter));
            _updateMapper = updateMapper;
            UpdateRepository = updateRepository;
            _updateUpdater = updateEntityActionFactory.MakeAsyncUpdateAction(_updateMapper.Paths, _updateMapper.DeleteAndCreateOnPaths);
            _updateIncludes = pathToDbIncludeConverter.GetIncludes(updateMapper.Paths, updateMapper.DeleteAndCreateOnPaths);
        }

        /// <summary>
        ///     Репозиторий
        /// </summary>
        public IUpdateRepository<TEntity> UpdateRepository { get; }

        /// <summary>
        ///     Обновить сущность
        /// </summary>
        /// <param name="filter">Предикат</param>
        /// <param name="changeModel">Модель для обновления</param>
        public async Task UpdateSingleAsync(Expression<Func<TEntity, bool>> filter, TChangeModel changeModel)
        {
            var res = await UpdateRepository.UpdateSingleFromAsync(_updateUpdater, filter, _updateIncludes, _updateMapper.Map(changeModel));
            if (res == null)
            {
                throw new ObjectNotFoundException();
            }
        }
    }
}