namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ParameterRebinderTests
    {
        private MockRepository _mockFactory;
        private Mock<IParameterMapFactory> _parameterMapFactory;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _parameterMapFactory = _mockFactory.Create<IParameterMapFactory>();
        }

        private ParameterRebinder Create()
        {
            return new ParameterRebinder(_parameterMapFactory.Object);
        }

        [TestMethod]
        public void Constructor() => new ParameterRebinder(_parameterMapFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReplaceParametersByFirstArgNullTest()
        {
            var rebinder = Create();
            rebinder.ReplaceParametersByFirst(null).ToList();
        }


        [TestMethod]
        public void ReplaceParametersByFirstTest()
        {
            var p1 = Expression.Parameter(typeof (int), "p1");
            var l1 = Expression.Lambda(Expression.Add(p1, p1), p1);

            var p2 = Expression.Parameter(typeof (int), "p2");
            var l2 = Expression.Lambda(Expression.Add(p2, p2), p2);

            var map = new Dictionary<ParameterExpression, ParameterExpression>()
            {
                {p2, p1}
            };

            _parameterMapFactory
                    .Setup(x => x.Create(l1, It.IsAny<IEnumerable<LambdaExpression>>()))
                    .Returns((LambdaExpression s, IEnumerable<LambdaExpression> ll) =>
                    {
                        Assert.AreEqual(1, ll.Count());
                        Assert.AreEqual(l2, ll.Single());
                        return map;
                    });


            var lambdas = Create().ReplaceParametersByFirst(l1, l2).ToArray();

            Assert.AreEqual(2, lambdas.Length);
            Assert.AreEqual(l1.Body, lambdas[0]);
            Assert.AreNotEqual(l2.Body, lambdas[1]);
            Assert.AreEqual(ExpressionType.Add, lambdas[1].NodeType);
            var be = lambdas[1] as BinaryExpression;
            Assert.AreEqual(p1, be.Left);
            Assert.AreEqual(p1, be.Right);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}