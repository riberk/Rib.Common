namespace Rib.Common.Metadata.ClientEnums
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ClientEnumPermanentStoreTests
    {
        [TestMethod]
        public void AddTest()
        {
            var data = new ClientEnumPermanentStore()
                .Add("1", typeof(E1))
                .Add("2", typeof(E2))
                .Data;
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(typeof(E1), data["1"]);
            Assert.AreEqual(typeof(E2), data["2"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNullTest() => new ClientEnumPermanentStore().Add(null, typeof(E1));

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNull1Test() => new ClientEnumPermanentStore().Add("1", null);

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNotEnumTest() => new ClientEnumPermanentStore().Add("1", typeof(string));
        
        public enum E1
        {
            V1
        }

        public enum E2
        {
            V2
        }
    }
}