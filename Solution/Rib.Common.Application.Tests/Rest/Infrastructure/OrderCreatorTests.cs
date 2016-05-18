namespace Rib.Common.Application.Rest.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Helpers.Expressions;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.Expressions.Helpers;
    using TsSoft.Expressions.Helpers.Extensions;
    using TsSoft.Expressions.OrderBy;

    [TestClass]
    public class OrderCreatorTests
    {
        private Mock<ICacherFactory> _cacherFactory;
        private MockRepository _mockFactory;
        private Mock<IPathBuilder> _pathBuilder;
        private Mock<IPathConvertRemover> _pathConvertRemover;
        private Mock<IPropertyStringResolver> _propertyStringResolver;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _pathBuilder = _mockFactory.Create<IPathBuilder>();
            _propertyStringResolver = _mockFactory.Create<IPropertyStringResolver>();
            _pathConvertRemover = _mockFactory.Create<IPathConvertRemover>();
            _cacherFactory = _mockFactory.Create<ICacherFactory>();
        }

        private OrderCreator<TestClass> Create()
        {
            return new OrderCreator<TestClass>(_pathBuilder.Object, _propertyStringResolver.Object, _pathConvertRemover.Object,
                _cacherFactory.Object);
        }

        [TestMethod]
        public void Constructor()
            =>
                new OrderCreator<TestClass>(_pathBuilder.Object, _propertyStringResolver.Object, _pathConvertRemover.Object,
                    _cacherFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1()
            => new OrderCreator<TestClass>(null, _propertyStringResolver.Object, _pathConvertRemover.Object, _cacherFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument2()
            => new OrderCreator<TestClass>(_pathBuilder.Object, null, _pathConvertRemover.Object, _cacherFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument3()
            => new OrderCreator<TestClass>(_pathBuilder.Object, _propertyStringResolver.Object, null, _cacherFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument4()
            => new OrderCreator<TestClass>(_pathBuilder.Object, _propertyStringResolver.Object, _pathConvertRemover.Object, null);

        [TestMethod]
        public void CreateWithCacheTest()
        {
            var properties = new[]
            {
                new KeyValuePair<string, bool>("arg.O", false)
            };
            var key = $"{properties[0].Key}|{properties[0].Value}";
            var cache = _mockFactory.Create<ICacher<IOrderByClause<TestClass>>>();
            var orderByClause = OrderByClause<TestClass>.Create(c => c.Id);
            _cacherFactory.Setup(x => x.Create<IOrderByClause<TestClass>>(typeof (OrderCreator<TestClass>).FullName, null))
                .Returns(cache.Object)
                .Verifiable();
            cache.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, IOrderByClause<TestClass>>>())).Returns(orderByClause).Verifiable();

            var res = Create().Create(properties).ToList();

            Assert.AreEqual(orderByClause, res[0]);
        }

        [TestMethod]
        public void CreateWithoutCacheTest()
        {
            var properties = new[]
            {
                new KeyValuePair<string, bool>("arg.O", false)
            };
            var key = $"{properties[0].Key}|{properties[0].Value}";
            var cache = _mockFactory.Create<ICacher<IOrderByClause<TestClass>>>();
            var propertyInfo = typeof (TestClass).GetProperty("Id");
            var propertyInfos = propertyInfo.ToEnumerable();
            Expression<Func<TestClass, int>> path = c => ((IEntityWithId<int>) c).Id;
            Expression<Func<TestClass, int>> pathAfterConvert = c => c.Id;

            _cacherFactory.Setup(x => x.Create<IOrderByClause<TestClass>>(typeof (OrderCreator<TestClass>).FullName, null))
                .Returns(cache.Object)
                .Verifiable();
            cache.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, IOrderByClause<TestClass>>>()))
                .Returns((string s, Func<string, IOrderByClause<TestClass>> f) => f(s))
                .Verifiable();
            _propertyStringResolver.Setup(x => x.GetProperties(typeof (TestClass), properties[0].Key)).Returns(propertyInfos).Verifiable();
            _pathBuilder.Setup(x => x.Build(propertyInfos)).Returns(path).Verifiable();
            _pathConvertRemover.Setup(x => x.RemoveLast(path)).Returns(pathAfterConvert).Verifiable();

            var res = Create().Create(properties).ToList();

            Assert.AreEqual(1, res.Count);
            var clause = res[0] as OrderByClause<TestClass, int>;
            Assert.AreEqual(properties[0].Value, clause.Descending);
            Assert.AreEqual(pathAfterConvert, clause.KeySelector);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        public class TestClass : IEntityWithId<int>
        {
            /// <summary>
            ///     Ключ экземпляра
            /// </summary>
            public int Id { get; set; }
        }
    }
}