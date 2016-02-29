namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Linq;
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
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), typeof (string))).Returns(new  object[] { (DescriptionAttribute)null }).Verifiable();
            _attributesReader.Object.ReadSafe<DescriptionAttribute>(typeof (string));
        }

        [TestMethod]
        public void ReadGenericTest()
        {
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), typeof(string))).Returns(new object[] { (DescriptionAttribute)null }).Verifiable();
            Assert.IsNull(_attributesReader.Object.Read<DescriptionAttribute>(typeof(string)));
        }

        [TestMethod]
        public void ReadTest()
        {
            var descriptionAttribute = new DescriptionAttribute("fad");
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), typeof(string))).Returns(new object[] { descriptionAttribute  }).Verifiable();
            Assert.AreEqual(descriptionAttribute, _attributesReader.Object.Read<DescriptionAttribute>(typeof(string)));
        }

        [TestMethod]
        public void ReadManyTest()
        {
            var descriptionAttribute1 = new DescriptionAttribute("fad");
            var descriptionAttribute2 = new DescriptionAttribute("fadawdawd");
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), typeof(string))).Returns(new object[] { descriptionAttribute1, descriptionAttribute2 }).Verifiable();
            var attrs = _attributesReader.Object.ReadMany<DescriptionAttribute>(typeof (string)).ToList();
            Assert.AreEqual(2, attrs.Count);
            Assert.AreEqual(descriptionAttribute1, attrs[0]);
            Assert.AreEqual(descriptionAttribute2, attrs[1]);
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