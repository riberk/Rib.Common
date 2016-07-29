namespace Rib.Common.Application.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Application.Models.Rest;
    using Rib.Common.Models.Contract;
    using TsSoft.EntityRepository;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.EntityService;
    using TsSoft.Expressions.OrderBy;

    [TestClass]
    public class RestServiceTests
    {
        private Mock<IRestCreateService<Entity, Model, int>> _create;
        private Mock<IRestDeleteService<Entity>> _delete;
        private Mock<IRestGetService<Entity, Model>> _get;
        private MockRepository _mockFactory;
        private RestService<Entity, Model, Model, Model, int> _rest;
        private Mock<IRestUpdateService<Entity, Model>> _update;

        [TestInitialize]
        public void TestInit()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _get = _mockFactory.Create<IRestGetService<Entity, Model>>();
            _update = _mockFactory.Create<IRestUpdateService<Entity, Model>>();
            _create = _mockFactory.Create<IRestCreateService<Entity, Model, int>>();
            _delete = _mockFactory.Create<IRestDeleteService<Entity>>();
            _rest = new RestService<Entity, Model, Model, Model, int>(_get.Object, _create.Object, _update.Object, _delete.Object);
        }

        [TestMethod]
        public void CreateAsyncTest()
        {
            var model = new Model();
            var fromResult = Task.FromResult(1);
            _create.Setup(x => x.CreateAsync(model)).Returns(fromResult).Verifiable();
            Assert.AreEqual(fromResult, _rest.CreateAsync(model));
        }

        [TestMethod]
        public void CreateAsyncTest1()
        {
            var model = new Model();
            Action<Entity> a = entity => { };
            var fromResult = Task.FromResult(1);
            _create.Setup(x => x.CreateAsync(model, a)).Returns(fromResult).Verifiable();
            Assert.AreEqual(fromResult, _rest.CreateAsync(model, a));
        }

        [TestMethod]
        public void DeleteSingleAsyncTest()
        {
            Expression<Func<Entity, bool>> e = entity => true;
            var t = Task.FromResult(1);
            _delete.Setup(x => x.DeleteSingleAsync(e)).Returns(t).Verifiable();
            Assert.AreEqual(t, _rest.DeleteSingleAsync(e));
        }

        [TestMethod]
        public void GetSingleRowAsyncTest()
        {
            Expression<Func<Entity, bool>> e = entity => true;
            var t = Task.FromResult(new Model());
            _get.Setup(x => x.GetSingleRowAsync(e)).Returns(t).Verifiable();
            Assert.AreEqual(t, _rest.GetSingleRowAsync(e));
        }

        [TestMethod]
        public void GetTableAsyncTest()
        {
            Expression<Func<Entity, bool>> e = entity => true;
            var t = Task.FromResult((IReadOnlyCollection<Model>) Enumerable.Empty<Model>().ToList());
            var order = Enumerable.Empty<IOrderByClause<Entity>>();
            _get.Setup(x => x.GetTableAsync(e, order)).Returns(t).Verifiable();
            Assert.AreEqual(t, _rest.GetTableAsync(e, order));
        }

        [TestMethod]
        public void GetPagedAsyncTest()
        {
            Expression<Func<Entity, bool>> e = entity => true;
            var t = Task.FromResult((IPagedResponse<Model>) new PagedResponse<Model>(new Model[0], 10));
            var order = Enumerable.Empty<IOrderByClause<Entity>>();
            var paginator = Paginator.Full;

            _get.Setup(x => x.GetPagedAsync(paginator, e, order)).Returns(t).Verifiable();
            Assert.AreEqual(t, _rest.GetPagedAsync(paginator, e, order));
        }

        [TestMethod]
        public void GetPagedAsyncTest1()
        {
            Expression<Func<Entity, bool>> e = entity => true;
            var t = Task.FromResult((IPagedResponse<Model>) new PagedResponse<Model>(new Model[0], 10));
            var req = _mockFactory.Create<IOrderedPaginationRequest>(MockBehavior.Loose);
            var paginator = Paginator.Full;

            _get.Setup(x => x.GetPagedAsync(e, req.Object)).Returns(t).Verifiable();
            Assert.AreEqual(t, _rest.GetPagedAsync(e, req.Object));
        }

        [TestMethod]
        public void GetPagedAsyncTest2()
        {
            var t = Task.FromResult((IPagedResponse<Model>) new PagedResponse<Model>(new Model[0], 10));
            var req = _mockFactory.Create<IOrderedPaginationPredicateRequest<Entity>>(MockBehavior.Loose);

            _get.Setup(x => x.GetPagedAsync(req.Object)).Returns(t).Verifiable();
            Assert.AreEqual(t, _rest.GetPagedAsync(req.Object));
        }

        [TestMethod]
        public void UpdateSingleAsyncTest()
        {
            Expression<Func<Entity, bool>> e = entity => true;
            var m = new Model();
            var t = Task.FromResult(1);

            _update.Setup(x => x.UpdateSingleAsync(e, m)).Returns(t).Verifiable();
            Assert.AreEqual(t, _rest.UpdateSingleAsync(e, m));
        }

        [TestMethod]
        public void PropsProvidedTest()
        {
            var createRepo = _mockFactory.Create<ICreateRepository<Entity>>();
            var updateRepo = _mockFactory.Create<IUpdateRepository<Entity>>();
            var deleteRepo = _mockFactory.Create<IDeleteRepository<Entity>>();
            var getRepo = _mockFactory.Create<IReadDatabaseService<Entity, Model>>();

            _create.Setup(x => x.CreateRepository).Returns(createRepo.Object).Verifiable();
            _update.Setup(x => x.UpdateRepository).Returns(updateRepo.Object).Verifiable();
            _delete.Setup(x => x.DeleteRepository).Returns(deleteRepo.Object).Verifiable();
            _get.Setup(x => x.ReadService).Returns(getRepo.Object).Verifiable();

            Assert.AreEqual(createRepo.Object, _rest.CreateRepository);
            Assert.AreEqual(updateRepo.Object, _rest.UpdateRepository);
            Assert.AreEqual(deleteRepo.Object, _rest.DeleteRepository);
            Assert.AreEqual(getRepo.Object, _rest.ReadService);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        public class Entity : IEntityWithId<int>
        {
            /// <summary>Ключ экземпляра</summary>
            public int Id { get; set; }
        }

        public class Model
        {
        }
    }
}