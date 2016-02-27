namespace Rib.Common.Helpers.Metadata
{
    using System;
    using Rib.Common.Models.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AttributesReaderExtensionsTests
    {
        private Mock<IAttributesReader> _attributesReader;
        private MockRepository _factory;

        [TestInitialize]
        public void Init()
        {
            _factory = new MockRepository(MockBehavior.Strict);
            _attributesReader = _factory.Create<IAttributesReader>();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ArgumentNullException1() => _attributesReader.Object.ReadSafe<DescriptionAttribute>(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ArgumentNullException2() => AttributesReaderExtensions.ReadSafe<DescriptionAttribute>(null, typeof (string));

        [TestMethod]
        [ExpectedException(typeof (AttributeNotFoundException))]
        public void ReadSafeWithNullTest()
        {
            _attributesReader.Setup(x => x.Read<DescriptionAttribute>(typeof (string))).Returns((DescriptionAttribute) null).Verifiable();
            _attributesReader.Object.ReadSafe<DescriptionAttribute>(typeof (string));
        }

        [TestMethod]
        public void ReadSafeTest()
        {
            object exp = new DescriptionAttribute("aawd");
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute),typeof (string))).Returns(new[] {exp}).Verifiable();
            var actual = _attributesReader.Object.ReadSafe<DescriptionAttribute>(typeof (string));
            Assert.AreEqual(exp, actual);
        }

        [TestCleanup]
        public void Clean()
        {
            _factory.VerifyAll();
        }
    }
}