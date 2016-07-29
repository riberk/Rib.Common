namespace Rib.Common.Application.Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Application.Models.Hierarchy;
    using Rib.Common.Helpers.Cache;
    using TsSoft.EntityService;
    using TsSoft.Expressions.Helpers.Collections;

    [TestClass]
    public class HierarchyCacheTests
    {
        private Mock<ICacherFactory> _cacherFactory;
        private HierarchyCache<E, Model, int> _hierarchyCache;
        private MockRepository _mockFactory;
        private Mock<IReadDatabaseService<E, Model>> _read;
        private Mock<ITreeHelper> _treeHelper;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _read = _mockFactory.Create<IReadDatabaseService<E, Model>>();
            _treeHelper = _mockFactory.Create<ITreeHelper>(MockBehavior.Loose);
            _cacherFactory = _mockFactory.Create<ICacherFactory>();

            _hierarchyCache = new HierarchyCache<E, Model, int>(_read.Object, _treeHelper.Object, _cacherFactory.Object);
        }

        [TestMethod]
        public void ReloadTest()
        {
            var cacher = _mockFactory.Create<ICacher<IHierarchyCollection<Model, int>>>();
            var cacheKey = _hierarchyCache.CacheKey();
            _cacherFactory.Setup(x => x.Create<IHierarchyCollection<Model, int>>(null, null)).Returns(cacher.Object).Verifiable();
            cacher.Setup(x => x.Remove(cacheKey)).Verifiable();
            _hierarchyCache.Reload();
        }

        [TestMethod]
        public void DataTest()
        {
            var cacher = _mockFactory.Create<ICacher<IHierarchyCollection<Model, int>>>();
            var cacheKey = _hierarchyCache.CacheKey();
            _cacherFactory.Setup(x => x.Create<IHierarchyCollection<Model, int>>(null, null)).Returns(cacher.Object).Verifiable();
            cacher.Setup(x => x.GetOrAdd(cacheKey, It.IsAny<Func<string, IHierarchyCollection<Model, int>>>()))
                .Returns((string s, Func<string, IHierarchyCollection<Model, int>> f) => f(s))
                .Verifiable();
            _read.Setup(x => x.Get(It.IsAny<Expression<Func<E, bool>>>(), null)).Returns(Enumerable.Empty<Model>()).Verifiable();
            var data = _hierarchyCache.Data;
            Assert.AreEqual(0, data.Count);
        }

        [TestMethod]
        public void CacheKey()
        {
            var cacheKey = _hierarchyCache.CacheKey();
            Assert.AreEqual(cacheKey, _hierarchyCache.GetType().AssemblyQualifiedName);
        }

        public class E : IHierarchyOrderedEntity<E, int>
        {
            /// <summary>Ключ экземпляра</summary>
            public int Id { get; set; }

            /// <summary>Дочерние сущности</summary>
            public ICollection<E> Children { get; set; }

            /// <summary>Родительская сущность</summary>
            public E Parent { get; set; }

            /// <summary>
            ///     Порядок
            /// </summary>
            public int Order { get; set; }

            /// <summary>
            ///     Идентификатор родителя
            /// </summary>
            public int? ParentId { get; set; }
        }

        public class Model : IHierarchycalCacheModel<Model, int>
        {
            /// <summary>Ключ экземпляра</summary>
            public int Id { get; set; }

            /// <summary>Дочерние сущности</summary>
            public ICollection<Model> Children { get; set; }

            /// <summary>Родительская сущность</summary>
            public Model Parent { get; set; }


            /// <summary>
            ///     Порядок
            /// </summary>
            public int Order { get; set; }

            /// <summary>
            ///     Идентификатор родителя
            /// </summary>
            public int? ParentId { get; set; }

            public IEnumerable<Model> Parents { get; }
        }
    }
}