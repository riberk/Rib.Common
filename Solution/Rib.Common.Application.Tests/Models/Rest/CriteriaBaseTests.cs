namespace Rib.Common.Application.Models.Rest
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CriteriaBaseTests
    {
        [TestMethod]
        public void PredicateTest()
        {
            var e = new Cb().Predicate();
            Assert.AreEqual(1, e.Parameters.Count);
            Assert.AreEqual(typeof(string), e.Parameters[0].Type);

            Assert.AreEqual(ExpressionType.AndAlso, e.Body.NodeType);
            var andExpr = (BinaryExpression) e.Body;

            Assert.AreEqual(ExpressionType.Constant, andExpr.Left.NodeType);
            Assert.AreEqual(true, ((ConstantExpression) andExpr.Left).Value);

            Assert.AreEqual(ExpressionType.Constant, andExpr.Right.NodeType);
            Assert.AreEqual(false, ((ConstantExpression) andExpr.Right).Value);
        }

        public class Cb : CriteriaBase<string>
        {
            public override Expression<Func<string, bool>> Predicate()
            {
                return CreateBuilder().And(x => false).BuildPredicate();
            }
        }
    }
}