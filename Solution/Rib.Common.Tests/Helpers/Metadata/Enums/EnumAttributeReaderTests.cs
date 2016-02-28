namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Reflection;
    using Rib.Common.Models.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

    [TestClass]
    public class EnumAttributeReaderTests
    {
        public enum TestEnum
        {
            Value
        }

        private Mock<IAttributesReader> _attributesReader;
        private DescriptionAttribute _da;
        private FieldInfo _field;
        private Mock<IEnumFieldReader> _fieldReader;
        private MockRepository _mockFactory;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _attributesReader = _mockFactory.Create<IAttributesReader>();
            _fieldReader = _mockFactory.Create<IEnumFieldReader>();
            _field = typeof (TestEnum).GetField(TestEnum.Value.ToString());
            _da = new DescriptionAttribute();
        }

        private EnumAttributeReader Create()
        {
            return new EnumAttributeReader(_attributesReader.Object, _fieldReader.Object);
        }

        [TestMethod]
        public void Constructor() => new EnumAttributeReader(_attributesReader.Object, _fieldReader.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new EnumAttributeReader(null, _fieldReader.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument2() => new EnumAttributeReader(_attributesReader.Object, null);

        [TestMethod]
        public void AttributeGenericTest()
        {
            _fieldReader.Setup(x => x.Field(TestEnum.Value)).Returns(_field).Verifiable();
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), _field)).Returns(new object []{ _da }).Verifiable();

            var actual = Create().Attribute<DescriptionAttribute, TestEnum>(TestEnum.Value);

            Assert.AreEqual(_da, actual);
        }

        [TestMethod]
        public void AttributeGenericWithNullTest()
        {
            _fieldReader.Setup(x => x.Field(TestEnum.Value)).Returns(_field).Verifiable();
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), _field)).Returns(new object[] { (DescriptionAttribute)null }).Verifiable();

            var actual = Create().Attribute<DescriptionAttribute, TestEnum>(TestEnum.Value);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void AttributeTest()
        {
            var withDescription = (Enum) TestEnum.Value;
            _fieldReader.Setup(x => x.Field(withDescription)).Returns(_field).Verifiable();
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), _field)).Returns(new object[] { _da }).Verifiable();

            var actual = Create().Attribute<DescriptionAttribute>(withDescription);

            Assert.AreEqual(_da, actual);
        }

        [TestMethod]
        public void AttributeWithNullTest()
        {
            var withoutDescription = (Enum) TestEnum.Value;
            _fieldReader.Setup(x => x.Field(withoutDescription)).Returns(_field).Verifiable();
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), _field)).Returns(new object[] { (DescriptionAttribute)null }).Verifiable();

            var actual = Create().Attribute<DescriptionAttribute>(withoutDescription);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void AttributeSafeGenericTest()
        {
            _fieldReader.Setup(x => x.Field(TestEnum.Value)).Returns(_field).Verifiable();
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), _field)).Returns(new object[] { _da }).Verifiable();

            var actual = Create().AttributeSafe<DescriptionAttribute, TestEnum>(TestEnum.Value);

            Assert.AreEqual(_da, actual);
        }

        [TestMethod]
        [ExpectedException(typeof (AttributeNotFoundException))]
        public void AttributeSafeGenericWithNullTest()
        {
            _fieldReader.Setup(x => x.Field(TestEnum.Value)).Returns(_field).Verifiable();
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), _field)).Returns(new object[] { (DescriptionAttribute)null }).Verifiable();

            Create().AttributeSafe<DescriptionAttribute, TestEnum>(TestEnum.Value);
        }

        [TestMethod]
        public void AttributeSafeTest()
        {
            var val = (Enum) TestEnum.Value;
            _fieldReader.Setup(x => x.Field(val)).Returns(_field).Verifiable();
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), _field)).Returns(new object[] { _da }).Verifiable();

            var actual = Create().AttributeSafe<DescriptionAttribute>(val);

            Assert.AreEqual(_da, actual);
        }

        [TestMethod]
        [ExpectedException(typeof (AttributeNotFoundException))]
        public void AttributeSafeWithNullTest()
        {
            var val = (Enum) TestEnum.Value;
            _fieldReader.Setup(x => x.Field(val)).Returns(_field).Verifiable();
            _attributesReader.Setup(x => x.ReadMany(typeof(DescriptionAttribute), _field)).Returns(new object[] { (DescriptionAttribute)null }).Verifiable();

            Create().AttributeSafe<DescriptionAttribute>(val);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void AttributeSafeArgumentNull()
        {
            Create().AttributeSafe<DescriptionAttribute>(null);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}