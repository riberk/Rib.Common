namespace Rib.Common.Application.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Expressions;
    using TsSoft.AbstractMapper;
    using TsSoft.EntityRepository;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.EntityService.UpdateAction;

    public class RestCreateService<TEntity, TCreateModel, TEntityId> : IRestCreateService<TEntity, TCreateModel, TEntityId>
            where TEntity : class, IEntityWithId<TEntityId>
            where TCreateModel : class
            where TEntityId : struct
    {
        [NotNull] private readonly IUpdatePathProviderMapper<TCreateModel, TEntity> _createMapper;
        [NotNull] private readonly Func<TEntity, TEntity, IEnumerable<Func<Task>>> _createUpdater;
        [NotNull] private readonly INewMaker _newMaker;

        public RestCreateService([NotNull] ICreateRepository<TEntity> createRepository,
                                 [NotNull] IUpdatePathProviderMapper<TCreateModel, TEntity> createMapper,
                                 [NotNull] IUpdateEntityActionFactory updateEntityActionFactory,
                                 [NotNull] INewMaker newMaker)
        {
            if (createRepository == null) throw new ArgumentNullException(nameof(createRepository));
            if (createMapper == null) throw new ArgumentNullException(nameof(createMapper));
            if (updateEntityActionFactory == null) throw new ArgumentNullException(nameof(updateEntityActionFactory));
            if (newMaker == null) throw new ArgumentNullException(nameof(newMaker));
            CreateRepository = createRepository;
            _createMapper = createMapper;
            _newMaker = newMaker;
            _createUpdater = updateEntityActionFactory.MakeAsyncUpdateAction(_createMapper.Paths, _createMapper.DeleteAndCreateOnPaths);
        }

        /// <summary>
        ///     Создать сущность
        /// </summary>
        /// <param name="createModel">Модель для создания</param>
        /// <returns>Идентификатор созданной сущности</returns>
        public Task<TEntityId> CreateAsync(TCreateModel createModel)
        {
            return CreateAsync(createModel, entity => { });
        }

        /// <summary>
        ///     Создать сущность
        /// </summary>
        /// <param name="createModel">Модель для создания</param>
        /// <param name="entityAction">Преапдейтер</param>
        /// <returns>Идентификатор созданной сущности</returns>
        public async Task<TEntityId> CreateAsync(TCreateModel createModel, Action<TEntity> entityAction)
        {
            if (createModel == null) throw new ArgumentNullException(nameof(createModel));
            if (entityAction == null) throw new ArgumentNullException(nameof(entityAction));
            var entity = _newMaker.Create<TEntity>()();
            entityAction(entity);
            var tasks = _createUpdater(_createMapper.Map(createModel), entity);
            foreach (var task in tasks ?? Enumerable.Empty<Func<Task>>())
            {
                await task();
            }
            var res = await CreateRepository.CreateAsync(entity);
            if (res == null)
            {
                throw new InvalidOperationException($"Failed create {typeof(TEntity)} entity in database");
            }
            return res.Id;
        }

        public ICreateRepository<TEntity> CreateRepository { get; }
    }
}