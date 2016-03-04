namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Models.Exceptions;

    [TestClass]
    public class MemberInitMergerTests
    {
        private MockRepository _mockFactory;
        private Mock<IParameterRebinder> _parameterRebinder;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _parameterRebinder = _mockFactory.Create<IParameterRebinder>();
        }

        private MemberInitMerger Create()
        {
            return new MemberInitMerger(_parameterRebinder.Object);
        }

        [TestMethod]
        public void Constructor() => new MemberInitMerger(_parameterRebinder.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void MergeArgnullTest()
        {
            ParameterExpression pe;
            Create().Merge<string>(null, out pe);
        }

        [TestMethod]
        [ExpectedException(typeof(MetadataException))]
        public void MergeWithoutEmptyCtorTest()
        {
            ParameterExpression pe;
            Create().Merge<WithoutParameterlessCtor>(new LambdaExpression[0], out pe);
        }

        [TestMethod]
        [ExpectedException(typeof(RibCommonException))]
        public void MergeUndefinedExpressionTest()
        {
            ParameterExpression pe;
            var expressions = new Expression<Func<Test, int>>[]
            {
                t => 1
            };
            _parameterRebinder.Setup(x => x.ReplaceParametersByFirst(expressions)).Returns(expressions.Select(x => x.Body)).Verifiable();

            Create().Merge<Test>(expressions, out pe);
        }

        [TestMethod]
        public void MergeTest()
        {
            var expressions = new Expression<Func<Test, Test>>[]
            {
                t => new Test {A = t.A},
                t => new Test {B = t.B},
                t => new Test {C = t.C},
                t => new Test {D = t.D},
            };
            var oneParamExpressions = expressions.Select(x => Expression.Lambda<Func<Test, Test>>(x.Body, expressions[0].Parameters[0])).ToList();
            ParameterExpression pe;
            _parameterRebinder.Setup(x => x.ReplaceParametersByFirst(expressions)).Returns(oneParamExpressions.Select(x => x.Body)).Verifiable();

            var result = Create().Merge<Test>(expressions, out pe);

            Assert.AreEqual(ExpressionType.MemberInit, result.NodeType);
            var mie = result as MemberInitExpression;
            var ci = mie.NewExpression.Constructor;
            Assert.AreEqual(ci, typeof (Test).GetConstructors()[0]);
            Assert.AreEqual(4, mie.Bindings.Count);
            Assert.AreEqual(1, mie.Bindings.Count(b => b.Member == typeof (Test).GetProperty("A")));
            Assert.AreEqual(1, mie.Bindings.Count(b => b.Member == typeof (Test).GetProperty("B")));
            Assert.AreEqual(1, mie.Bindings.Count(b => b.Member == typeof (Test).GetProperty("C")));
            Assert.AreEqual(1, mie.Bindings.Count(b => b.Member == typeof (Test).GetProperty("D")));
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        public class WithoutParameterlessCtor
        {
            public WithoutParameterlessCtor(string a)
            {
                
            } 
        }

        public class Test
        {
            public string A { get; set; }

            public string B { get; set; }

            public string C { get; set; }

            public string D { get; set; }
        }
    }
}