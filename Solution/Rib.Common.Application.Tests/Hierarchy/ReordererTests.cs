namespace Rib.Common.Application.Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Models.Interfaces;
    using TsSoft.EntityRepository;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.Expressions.OrderBy;
    using TsSoft.Expressions.SelectBuilder.Models;
    using Updater =
            System.Func
                    <System.Action<Entity>, System.Linq.Expressions.Expression<System.Func<Entity, bool>>,
                            System.Collections.Generic.IEnumerable
                                    <System.Linq.Expressions.Expression<System.Func<Entity, object>>>,
                            bool, System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyCollection<Entity>>>;

    [TestClass]
    public class ReordererTests
    {
        [NotNull] private Expression<Func<Entity, bool>> _expression;
        [NotNull] private MockRepository _mockFactory;
        [NotNull] private Reorderer<Entity, int> _reorderer;
        [NotNull] private Mock<IBaseRepository<Entity>> _repository;

        [TestInitialize]
        public void Init()
        {
            //_mockFactory = new MockRepository(MockBehavior.Strict);
            //var repo = new Mock<IRawSqlWriteRepository>();
            //var o = repo.Object;
            //_repository = _mockFactory.Create<IBaseRepository<Entity>>();
            //_reorderer = new Reorderer<Entity, int>(_repository.Object);
            //_expression = e => true;
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
        //[ExpectedException(typeof(ArgumentNullException))]
        public void ReorderAsyncNullTest()
        {
            

            var aName = new AssemblyName("DynamicAssemblyExample");
            var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
            var mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");
            var tb = mb.DefineType("MyDynamicType", TypeAttributes.Public);
            var methodbuilder = tb.DefineMethod("Create", MethodAttributes.Public, typeof(int), new[] {typeof(IsolationLevel?) });
            var parameterBuilder = methodbuilder.DefineParameter(1, ParameterAttributes.None, "param2");
            parameterBuilder.SetConstant((int?)IsolationLevel.ReadUncommitted);
            
            //await _reorderer.ReorderAsync(null, 1, 10);
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
            Assert.AreNotEqual(4, dict[4].Order);
            MockGet(entities);
            Updater valueFunction = (updater, @where, include, commit) =>
            {
                foreach (var entity in entities)
                {
                    updater(entity);
                }
                return Task.FromResult(entities);
            };
            _repository.Setup(x => x.UpdateAsync(It.IsAny<Action<Entity>>(), _expression, null, false))
                       .Returns(valueFunction)
                       .Verifiable();
            _repository.Setup(x =>x.SaveChangesAsync()).Returns(Task.FromResult(1)).Verifiable();

            await _reorderer.ReorderAsync(_expression, 1, null);

            Assert.AreEqual(1, dict[1].Order);
            Assert.AreEqual(2, dict[2].Order);
            Assert.AreEqual(3, dict[3].Order);
            Assert.AreEqual(4, dict[4].Order);
        }


        [TestMethod]
        public void NormalizeAsyncTest()
        {
            var repo = new Mock<ITest>();
            var o = repo.Object;
        }

        public interface ITest
        {
            int Execute(IsolationLevel? transactionIsolationLevel = IsolationLevel.ReadUncommitted);
        }


        [TestMethod]
        public void NormalizeAsyncTest1()
        {
            Assert.Fail();
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        private void MockGet(IReadOnlyCollection<Entity> entities)
        {
            _repository.Setup(x => x.GetAsync(_expression,
                                              It.IsAny<IReadOnlySelectExpression<Entity>>(),
                                              It.IsAny<IEnumerable<IOrderByClause<Entity>>>()))
                       .Returns(Task.FromResult(entities))
                       .Verifiable();
        }

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