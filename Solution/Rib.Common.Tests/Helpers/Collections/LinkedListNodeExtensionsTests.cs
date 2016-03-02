namespace Rib.Common.Helpers.Collections
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LinkedListNodeExtensionsTests
    {
        [TestMethod]
        public void NextOrFirstTest()
        {
            const string first = "1";
            const string second = "2";
            const string last = "3";
            var ll = new LinkedList<string>(new[] {first, second, last});

            var firstNode = ll.First;

            Assert.AreEqual(first, firstNode.Value);
            Assert.AreEqual(second, firstNode.NextOrFirst().Value);
            Assert.AreEqual(last, firstNode.NextOrFirst().NextOrFirst().Value);
            Assert.AreEqual(first, firstNode.NextOrFirst().NextOrFirst().NextOrFirst().Value);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void NextOrFirstNullArgTest() => LinkedListNodeExtensions.NextOrFirst<string>(null);

        [TestMethod]
        public void PreviousOrLastTest()
        {
            const string first = "1";
            const string second = "2";
            const string last = "3";

            var ll = new LinkedList<string>(new[] {first, second, last});

            var lastNode = ll.Last;

            Assert.AreEqual(last, lastNode.Value);
            Assert.AreEqual(second, lastNode.PreviousOrLast().Value);
            Assert.AreEqual(first, lastNode.PreviousOrLast().PreviousOrLast().Value);
            Assert.AreEqual(last, lastNode.PreviousOrLast().PreviousOrLast().PreviousOrLast().Value);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void PreviousOrLastNullArgTest() => LinkedListNodeExtensions.PreviousOrLast<string>(null);
    }
}