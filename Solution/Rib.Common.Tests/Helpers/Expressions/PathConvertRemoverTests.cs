namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.Expressions.Helpers;

    [TestClass]
    public class PathConvertRemoverTests
    {
        private MockRepository _mockFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
        }

        private PathConvertRemover Create()
        {
            return new PathConvertRemover();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void RemoveLastArgumentNullTest()
        {
            Create().RemoveLast(null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void RemoveLastArgumentOutOfRangeExceptionTest()
        {
            Expression<Func<string>> f = () => "";
            Create().RemoveLast(f);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidCastException))]
        public void RemoveLastWithInvalidCastTest()
        {
            Expression<Func<TestClass, bool>> f = c => c.Id == 1;
            Create().RemoveLast(f);
        }

        [TestMethod]
        public void RemoveLastWithMemberExpressionTest()
        {
            Expression<Func<TestClass, int>> f = c => c.Id;
            var res = Create().RemoveLast(f);

            Assert.AreNotEqual(f, res);
            Assert.IsTrue(ExpressionEqualityComparer.Instance.Equals(f, res));
        }

        [TestMethod]
        public void RemoveLastTest()
        {
            Expression<Func<TestClass, object>> f = c => (object) c.Id;
            Expression<Func<TestClass, int>> fWithoutCast = c => c.Id;
            var res = Create().RemoveLast(f);

            Assert.AreNotEqual(f, res);
            Assert.AreNotEqual(fWithoutCast, res);
            Assert.IsTrue(ExpressionEqualityComparer.Instance.Equals(fWithoutCast, res));
        }


        [TestMethod]
        public void Constructor() => new PathConvertRemover();

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