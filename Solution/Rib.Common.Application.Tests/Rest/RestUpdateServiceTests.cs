namespace CallTeaser.Logic.Services.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Application.Rest;
    using TsSoft.AbstractMapper;
    using TsSoft.EntityRepository;
    using TsSoft.EntityService.UpdateAction;
    using TsSoft.Expressions.IncludeBuilder;

    [TestClass]
    public class RestUpdateServiceTests
    {
        private readonly Expression<Func<TestEntity, object>>[] _dropCreateOnPaths = new Expression<Func<TestEntity, object>>[0];
        private readonly Expression<Func<TestEntity, object>>[] _includes = new Expression<Func<TestEntity, object>>[0];
        private readonly Expression<Func<TestEntity, object>>[] _paths = new Expression<Func<TestEntity, object>>[0];

        private readonly TestEntity _testEntity = new TestEntity();
        private readonly TestEntityUpdateModel _testEntityModel = new TestEntityUpdateModel();
        private MockRepository _mockFactory;
        private Mock<IPathToDbIncludeConverter> _pathToDbIncludeConverter;
        private Mock<IUpdateEntityActionFactory> _updateEntityActionFactory;
        private Mock<IUpdatePathProviderMapper<TestEntityUpdateModel, TestEntity>> _updateMapper;
        private Mock<IUpdateRepository<TestEntity>> _updateRepository;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _updateRepository = _mockFactory.Create<IUpdateRepository<TestEntity>>();
            _updateMapper = _mockFactory.Create<IUpdatePathProviderMapper<TestEntityUpdateModel, TestEntity>>();
            _updateEntityActionFactory = _mockFactory.Create<IUpdateEntityActionFactory>();
            _pathToDbIncludeConverter = _mockFactory.Create<IPathToDbIncludeConverter>();
        }

        private RestUpdateService<TestEntity, TestEntityUpdateModel> Create(Func<TestEntity, TestEntity, IEnumerable<Func<Task>>> createUpdater)
        {
            _updateMapper.Setup(x => x.Paths).Returns(_paths).Verifiable();
            _updateMapper.Setup(x => x.DeleteAndCreateOnPaths).Returns(_dropCreateOnPaths).Verifiable();
            _updateEntityActionFactory.Setup(x => x.MakeAsyncUpdateAction(_paths, _dropCreateOnPaths)).Returns(createUpdater).Verifiable();
            _pathToDbIncludeConverter.Setup(x => x.GetIncludes(_paths, _dropCreateOnPaths)).Returns(_includes).Verifiable();

            return new RestUpdateService<TestEntity, TestEntityUpdateModel>(_updateRepository.Object, _updateMapper.Object,
                _updateEntityActionFactory.Object, _pathToDbIncludeConverter.Object);
        }

        [TestMethod]
        public void RepositoryTest()
        {
            Assert.AreEqual(_updateRepository.Object, Create(null).UpdateRepository);
        }

        [TestMethod]
        public async Task UpdateSingleAsyncTest()
        {
            Func<TestEntity, TestEntity, IEnumerable<Func<Task>>> updater = (entity, testEntity) => new Func<Task>[0];
            Expression<Func<TestEntity, bool>> expression = entity => true;

            _updateMapper.Setup(x => x.Map(_testEntityModel)).Returns(_testEntity).Verifiable();
            _updateRepository.Setup(x => x.UpdateSingleFromAsync(updater, expression, _includes, _testEntity, true))
                .Returns(Task.FromResult(_testEntity))
                .Verifiable();

            await Create(updater).UpdateSingleAsync(expression, _testEntityModel);
        }

        [TestMethod]
        [ExpectedException(typeof (ObjectNotFoundException))]
        public async Task UpdateSingleAsyncWithNullTest()
        {
            Func<TestEntity, TestEntity, IEnumerable<Func<Task>>> updater = (entity, testEntity) => new Func<Task>[0];
            Expression<Func<TestEntity, bool>> expression = entity => true;

            _updateMapper.Setup(x => x.Map(_testEntityModel)).Returns(_testEntity).Verifiable();
            _updateRepository.Setup(x => x.UpdateSingleFromAsync(updater, expression, _includes, _testEntity, true))
                .Returns(Task.FromResult((TestEntity) null))
                .Verifiable();

            await Create(updater).UpdateSingleAsync(expression, _testEntityModel);
        }

        public class TestEntity
        {
        }

        public class TestEntityUpdateModel
        {
        }
    }
}