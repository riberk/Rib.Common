namespace Rib.Common.Application.Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Models.Interfaces;
    using TsSoft.EntityRepository;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.Expressions.OrderBy;
    using TsSoft.Expressions.SelectBuilder.Models;
    using Updater = System.Func
            <System.Action<ReordererTests.Entity>, System.Linq.Expressions.Expression<System.Func<ReordererTests.Entity, bool>>, System.Collections.Generic.IEnumerable
                    <System.Linq.Expressions.Expression<System.Func<ReordererTests.Entity, object>>>,
                    bool, System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyCollection<ReordererTests.Entity>>>;

    [TestClass]
    public class ReordererTests
    {
        [NotNull] private Expression<Func<Entity, bool>> _expression;
        [NotNull] private MockRepository _mockFactory;
        [NotNull] private Mock<IReadRepository<Entity>> _readRepository;
        [NotNull] private Reorderer<Entity, int> _reorderer;
        [NotNull] private Mock<IUpdateRepository<Entity>> _updateRepository;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _readRepository = _mockFactory.Create<IReadRepository<Entity>>();
            _updateRepository = _mockFactory.Create<IUpdateRepository<Entity>>();
            _reorderer = new Reorderer<Entity, int>(_readRepository.Object, _updateRepository.Object);
            _expression = e => true;
        }

        private static IReadOnlyCollection<Entity> GetEntities()
        {
            return new[]
            {
                new Entity(1, 2),
                new Entity(2, 3),
                new Entity(3, 1),
                new Entity(4, 4),
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ReorderAsyncNullTest()
        {
            await _reorderer.ReorderAsync(null, 1, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task NormalizeAsyncNullTest()
        {
            await _reorderer.NormalizeAsync(null);
        }

        [TestMethod]
        public async Task ReorderAsyncTest()
        {
            var entities = GetEntities();
            var dict = entities.ToDictionary(x => x.Id);

            Assert.AreNotEqual(1, dict[1].Order);
            Assert.AreNotEqual(2, dict[2].Order);
            Assert.AreNotEqual(3, dict[3].Order);
            Assert.AreEqual(4, dict[4].Order);

            MockGet(entities);
            Updater valueFunction = (updater, @where, include, commit) =>
            {
                foreach (var entity in entities)
                {
                    updater(entity);
                }
                return Task.FromResult(entities);
            };
            _updateRepository.Setup(x => x.UpdateAsync(It.IsAny<Action<Entity>>(), _expression, null, false))
                             .Returns(valueFunction)
                             .Verifiable();
            _updateRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1)).Verifiable();

            await _reorderer.ReorderAsync(_expression, 1, null);

            Assert.AreEqual(1, dict[1].Order);
            Assert.AreEqual(2, dict[2].Order);
            Assert.AreEqual(3, dict[3].Order);
            Assert.AreEqual(4, dict[4].Order);
        }

        [TestMethod]
        public async Task ReorderAsync2Test()
        {
            var entities = GetEntities();
            var dict = entities.ToDictionary(x => x.Id);

            Assert.AreNotEqual(1, dict[1].Order);
            Assert.AreNotEqual(2, dict[2].Order);
            Assert.AreNotEqual(3, dict[3].Order);
            Assert.AreEqual(4, dict[4].Order);

            MockGet(entities);
            Updater valueFunction = (updater, @where, include, commit) =>
            {
                foreach (var entity in entities)
                {
                    updater(entity);
                }
                return Task.FromResult(entities);
            };
            _updateRepository.Setup(x => x.UpdateAsync(It.IsAny<Action<Entity>>(), _expression, null, false))
                             .Returns(valueFunction)
                             .Verifiable();
            _updateRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1)).Verifiable();

            await _reorderer.ReorderAsync(_expression, 1, 2);

            Assert.AreEqual(2, dict[1].Order);
            Assert.AreEqual(1, dict[2].Order);
            Assert.AreEqual(3, dict[3].Order);
            Assert.AreEqual(4, dict[4].Order);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task ReorderAsync3Test()
        {
            var entities = GetEntities();

            MockGet(entities);
            Updater valueFunction = (updater, @where, include, commit) =>
            {
                updater(new Entity(100, 25));
                return Task.FromResult(entities);
            };
            _updateRepository.Setup(x => x.UpdateAsync(It.IsAny<Action<Entity>>(), _expression, null, false))
                             .Returns(valueFunction)
                             .Verifiable();

            await _reorderer.ReorderAsync(_expression, 1, 2);
        }


        [TestMethod]
        public async Task NormalizeAsyncTest()
        {
            IReadOnlyCollection<Entity> entities = new[]
            {
                new Entity(10, 2),
                new Entity(20, 3),
                new Entity(30, 1),
                new Entity(40, 4),
            };
            MockGet(entities);
            var dict = entities.ToDictionary(x => x.Id);

            Assert.AreNotEqual(1, dict[1].Order);
            Assert.AreNotEqual(2, dict[2].Order);
            Assert.AreNotEqual(3, dict[3].Order);
            Assert.AreNotEqual(4, dict[4].Order);

            Updater valueFunction = (updater, @where, include, commit) =>
            {
                foreach (var entity in entities)
                {
                    updater(entity);
                }
                return Task.FromResult(entities);
            };
            _updateRepository.Setup(x => x.UpdateAsync(It.IsAny<Action<Entity>>(), _expression, null, false))
                             .Returns(valueFunction)
                             .Verifiable();
            _updateRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1)).Verifiable();

            await _reorderer.NormalizeAsync(_expression);

            Assert.AreEqual(3, dict[1].Order);
            Assert.AreEqual(1, dict[2].Order);
            Assert.AreEqual(2, dict[3].Order);
            Assert.AreEqual(4, dict[4].Order);
        }


        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        private void MockGet(IReadOnlyCollection<Entity> entities)
        {
            _readRepository.Setup(x => x.GetAsync(_expression,
                                                  It.IsAny<IReadOnlySelectExpression<Entity>>(),
                                                  It.IsAny<IEnumerable<IOrderByClause<Entity>>>()))
                           .Returns(Task.FromResult(entities))
                           .Verifiable();
        }

        public class Entity : IOrderedEntity, IEntityWithId<int>
        {
            /// <summary>Инициализирует новый экземпляр класса <see cref="T:System.Object" />.</summary>
            public Entity(int order, int id)
            {
                Order = order;
                Id = id;
            }

            public Entity()
            {
            }

            /// <summary>Ключ экземпляра</summary>
            public int Id { get; set; }

            /// <summary>
            ///     Порядок
            /// </summary>
            public int Order { get; set; }
        }
    }
}