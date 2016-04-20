namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExpressionsExtensionsConvertRemoverTests
    {
        [TestMethod]
        public void RemoveTest()
        {
            var notRemoveConvert = Create<Test>();
            Assert.AreEqual(ExpressionType.Equal, notRemoveConvert.Body.NodeType);
            var nBody = notRemoveConvert.Body as BinaryExpression;
            Assert.AreEqual(ExpressionType.Constant, nBody.Right.NodeType);
            Assert.AreEqual("123", (nBody.Right as ConstantExpression).Value);
            Assert.AreEqual(ExpressionType.MemberAccess, nBody.Left.NodeType);
            var nMemberAccess = nBody.Left as MemberExpression;
            Assert.AreEqual(typeof (ITestI).GetProperty("A"), nMemberAccess.Member);
            Assert.AreEqual(ExpressionType.Convert, nMemberAccess.Expression.NodeType);
            var nConvert = nMemberAccess.Expression as UnaryExpression;
            Assert.AreEqual(ExpressionType.Parameter, nConvert.Operand.NodeType);

            var expr = notRemoveConvert.RemoveConvert();
            Assert.AreEqual(ExpressionType.Equal, expr.Body.NodeType);
            var body = expr.Body as BinaryExpression;
            Assert.AreEqual(ExpressionType.Constant, body.Right.NodeType);
            Assert.AreEqual("123", (body.Right as ConstantExpression).Value);
            Assert.AreEqual(ExpressionType.MemberAccess, body.Left.NodeType);
            var memberAccess = body.Left as MemberExpression;
            Assert.AreEqual(typeof (ITestI).GetProperty("A"), memberAccess.Member);
            Assert.AreEqual(ExpressionType.Parameter, memberAccess.Expression.NodeType);
        }

        [TestMethod]
        public void RemoveWithoutCastTest()
        {
            var notRemoveConvert = CreateObjectCast();
            Assert.AreEqual(ExpressionType.Convert, notRemoveConvert.Body.NodeType);
            var nBody = notRemoveConvert.Body as UnaryExpression;
            Assert.AreEqual(ExpressionType.Parameter, nBody.Operand.NodeType);
            
            var expr =notRemoveConvert.RemoveConvert();

            Assert.AreEqual(ExpressionType.Convert, expr.Body.NodeType);
            var body = expr.Body as UnaryExpression;
            Assert.AreEqual(ExpressionType.Parameter, body.Operand.NodeType);
        }

        [TestMethod]
        public void RemoveWithoutCastUnary1Test()
        {
            Expression<Func<Test, bool>> notRemoveConvert = test => !test.B;
            Assert.AreEqual(ExpressionType.Not, notRemoveConvert.Body.NodeType);
            var nBody = notRemoveConvert.Body as UnaryExpression;
            Assert.AreEqual(ExpressionType.MemberAccess, nBody.Operand.NodeType);
            var ma = nBody.Operand as MemberExpression;
            Assert.AreEqual(typeof(Test).GetProperty(nameof(Test.B)), ma.Member);
            Assert.AreEqual(ExpressionType.Parameter, ma.Expression.NodeType);

            var expr =notRemoveConvert.RemoveConvert();

            Assert.AreEqual(ExpressionType.Not, expr.Body.NodeType);
            var eBody = expr.Body as UnaryExpression;
            Assert.AreEqual(ExpressionType.MemberAccess, eBody.Operand.NodeType);
            var ema = eBody.Operand as MemberExpression;
            Assert.AreEqual(typeof(Test).GetProperty(nameof(Test.B)), ema.Member);
            Assert.AreEqual(ExpressionType.Parameter, ema.Expression.NodeType);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void RemoveNullArgumentTest() => ExpressionExtensions.RemoveConvert<Expression>(null);

        [NotNull]
        private static Expression<Func<T, bool>> Create<T>()
                where T : ITestI
        {
            return arg => arg.A == "123";
        }

        [NotNull]
        private static Expression<Func<Test, object>> CreateObjectCast()
        {
            // ReSharper disable once RedundantCast
            return arg => (object)arg;
        }

        private interface ITestI
        {
            string A { get; set; }
        }

        [UsedImplicitly]
        private class Test : ITestI
        {
            public string A { get; set; }
            public bool B { get; set; }
        }
    }
}