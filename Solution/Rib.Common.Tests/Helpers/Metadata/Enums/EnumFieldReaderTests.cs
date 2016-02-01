namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class EnumFieldReaderTests
    {
        private Mock<ICacher<FieldInfo>> _cacher;
        private Mock<ICacherFactory> _cacherFactory;
        private DayOfWeek _e;
        private string _eName;
        private FieldInfo _f;

        private MockRepository _mockFactory;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _cacherFactory = _mockFactory.Create<ICacherFactory>();
            _cacher = _mockFactory.Create<ICacher<FieldInfo>>();
            _e = DayOfWeek.Friday;
            _eName = _e.ToString();
            _f = typeof (DayOfWeek).GetField(_eName);
        }

        private EnumFieldReader Create()
        {
            return new EnumFieldReader(_cacherFactory.Object);
        }

        [TestMethod]
        public void Constructor() => new EnumFieldReader(_cacherFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new EnumFieldReader(null);

        [TestMethod]
        public void FieldGenericTest()
        {
            _cacherFactory.Setup(x => x.Create<FieldInfo>($"{typeof (EnumFieldReader)}", null)).Returns(_cacher.Object).Verifiable();
            _cacher.Setup(x => x.GetOrAdd($"{_e.GetType().FullName}|{_eName}", It.IsAny<Func<string, FieldInfo>>())).Returns(_f).Verifiable();

            var actual = Create().Field(_e);

            Assert.AreEqual(_f, actual);
        }

        [TestMethod]
        public void FieldGenericWithoutCacheTest()
        {
            var e = DataAccessMethod.Random;
            var ff = typeof (DataAccessMethod).GetField(e.ToString());
            _cacherFactory.Setup(x => x.Create<FieldInfo>($"{typeof (EnumFieldReader)}", null)).Returns(_cacher.Object).Verifiable();
            _cacher.Setup(x => x.GetOrAdd($"{e.GetType().FullName}|{e}", It.IsAny<Func<string, FieldInfo>>()))
                .Returns((string s, Func<string, FieldInfo> f) => f(s))
                .Verifiable();

            var actual = Create().Field(e);

            Assert.AreEqual(ff, actual);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void FieldGenericWithNullTest()
        {
            var e = DataAccessMethod.Random;
            var f = typeof (DataAccessMethod).GetField(e.ToString());
            _cacherFactory.Setup(x => x.Create<FieldInfo>($"{typeof (EnumFieldReader)}", null)).Returns(_cacher.Object).Verifiable();
            _cacher.Setup(x => x.GetOrAdd($"{e.GetType().FullName}|{e}", It.IsAny<Func<string, FieldInfo>>()))
                .Returns((FieldInfo) null)
                .Verifiable();

            Create().Field(e);
        }

        [TestMethod]
        public void FieldTest()
        {
            _cacherFactory.Setup(x => x.Create<FieldInfo>($"{typeof (EnumFieldReader)}", null)).Returns(_cacher.Object).Verifiable();
            _cacher.Setup(x => x.GetOrAdd($"{_e.GetType().FullName}|{_eName}", It.IsAny<Func<string, FieldInfo>>())).Returns(_f).Verifiable();

            var actual = Create().Field((Enum) _e);

            Assert.AreEqual(_f, actual);
        }

        [TestMethod]
        public void FieldWithoutCacheTest()
        {
            var e = DataAccessMethod.Random;
            var ff = typeof (DataAccessMethod).GetField(e.ToString());
            _cacherFactory.Setup(x => x.Create<FieldInfo>($"{typeof (EnumFieldReader)}", null)).Returns(_cacher.Object).Verifiable();
            _cacher.Setup(x => x.GetOrAdd($"{e.GetType().FullName}|{e}", It.IsAny<Func<string, FieldInfo>>()))
                .Returns((string s, Func<string, FieldInfo> f) => f(s))
                .Verifiable();

            var actual = Create().Field((Enum) e);

            Assert.AreEqual(ff, actual);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void FieldWithNullTest()
        {
            var e = DataAccessMethod.Random;
            var f = typeof (DataAccessMethod).GetField(e.ToString());
            _cacherFactory.Setup(x => x.Create<FieldInfo>($"{typeof (EnumFieldReader)}", null)).Returns(_cacher.Object).Verifiable();
            _cacher.Setup(x => x.GetOrAdd($"{e.GetType().FullName}|{e}", It.IsAny<Func<string, FieldInfo>>()))
                .Returns((FieldInfo) null)
                .Verifiable();

            Create().Field((Enum) e);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void FieldArgNullTest()
        {
            Create().Field(null);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}