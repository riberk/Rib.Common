namespace CallTeaser.Logic.Services.Rest
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Application.Rest;
    using TsSoft.EntityRepository;
    using TsSoft.EntityRepository.Interfaces;

    [TestClass]
    public class RestDeleteServiceTests
    {
        private Mock<IDeleteRepository<TestClass>> _deleteRepository;
        private MockRepository _mockFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _deleteRepository = _mockFactory.Create<IDeleteRepository<TestClass>>();
        }

        private RestDeleteService<TestClass> Create()
        {
            return new RestDeleteService<TestClass>(_deleteRepository.Object);
        }

        [TestMethod]
        public void Constructor() => new RestDeleteService<TestClass>(_deleteRepository.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new RestDeleteService<TestClass>(null);

        [TestMethod]
        public async Task DeleteAsyncTest()
        {
            const int id = 10;
            Expression<Func<TestClass, bool>> predicate = @class => @class.Id == id;
            _deleteRepository.Setup(x => x.DeleteSingleAsync(predicate, true)).Returns(Task.FromResult(new TestClass())).Verifiable();
            await Create().DeleteSingleAsync(predicate);
        }

        [TestMethod]
        public void DeleteRepositoryTest()
        {
            Assert.AreEqual(_deleteRepository.Object, Create().DeleteRepository);
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