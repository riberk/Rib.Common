namespace Rib.Common.Application.Rest
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using TsSoft.EntityRepository;

    internal class RestDeleteService<TEntity> : IRestDeleteService<TEntity>
            where TEntity : class
    {
        public RestDeleteService([NotNull] IDeleteRepository<TEntity> deleteRepository)
        {
            if (deleteRepository == null) throw new ArgumentNullException(nameof(deleteRepository));
            DeleteRepository = deleteRepository;
        }

        public async Task DeleteSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await DeleteRepository.DeleteSingleAsync(predicate);
        }

        public IDeleteRepository<TEntity> DeleteRepository { get; }
    }
}