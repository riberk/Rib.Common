namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParameterMapFactoryTests
    {
        internal ParameterMapFactory Create()
        {
            return new ParameterMapFactory();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateNullArg1Test() => Create().Create(null, new LambdaExpression[0]);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void CreateNullArg2Test()
        {
            Expression<Func<string>> ss = () => "123";
            Create().Create(ss, null);
        }

        [TestMethod]
        public void CreateTest()
        {
            var p11 = Expression.Parameter(typeof (int), "p11");
            var p12 = Expression.Parameter(typeof (int), "p12");
            var l1 = Expression.Lambda(Expression.Add(p11, p12), p11, p12);


            var p21 = Expression.Parameter(typeof (int), "p21");
            var p22 = Expression.Parameter(typeof (int), "p22");
            var l2 = Expression.Lambda(Expression.Add(p21, p22), p21, p22);

            var p31 = Expression.Parameter(typeof (int), "p31");
            var p32 = Expression.Parameter(typeof (int), "p32");
            var l3 = Expression.Lambda(Expression.Add(p31, p32), p31, p32);

            var map = Create().Create(l1, new[] {l2, l3});

            Assert.AreEqual(4, map.Count);

            Assert.IsTrue(map.ContainsKey(p21));
            Assert.IsTrue(map.ContainsKey(p22));
            Assert.IsTrue(map.ContainsKey(p31));
            Assert.IsTrue(map.ContainsKey(p32));

            Assert.AreEqual(p11, map[p21]);
            Assert.AreEqual(p12, map[p22]);
            Assert.AreEqual(p11, map[p31]);
            Assert.AreEqual(p12, map[p32]);
        }
    }
}