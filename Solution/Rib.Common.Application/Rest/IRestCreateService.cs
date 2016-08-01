namespace Rib.Common.Application.Rest
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;
    using TsSoft.EntityRepository;
    using TsSoft.EntityRepository.Interfaces;

    [BindTo(typeof(RestCreateService<,,>))]
    public interface IRestCreateService<TEntity, in TCreateModel, TEntityId>
            where TEntity : class, IEntityWithId<TEntityId>
            where TEntityId : struct
            where TCreateModel : class
    {
        [NotNull]
        ICreateRepository<TEntity> CreateRepository { get; }

        /// <summary>
        ///     Создать сущность
        /// </summary>
        /// <param name="createModel">Модель для создания</param>
        /// <returns>Идентификатор созданной сущности</returns>
        [NotNull]
        Task<TEntityId> CreateAsync([NotNull] TCreateModel createModel);

        /// <summary>
        ///     Создать сущность
        /// </summary>
        /// <param name="createModel">Модель для создания</param>
        /// <param name="entityAction">Преапдейтер</param>
        /// <returns>Идентификатор созданной сущности</returns>
        [NotNull]
        Task<TEntityId> CreateAsync([NotNull] TCreateModel createModel, [NotNull] Action<TEntity> entityAction);
    }
}