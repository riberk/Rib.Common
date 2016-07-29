namespace Rib.Common.Helpers.Reflection
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypeExtensionsTests
    {
        public const string A = "A";
        internal const string B = "B";
        private const string C = "C";

        [TestMethod]
        public void ConstantsTest()
        {
            var constants = typeof(TypeExtensionsTests).Constants().ToList();
            Assert.AreEqual(1, constants.Count);
            Assert.AreEqual(typeof(TypeExtensionsTests).GetField("A"), constants[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstantsArgNullTest()
        {
            TypeExtensions.Constants(null);
        }
    }
}