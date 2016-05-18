namespace Rib.Common.Application.Rest
{
    using TsSoft.EntityRepository.Interfaces;

    public interface IRestService<TEntity, in TCreateModel, in TUpdateModel, TTableModel, TId>
            : IRestGetService<TEntity, TTableModel>,
              IRestCreateService<TEntity, TCreateModel, TId>,
              IRestUpdateService<TEntity, TUpdateModel>,
              IRestDeleteService<TEntity>
            where TEntity : class, IEntityWithId<TId>
            where TCreateModel : class
            where TTableModel : class
            where TId : struct
    {
    }
}