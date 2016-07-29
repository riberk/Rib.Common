namespace Rib.Common.Application.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Application.Models.Rest;
    using Rib.Common.Application.Rest.Infrastructure;
    using Rib.Common.Models.Contract;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.EntityRepository.Models;
    using TsSoft.EntityService;
    using TsSoft.Expressions.Helpers.Extensions;
    using TsSoft.Expressions.OrderBy;

    [TestClass]
    public class RestGetServiceTests
    {
        private readonly IOrderByClause<TestEntity>[] _orderByClauses =
        {
            OrderByClause<TestEntity>.Create(entity => entity.Id)
        };

        private readonly Paginator _paginator = new Paginator(2, 10);
        private Expression<Func<TestEntity, bool>> _expression;
        private MockRepository _mockFactory;
        private Mock<IOrderCreator<TestEntity>> _orderCreator;
        private Mock<IReadDatabaseService<TestEntity, TestEntityTableModel>> _readDbService;
        private TestEntityTableModel _testEntityTableModel;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _readDbService = _mockFactory.Create<IReadDatabaseService<TestEntity, TestEntityTableModel>>();
            _orderCreator = _mockFactory.Create<IOrderCreator<TestEntity>>();
            _expression = entity => entity.Id == 1;
            _testEntityTableModel = new TestEntityTableModel();
        }

        private RestGetService<TestEntity, TestEntityTableModel> Create()
        {
            return new RestGetService<TestEntity, TestEntityTableModel>(_readDbService.Object, _orderCreator.Object);
        }

        [TestMethod]
        public void Constructor()
                => new RestGetService<TestEntity, TestEntityTableModel>(_readDbService.Object, _orderCreator.Object);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullArgument1()
                => new RestGetService<TestEntity, TestEntityTableModel>(null, _orderCreator.Object);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullArgument3()
                => new RestGetService<TestEntity, TestEntityTableModel>(_readDbService.Object, null);

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public async Task GetSingleRowAsyncWithNullTest()
        {
            _readDbService.Setup(x => x.GetSingleAsync(_expression)).Returns(Task.FromResult((TestEntityTableModel) null)).Verifiable();
            await Create().GetSingleRowAsync(_expression);
        }

        [TestMethod]
        public async Task GetSingleRowAsyncTest()
        {
            _readDbService.Setup(x => x.GetSingleAsync(_expression)).Returns(Task.FromResult(_testEntityTableModel)).Verifiable();
            Assert.AreEqual(_testEntityTableModel, await Create().GetSingleRowAsync(_expression));
        }

        [TestMethod]
        public async Task GetTableAsyncTest()
        {
            _readDbService.Setup(x => x.GetAsync(_expression, _orderByClauses))
                          .Returns(Task.FromResult(_testEntityTableModel.ToEnumerable()))
                          .Verifiable();

            var testEntityTableModels = await Create().GetTableAsync(_expression, _orderByClauses);

            Assert.AreEqual(_testEntityTableModel, testEntityTableModels.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetPagedAsyncArgumentNullException()
        {
            await Create().GetPagedAsync(null, _expression, _orderByClauses);
        }

        [TestMethod]
        public async Task GetPagedAsyncTest()
        {
            var pagedListResult = MockGetPaged();

            var actual = await Create().GetPagedAsync(_paginator, _expression, _orderByClauses);

            Assert.AreEqual(pagedListResult.Result, actual.Value);
            Assert.AreEqual(pagedListResult.TotalRecords, actual.Count);
        }

        

        [TestMethod]
        public async Task GetTableAsyncGetRequestWithNull()
        {
            _readDbService.Setup(x => x.GetAsync(null, null))
                          .Returns(Task.FromResult(_testEntityTableModel.ToEnumerable()))
                          .Verifiable();

            var testEntityTableModels = await Create().GetTableAsync(null);

            Assert.AreEqual(_testEntityTableModel, testEntityTableModels.Single());
        }

        [TestMethod]
        public async Task GetPagedAsyncWithPredicateRequestTest()
        {
            var request= _mockFactory.Create<IOrderedPaginationPredicateRequest<TestEntity>>();
            var order = new Dictionary<string, bool>();
            _orderCreator.Setup(x => x.Create(order)).Returns(_orderByClauses).Verifiable();
            request.Setup(x => x.Pagination).Returns(_paginator).Verifiable();
            request.Setup(x => x.Order).Returns(order).Verifiable();
            request.Setup(x => x.Predicate()).Returns(_expression).Verifiable();

            var pagedListResult = MockGetPaged();
            var actual = await Create().GetPagedAsync(request.Object);

            Assert.AreEqual(pagedListResult.Result, actual.Value);
            Assert.AreEqual(pagedListResult.TotalRecords, actual.Count);
        }

        [TestMethod]
        public async Task GetPagedAsyncWithPredicateRequestWithNullTest()
        {
            var pagedListResult = MockGetPaged(predicate: RestGetService<TestEntity, TestEntityTableModel>.True, paginator: Paginator.Full);
            _orderCreator.Setup(x => x.Create(null)).Returns(_orderByClauses).Verifiable();
            var actual = await Create().GetPagedAsync(null);

            Assert.AreEqual(pagedListResult.Result, actual.Value);
            Assert.AreEqual(pagedListResult.TotalRecords, actual.Count);
        }

        [TestMethod]
        public async Task GetPagedAsyncWithRequestTest()
        {
            var request = _mockFactory.Create<IOrderedPaginationRequest>();
            var order = new Dictionary<string, bool>();
            _orderCreator.Setup(x => x.Create(order)).Returns(_orderByClauses).Verifiable();
            request.Setup(x => x.Pagination).Returns(_paginator).Verifiable();
            request.Setup(x => x.Order).Returns(order).Verifiable();

            var pagedListResult = MockGetPaged();
            var actual = await Create().GetPagedAsync(_expression, request.Object);

            Assert.AreEqual(pagedListResult.Result, actual.Value);
            Assert.AreEqual(pagedListResult.TotalRecords, actual.Count);
        }

        [TestMethod]
        public async Task GetPagedAsyncWithRequestWithNullTest()
        {
            var pagedListResult = MockGetPaged(predicate: RestGetService<TestEntity, TestEntityTableModel>.True, paginator: Paginator.Full);
            _orderCreator.Setup(x => x.Create(null)).Returns(_orderByClauses).Verifiable();
            var actual = await Create().GetPagedAsync(null, (IOrderedPaginationRequest)null);

            Assert.AreEqual(pagedListResult.Result, actual.Value);
            Assert.AreEqual(pagedListResult.TotalRecords, actual.Count);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        private IPage<TestEntityTableModel> MockGetPaged(IEnumerable<TestEntityTableModel> models = null, Expression<Func<TestEntity, bool>> predicate = null, IPaginator paginator = null, IEnumerable<IOrderByClause<TestEntity>> order = null)
        {
            models = models ?? _testEntityTableModel.ToEnumerable();
            predicate = predicate ?? _expression;
            paginator = paginator ?? _paginator;
            order = order ?? _orderByClauses;

            IPage<TestEntityTableModel> pagedListResult = new Page<TestEntityTableModel>(1, models);

            _readDbService.Setup(x => x.GetPagedAsync(predicate, paginator.PageNumber, paginator.PageSize, order))
                          .Returns(Task.FromResult(pagedListResult))
                          .Verifiable();
            return pagedListResult;
        }

        public class TestEntity : IEntityWithId<int>
        {
            /// <summary>
            ///     Ключ экземпляра
            /// </summary>
            public int Id { get; set; }
        }

        public class TestEntityTableModel
        {
        }

    }
}