namespace Rib.Common.Application.Rest
{
    using Rib.Common.Models.Metadata;
    using TsSoft.EntityRepository.Interfaces;

    [BindTo(typeof(RestService<,,,,>))]
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