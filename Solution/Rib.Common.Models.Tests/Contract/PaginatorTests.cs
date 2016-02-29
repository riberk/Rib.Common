namespace Rib.Common.Models.Contract
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PaginatorTests
    {
        [TestMethod]
        public void PaginatorSkipTest1()
        {
            var paginator = new Paginator(2, 10);

            Assert.AreEqual(2, paginator.PageNumber);
            Assert.AreEqual(10, paginator.PageSize);
            Assert.AreEqual(10, paginator.Skip);
        }

        [TestMethod]
        public void PaginatorSkipTest2()
        {
            var paginator = new Paginator(1, 10);

            Assert.AreEqual(1, paginator.PageNumber);
            Assert.AreEqual(10, paginator.PageSize);
            Assert.AreEqual(0, paginator.Skip);
        }

        [TestMethod]
        public void PaginatorSkipTest3()
        {
            Assert.AreEqual(1, Paginator.Full.PageNumber);
            Assert.AreEqual(int.MaxValue, Paginator.Full.PageSize);
            Assert.AreEqual(0, Paginator.Full.Skip);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void PageNumberArgumentOutOfRange1() => new Paginator(0, 10);

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void PageNumberArgumentOutOfRange2() => new Paginator(1, 0);

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void PageNumberArgumentOutOfRange3() => new Paginator(-1, 10);

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void PageNumberArgumentOutOfRange4() => new Paginator(1, -1);
    }
}