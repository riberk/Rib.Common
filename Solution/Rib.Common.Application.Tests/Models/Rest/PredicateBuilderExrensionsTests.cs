namespace Rib.Common.Application.Models.Rest
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TsSoft.Expressions.Helpers;

    [TestClass]
    public class PredicateBuilderExrensionsTests
    {
        [TestMethod]
        public void AndTest()
        {
            var mock = new Mock<IPredicateBuilder<string>>(MockBehavior.Strict);
            Assert.AreEqual(mock.Object, mock.Object.And(s => true, false));
            mock.VerifyAll();
        }

        [TestMethod]
        public void AndTest1()
        {
            var mock = new Mock<IPredicateBuilder<string>>(MockBehavior.Strict);
            Assert.AreEqual(mock.Object, mock.Object.And(s => true, false));
            mock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AndTestNull()
        {
            Expression<Func<string, bool>> expression = s => true;
            PredicateBuilderExrensions.And(null, expression, true);
        }

        [TestMethod]
        public void OrTest()
        {
            var mock = new Mock<IPredicateBuilder<string>>(MockBehavior.Strict);
            Assert.AreEqual(mock.Object, mock.Object.Or(s => true, false));
            mock.VerifyAll();
        }

        [TestMethod]
        public void OrTest1()
        {
            var mock = new Mock<IPredicateBuilder<string>>(MockBehavior.Strict);
            Assert.AreEqual(mock.Object, mock.Object.Or(s => true, false));
            mock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrTestNull()
        {
            Expression<Func<string, bool>> expression = s => true;
            PredicateBuilderExrensions.Or(null, expression, true);
        }


    }
}