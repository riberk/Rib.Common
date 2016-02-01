namespace Rib.Common.Helpers.Encrypting
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ByteArraySplitterTests
    {
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void SplitArgumentNullTest()
        {
            var splitter = new ByteArraySplitter();
            splitter.Split(null, 120);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void SplitArgumentOutOfRange1Test()
        {
            var splitter = new ByteArraySplitter();
            splitter.Split(new byte[0], 0);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void SplitArgumentOutOfRange2Test()
        {
            var splitter = new ByteArraySplitter();
            splitter.Split(new byte[0], -1);
        }

        [TestMethod]
        public void Split1Test()
        {
            var splitter = new ByteArraySplitter();
            const int blockLength = 2;
            var res = splitter.Split(new byte[] {0, 1, 2, 3, 4, 5, 6, 7}, blockLength);
            Assert.IsNotNull(res);
            Assert.AreEqual(4, res.Length);
            Assert.IsTrue(res.All(x => x.Length == blockLength));
            Assert.AreEqual(0, res[0][0]);
            Assert.AreEqual(1, res[0][1]);
            Assert.AreEqual(2, res[1][0]);
            Assert.AreEqual(3, res[1][1]);
            Assert.AreEqual(4, res[2][0]);
            Assert.AreEqual(5, res[2][1]);
            Assert.AreEqual(6, res[3][0]);
            Assert.AreEqual(7, res[3][1]);
        }

        [TestMethod]
        public void Split2Test()
        {
            var splitter = new ByteArraySplitter();
            const int blockLength = 3;
            var res = splitter.Split(new byte[] {0, 1, 2, 3, 4}, blockLength);
            Assert.IsNotNull(res);
            Assert.AreEqual(2, res.Length);
            Assert.AreEqual(3, res[0].Length);
            Assert.AreEqual(2, res[1].Length);
            Assert.AreEqual(0, res[0][0]);
            Assert.AreEqual(1, res[0][1]);
            Assert.AreEqual(2, res[0][2]);
            Assert.AreEqual(3, res[1][0]);
            Assert.AreEqual(4, res[1][1]);
        }
    }
}