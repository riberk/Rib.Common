namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ClousureTests
    {
        [TestMethod]
        public void ToClousureTest()
        {
            var res = Clousure.ToClousure(1);
            Assert.AreEqual(ExpressionType.MemberAccess, res.NodeType);
            var me = res as MemberExpression;
            Assert.IsTrue((me.Member.MemberType & MemberTypes.Field) != 0);

            var resVal = Expression.Lambda<Func<int>>(res).Compile()();
            Assert.AreEqual(1, resVal);
        }
    }
}