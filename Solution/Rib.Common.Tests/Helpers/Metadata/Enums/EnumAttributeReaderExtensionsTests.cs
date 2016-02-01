namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

    [TestClass]
    public class EnumAttributeReaderExtensionsTests
    {
        private Mock<IEnumAttributeReader> _enumattributeReader;
        private MockRepository _mockFactory;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _enumattributeReader = _mockFactory.Create<IEnumAttributeReader>();
        }


        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void DescriptionNullArgument1() => EnumAttributeReaderExtensions.Description(null, DayOfWeek.Friday);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DescriptionNullArgument2() => EnumAttributeReaderExtensions.Description(null, (Enum)DayOfWeek.Friday);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void DescriptionSafeNullArgument1() => EnumAttributeReaderExtensions.DescriptionSafe(null, DayOfWeek.Friday);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DescriptionSafeNullArgument2() => EnumAttributeReaderExtensions.DescriptionSafe(null, (Enum)DayOfWeek.Friday);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DescriptionSafeNullArgument3() => EnumAttributeReaderExtensions.DescriptionSafe(_enumattributeReader.Object, null);



        [TestMethod]
        public void DescriptionWithNullAttrTest()
        {
            _enumattributeReader.Setup(x => x.Attribute<DescriptionAttribute, DayOfWeek>(DayOfWeek.Friday))
                .Returns((DescriptionAttribute) null)
                .Verifiable();

            Assert.IsNull(_enumattributeReader.Object.Description(DayOfWeek.Friday));
        }

        [TestMethod]
        public void DescriptionWithNullDescTest()
        {
            _enumattributeReader.Setup(x => x.Attribute<DescriptionAttribute, DayOfWeek>(DayOfWeek.Friday))
                .Returns(new DescriptionAttribute(null))
                .Verifiable();

            Assert.IsNull(_enumattributeReader.Object.Description(DayOfWeek.Friday));
        }

        [TestMethod]
        public void DescriptionTest()
        {
            var descriptionAttribute = new DescriptionAttribute("123");
            _enumattributeReader.Setup(x => x.Attribute<DescriptionAttribute, DayOfWeek>(DayOfWeek.Friday)).Returns(descriptionAttribute).Verifiable();

            Assert.AreEqual(descriptionAttribute.Description, _enumattributeReader.Object.Description(DayOfWeek.Friday));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void DescriptionSafeWithNullTest()
        {
            _enumattributeReader.Setup(x => x.AttributeSafe<DescriptionAttribute, DayOfWeek>(DayOfWeek.Friday))
                .Returns(new DescriptionAttribute(null))
                .Verifiable();

            _enumattributeReader.Object.DescriptionSafe(DayOfWeek.Friday);
        }

        [TestMethod]
        public void DescriptionSafeTest()
        {
            var descriptionAttribute = new DescriptionAttribute("123");
            _enumattributeReader.Setup(x => x.AttributeSafe<DescriptionAttribute, DayOfWeek>(DayOfWeek.Friday))
                .Returns(descriptionAttribute)
                .Verifiable();


            Assert.AreEqual(descriptionAttribute.Description, _enumattributeReader.Object.DescriptionSafe(DayOfWeek.Friday));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void DescriptionSafeBoxedNullTest()
        {
            var descriptionAttribute = new DescriptionAttribute(null);
            _enumattributeReader.Setup(x => x.AttributeSafe<DescriptionAttribute>(DayOfWeek.Friday))
                .Returns(descriptionAttribute)
                .Verifiable();
            _enumattributeReader.Object.DescriptionSafe((Enum) DayOfWeek.Friday);
        }

        [TestMethod]
        public void DescriptionSafeBoxedTest()
        {
            var descriptionAttribute = new DescriptionAttribute("123");
            _enumattributeReader.Setup(x => x.AttributeSafe<DescriptionAttribute>(DayOfWeek.Friday))
                .Returns(descriptionAttribute)
                .Verifiable();

            Assert.AreEqual(descriptionAttribute.Description, _enumattributeReader.Object.DescriptionSafe((Enum)DayOfWeek.Friday));
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}