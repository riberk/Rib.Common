namespace Rib.Common.Helpers.CallContext
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CallContextResolverTests
    {
        [TestMethod]
        public void TestCurrent()
        {
            try
            {
                var data = new TestClass();
                CallContext.LogicalSetData("TestKey", data);
                var ccr = new CallContextResolverT();
                Assert.AreEqual(data, ccr.Current);
            }
            finally
            {
                CallContext.LogicalSetData("TestKey", null);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestCurrentException()
        {
            try
            {
                CallContext.LogicalSetData("TestKey", "awdawd");
                var ccr = new CallContextResolverT();
                var c = ccr.Current;
            }
            finally
            {
                CallContext.LogicalSetData("TestKey", null);
            }
        }

        [TestMethod]
        public void TestCurrentNull()
        {
            try
            {
                var ccr = new CallContextResolverT();
                Assert.IsNull(ccr.Current);
            }
            finally
            {
                CallContext.LogicalSetData("TestKey", null);
            }
        }


        public class CallContextResolverT : CallContextResolver<TestClass>
        {
            public override string ContextKey { get; } = "TestKey";
        }

        public class TestClass
        {
        }
    }
}