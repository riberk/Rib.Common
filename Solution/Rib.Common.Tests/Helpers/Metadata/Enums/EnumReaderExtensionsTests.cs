namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Linq;
    using Rib.Common.Models.Metadata;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class EnumReaderExtensionsTests
    {
        public enum TestEnum
        {
            One,
            Two
        }

        [NotNull] private Mock<IEnumReader> _enumReader;
        [NotNull] private MockRepository _mockFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _enumReader = _mockFactory.Create<IEnumReader>();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadManyNullArgument1() => EnumReaderExtensions.ReadMany<TestEnum>(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadManyNullArgument2() => EnumReaderExtensions.ReadMany(null, typeof (TestEnum));

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadManyNullArgument3() => _enumReader.Object.ReadMany(null);

        [TestMethod]
        public void ReadManyTest()
        {
            var enumModelOne = new EnumModel<TestEnum>(TestEnum.One, "123", 1);
            var enumModelTwo = new EnumModel<TestEnum>(TestEnum.Two, "222", 2);
            _enumReader.Setup(x => x.Read(TestEnum.One)).Returns(enumModelOne).Verifiable();
            _enumReader.Setup(x => x.Read(TestEnum.Two)).Returns(enumModelTwo).Verifiable();

            var result = _enumReader.Object.ReadMany<TestEnum>().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(enumModelOne, result[0]);
            Assert.AreEqual(enumModelTwo, result[1]);
        }

        [TestMethod]
        public void ReadManyBoxedTest()
        {
            var enumModelOne = new EnumModel<TestEnum>(TestEnum.One, "123", 1);
            var enumModelTwo = new EnumModel<TestEnum>(TestEnum.Two, "222", 2);
            _enumReader.Setup(x => x.Read((Enum) TestEnum.One)).Returns(enumModelOne).Verifiable();
            _enumReader.Setup(x => x.Read((Enum) TestEnum.Two)).Returns(enumModelTwo).Verifiable();

            var result = _enumReader.Object.ReadMany(typeof (TestEnum)).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(enumModelOne, result[0]);
            Assert.AreEqual(enumModelTwo, result[1]);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}