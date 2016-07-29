namespace Rib.Common.Application.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Helpers.Expressions;
    using TsSoft.AbstractMapper;
    using TsSoft.EntityRepository;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.EntityService.UpdateAction;

    [TestClass]
    public class RestCreateServiceTests
    {
        private readonly TestEntity _testEntity = new TestEntity();
        private Mock<ICreateRepository<TestEntity>> _createRepository;
        private MockRepository _mockFactory;
        private Mock<INewMaker> _newMaker;
        private Mock<IUpdateEntityActionFactory> _updateEntityActionFactory;
        private Mock<IUpdatePathProviderMapper<TestEntityCreateModel, TestEntity>> _updateMapper;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _createRepository = _mockFactory.Create<ICreateRepository<TestEntity>>();
            _updateMapper = _mockFactory.Create<IUpdatePathProviderMapper<TestEntityCreateModel, TestEntity>>();
            _updateEntityActionFactory = _mockFactory.Create<IUpdateEntityActionFactory>();
            _newMaker = _mockFactory.Create<INewMaker>();
        }

        private RestCreateService<TestEntity, TestEntityCreateModel, int> Create(Func<TestEntity, TestEntity, IEnumerable<Func<Task>>> createUpdater)
        {
            var paths = new Expression<Func<TestEntity, object>>[]
            {
                entity => entity.Id
            };
            _updateMapper.Setup(x => x.Paths).Returns(paths).Verifiable();
            _updateMapper.Setup(x => x.DeleteAndCreateOnPaths).Returns(paths).Verifiable();
            _updateEntityActionFactory.Setup(x => x.MakeAsyncUpdateAction(paths, paths)).Returns(createUpdater).Verifiable();
            return new RestCreateService<TestEntity, TestEntityCreateModel, int>(_createRepository.Object, _updateMapper.Object,
                _updateEntityActionFactory.Object, _newMaker.Object);
        }

        [TestMethod]
        public void Constructor() => Create(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1()
            => new RestCreateService<TestEntity, TestEntityCreateModel, int>(null, _updateMapper.Object, _updateEntityActionFactory.Object,
                _newMaker.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument2()
            => new RestCreateService<TestEntity, TestEntityCreateModel, int>(_createRepository.Object, null, _updateEntityActionFactory.Object,
                _newMaker.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument3()
            => new RestCreateService<TestEntity, TestEntityCreateModel, int>(_createRepository.Object, _updateMapper.Object, null, _newMaker.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument4()
            => new RestCreateService<TestEntity, TestEntityCreateModel, int>(_createRepository.Object, _updateMapper.Object,
                _updateEntityActionFactory.Object, null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task CreateAsyncArgumentNullTest1()
        {
            await Create(null).CreateAsync(null, entity => { });
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task CreateAsyncArgumentNullTest2()
        {
            await Create(null).CreateAsync(new TestEntityCreateModel(), null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task CreateAsyncArgumentNullTest3()
        {
            await Create(null).CreateAsync(null);
        }

        [TestMethod]
        public async Task CreateAsyncTest()
        {
            _newMaker.Setup(x => x.Create<TestEntity>()).Returns(() => _testEntity).Verifiable();
            const int id = 10;
            var createModel = new TestEntityCreateModel();
            var actionInvoked = false;
            var func1Invoked = false;
            var func2Invoked = false;
            Func<Task> func1 = () =>
            {
                func1Invoked = true;
                return Task.CompletedTask;
            };
            Func<Task> func2 = () =>
            {
                func2Invoked = true;
                return Task.CompletedTask;
            };
            Func<TestEntity, TestEntity, IEnumerable<Func<Task>>> updater = (entity, testEntity) => new[] {func1, func2};
            var value = new TestEntity {Id = id};
            _updateMapper.Setup(x => x.Map(createModel)).Returns(value).Verifiable();
            _createRepository.Setup(x => x.CreateAsync(_testEntity, true)).Returns(Task.FromResult(value)).Verifiable();
            var res = await Create(updater).CreateAsync(createModel, entity => actionInvoked = true);

            Assert.IsTrue(actionInvoked);
            Assert.IsTrue(func1Invoked);
            Assert.IsTrue(func2Invoked);
            Assert.AreEqual(id, res);
        }

        [TestMethod]
        public async Task CreateAsyncWithoutUpdaterTest()
        {
            _newMaker.Setup(x => x.Create<TestEntity>()).Returns(() => _testEntity).Verifiable();
            const int id = 10;
            var createModel = new TestEntityCreateModel();
            var func1Invoked = false;
            var func2Invoked = false;
            Func<Task> func1 = () =>
            {
                func1Invoked = true;
                return Task.CompletedTask;
            };
            Func<Task> func2 = () =>
            {
                func2Invoked = true;
                return Task.CompletedTask;
            };
            Func<TestEntity, TestEntity, IEnumerable<Func<Task>>> updater = (entity, testEntity) => new[] {func1, func2};
            var value = new TestEntity {Id = id};
            _updateMapper.Setup(x => x.Map(createModel)).Returns(value).Verifiable();
            _createRepository.Setup(x => x.CreateAsync(_testEntity, true)).Returns(Task.FromResult(value)).Verifiable();
            var res = await Create(updater).CreateAsync(createModel);

            Assert.IsTrue(func1Invoked);
            Assert.IsTrue(func2Invoked);
            Assert.AreEqual(id, res);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public async Task CreateAsyncWithNullRepoTest()
        {
            _newMaker.Setup(x => x.Create<TestEntity>()).Returns(() => _testEntity).Verifiable();
            var createModel = new TestEntityCreateModel();
            Func<Task> func1 = () => Task.CompletedTask;
            Func<TestEntity, TestEntity, IEnumerable<Func<Task>>> updater = (entity, testEntity) => new[] {func1};
            _updateMapper.Setup(x => x.Map(createModel)).Returns(_testEntity).Verifiable();
            _createRepository.Setup(x => x.CreateAsync(_testEntity, true)).Returns(Task.FromResult((TestEntity) null)).Verifiable();
            await Create(updater).CreateAsync(createModel, entity => { });
        }

        [TestMethod]
        public void CreateRepositoryTest()
        {
            Assert.AreEqual(_createRepository.Object, Create(null).CreateRepository);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        public class TestEntity : IEntityWithId<int>
        {
            /// <summary>
            ///     Ключ экземпляра
            /// </summary>
            public int Id { get; set; }
        }

        public class TestEntityCreateModel
        {
        }
    }
}