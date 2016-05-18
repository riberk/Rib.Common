namespace Rib.Common.Application.Services.Rest
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using TsSoft.EntityRepository;

    internal class RestDeleteService<TEntity> : IRestDeleteService<TEntity>
        where TEntity : class
    {
        [NotNull] private readonly ILog _log;
        private readonly Type _type;

        public RestDeleteService([NotNull] IDeleteRepository<TEntity> deleteRepository, [NotNull] ILog log)
        {
            if (deleteRepository == null) throw new ArgumentNullException(nameof(deleteRepository));
            if (log == null) throw new ArgumentNullException(nameof(log));
            DeleteRepository = deleteRepository;
            _log = log;
            _type = typeof (TEntity);
        }

        public async Task DeleteSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            _log.Trace($"Deleting entity {_type}");
            await DeleteRepository.DeleteSingleAsync(predicate);
            _log.Trace($"Entity deleted");
        }

        public IDeleteRepository<TEntity> DeleteRepository { get; }
    }
}