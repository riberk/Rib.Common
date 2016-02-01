namespace Rib.Common.Helpers.Configuration
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SettingsWriterExtensionsTests
    {
        private MockRepository _mockFactory;
        private Mock<ISettingsWriter> _settingsWriter;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _settingsWriter = _mockFactory.Create<ISettingsWriter>();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void WriteNullArgument1() => SettingsWriterExtensions.Write(null, "1", 1);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void WriteNullArgument2() => _settingsWriter.Object.Write(null, 1);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void WriteNullArgument3() => SettingsWriterExtensions.Write(null, "1", (int?) 1);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void WriteNullArgument4() => _settingsWriter.Object.Write(null, (int?) 1);

        [TestMethod]
        public void WriteTest1()
        {
            _settingsWriter.Setup(x => x.Write("123", "1")).Verifiable();
            _settingsWriter.Object.Write("123", 1);
        }

        [TestMethod]
        public void WriteTest2()
        {
            _settingsWriter.Setup(x => x.Write("123", "1")).Verifiable();
            _settingsWriter.Object.Write("123", (int?) 1);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}