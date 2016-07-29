namespace Rib.Common.Application.Hierarchy
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Application.Models.Hierarchy;
    using Rib.Common.Helpers.Cache;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.EntityService;
    using TsSoft.Expressions.Helpers.Collections;

    internal class HierarchyCache<TEntity, TModel, TId> : IHierarchyCache<TEntity, TModel, TId>
        where TEntity : class, IEntityWithId<TId>, IHierarchyOrderedEntity<TEntity, TId>
        where TModel : class, IHierarchycalCacheModel<TModel, TId>
        where TId : struct
    {
        [NotNull] private readonly ICacherFactory _cacherFactory;
        [NotNull] private readonly IReadDatabaseService<TEntity, TModel> _reader;
        [NotNull] private readonly ITreeHelper _treeHelper;

        public HierarchyCache(
            [NotNull] IReadDatabaseService<TEntity, TModel> reader,
            [NotNull] ITreeHelper treeHelper,
            [NotNull] ICacherFactory cacherFactory)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (treeHelper == null) throw new ArgumentNullException(nameof(treeHelper));
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _reader = reader;
            _treeHelper = treeHelper;
            _cacherFactory = cacherFactory;
        }

        public IHierarchyCollection<TModel, TId> Data => GetCacher().GetOrAdd(CacheKey(), s => Get());

        [NotNull]
        private ICacher<IHierarchyCollection<TModel, TId>> GetCacher()
        {
            return _cacherFactory.Create<IHierarchyCollection<TModel, TId>>();
        }

        [NotNull]
        internal string CacheKey() => GetType().AssemblyQualifiedName;

        public void Reload() => GetCacher().Remove(CacheKey());

        
        [NotNull]
        private IHierarchyCollection<TModel, TId> Get()
        {
            var data = _reader.Get(e => true);
            return new HierarchyCollection<TModel, TId>(data, _treeHelper);
        }
    }
}