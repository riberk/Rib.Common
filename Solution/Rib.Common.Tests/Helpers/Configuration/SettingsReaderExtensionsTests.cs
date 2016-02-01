namespace Rib.Common.Helpers.Configuration
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SettingsReaderExtensionsTests
    {
        private MockRepository _mockFactory;
        private Mock<ISettingsReader> _settingsReader;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _settingsReader = _mockFactory.Create<ISettingsReader>();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadSafeNullArgument1() => SettingsReaderExtensions.ReadSafe(null, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadSafeNullArgument2() => _settingsReader.Object.ReadSafe(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadIntNullArgument1() => SettingsReaderExtensions.ReadInt(null, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadIntNullArgument2() => _settingsReader.Object.ReadInt(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadSafeIntNullArgument1() => SettingsReaderExtensions.ReadSafeInt(null, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadSafeIntNullArgument2() => _settingsReader.Object.ReadSafeInt(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadBoolNullArgument1() => SettingsReaderExtensions.ReadBool(null, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadBoolNullArgument2() => _settingsReader.Object.ReadBool(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadSafeBoolNullArgument1() => SettingsReaderExtensions.ReadSafeBool(null, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadSafeBoolNullArgument2() => _settingsReader.Object.ReadSafeBool(null);

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ReadSafeWithNullResult()
        {
            _settingsReader.Setup(x => x.Read("123")).Returns((string) null).Verifiable();
            _settingsReader.Object.ReadSafe("123");
        }

        [TestMethod]
        public void ReadSafeTest()
        {
            const string exp = "321";
            _settingsReader.Setup(x => x.Read("123")).Returns(exp).Verifiable();
            var actual = _settingsReader.Object.ReadSafe("123");
            Assert.AreEqual(exp, actual);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidCastException))]
        public void ReadIntWithNotIntResult()
        {
            const string exp = "3safase21";
            _settingsReader.Setup(x => x.Read("123")).Returns(exp).Verifiable();
            _settingsReader.Object.ReadInt("123");
        }

        [TestMethod]
        public void ReadIntTest()
        {
            const string exp = "321";
            _settingsReader.Setup(x => x.Read("123")).Returns(exp).Verifiable();
            var actual = _settingsReader.Object.ReadInt("123");
            Assert.AreEqual(int.Parse(exp), actual);
        }

        [TestMethod]
        public void ReadIntWithNullTest()
        {
            _settingsReader.Setup(x => x.Read("123")).Returns((string) null).Verifiable();
            var actual = _settingsReader.Object.ReadInt("123");
            Assert.IsNull(actual);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ReadSafeIntNullTest()
        {
            _settingsReader.Setup(x => x.Read("123")).Returns((string) null).Verifiable();
            _settingsReader.Object.ReadSafeInt("123");
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidCastException))]
        public void ReadSafeIntNotIntTest()
        {
            const string exp = "3safase21";
            _settingsReader.Setup(x => x.Read("123")).Returns(exp).Verifiable();
            _settingsReader.Object.ReadSafeInt("123");
        }

        [TestMethod]
        public void ReadSafeIntTest()
        {
            const string exp = "321";
            _settingsReader.Setup(x => x.Read("123")).Returns(exp).Verifiable();
            var actual = _settingsReader.Object.ReadSafeInt("123");
            Assert.AreEqual(int.Parse(exp), actual);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidCastException))]
        public void ReadBoolWithNotBoolResult()
        {
            const string exp = "3safase21";
            _settingsReader.Setup(x => x.Read("123")).Returns(exp).Verifiable();
            _settingsReader.Object.ReadBool("123");
        }

        [TestMethod]
        public void ReadBoolTest()
        {
            const string exp = "true";
            _settingsReader.Setup(x => x.Read("123")).Returns(exp).Verifiable();
            var actual = _settingsReader.Object.ReadBool("123");
            Assert.AreEqual(bool.Parse(exp), actual);
        }

        [TestMethod]
        public void ReadBoolWithNullTest()
        {
            _settingsReader.Setup(x => x.Read("123")).Returns((string) null).Verifiable();
            var actual = _settingsReader.Object.ReadBool("123");
            Assert.IsNull(actual);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ReadSafeBoolNullTest()
        {
            _settingsReader.Setup(x => x.Read("123")).Returns((string) null).Verifiable();
            _settingsReader.Object.ReadSafeBool("123");
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidCastException))]
        public void ReadSafeBoolNotBoolTest()
        {
            const string exp = "3safase21";
            _settingsReader.Setup(x => x.Read("123")).Returns(exp).Verifiable();
            _settingsReader.Object.ReadSafeBool("123");
        }

        [TestMethod]
        public void ReadSafeBoolTest()
        {
            const string exp = "false";
            _settingsReader.Setup(x => x.Read("123")).Returns(exp).Verifiable();
            var actual = _settingsReader.Object.ReadSafeBool("123");
            Assert.AreEqual(bool.Parse(exp), actual);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}