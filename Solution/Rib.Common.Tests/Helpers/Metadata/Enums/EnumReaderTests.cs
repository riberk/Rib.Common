namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Models.Metadata;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
    [TestClass]
    public class EnumReaderTests
    {
        public enum TestEnum
        {
            One = 100,
            Two = 200
        }

        private Mock<IEnumAttributeReader> _attrsReader;
        private Mock<ICacherFactory> _cacherFactory;
        private MockRepository _mockFactory;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _attrsReader = _mockFactory.Create<IEnumAttributeReader>();
            _cacherFactory = _mockFactory.Create<ICacherFactory>();
        }

        private EnumReader Create()
        {
            return new EnumReader(_attrsReader.Object, _cacherFactory.Object);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadBoxedNullArgTest() => Create().Read(null);

        [TestMethod]
        public void ReadBoxedWithCacheTest()
        {
            var cache = _mockFactory.Create<ICacher<EnumModel>>();
            var o = (Enum) TestEnum.One;
            var enumModel = new EnumModel<TestEnum>(TestEnum.One, "123", 1);
            cache.Setup(x => x.GetOrAdd($"{o.GetType()}|{o}", It.IsAny<Func<string, EnumModel>>())).Returns(enumModel).Verifiable();
            _cacherFactory.Setup(x => x.Create<EnumModel>(null, null)).Returns(cache.Object).Verifiable();

            var res = Create().Read(o);
            Assert.AreEqual(enumModel, res);
        }

        [TestMethod]
        public void ReadBoxedWithoutCacheTest()
        {
            var cache = _mockFactory.Create<ICacher<EnumModel>>();
            const string desc = "desc";
            var o = (Enum) TestEnum.One;
            cache.Setup(x => x.GetOrAdd($"{o.GetType()}|{o}", It.IsAny<Func<string, EnumModel>>()))
                 .Returns((string s, Func<string, EnumModel> f) => f(s))
                 .Verifiable();
            _cacherFactory.Setup(x => x.Create<EnumModel>(null, null)).Returns(cache.Object).Verifiable();
            _attrsReader.Setup(x => x.Attribute<DescriptionAttribute>(o)).Returns(new DescriptionAttribute(desc)).Verifiable();
            var res = Create().Read(o) as EnumModel<TestEnum>;

            Assert.AreEqual(TestEnum.One, res.EnumValue);
            Assert.AreEqual((int) TestEnum.One, res.Value);
            Assert.AreEqual(desc, res.Description);
            Assert.AreEqual(o.ToString(), res.Name);
        }

        [TestMethod]
        public void ReadGenericTest()
        {
            var cache = _mockFactory.Create<ICacher<EnumModel>>();
            var o = (Enum)TestEnum.One;
            var enumModel = new EnumModel<TestEnum>(TestEnum.One, "123", 1);
            cache.Setup(x => x.GetOrAdd($"{o.GetType()}|{o}", It.IsAny<Func<string, EnumModel>>())).Returns(enumModel).Verifiable();
            _cacherFactory.Setup(x => x.Create<EnumModel>(null, null)).Returns(cache.Object).Verifiable();

            var res = Create().Read(TestEnum.One);
            Assert.AreEqual(enumModel, res);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadGenericWithCastExceptionTest()
        {
            var cache = _mockFactory.Create<ICacher<EnumModel>>();
            var o = (Enum)TestEnum.One;
            var enumModel = new EnumModel<DayOfWeek>(DayOfWeek.Friday, "123", 1);
            cache.Setup(x => x.GetOrAdd($"{o.GetType()}|{o}", It.IsAny<Func<string, EnumModel>>())).Returns(enumModel).Verifiable();
            _cacherFactory.Setup(x => x.Create<EnumModel>(null, null)).Returns(cache.Object).Verifiable();

            Create().Read(TestEnum.One);
        }

        [TestMethod]
        public void Constructor() => new EnumReader(_attrsReader.Object, _cacherFactory.Object);

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}