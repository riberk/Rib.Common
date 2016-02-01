namespace Rib.Common.Helpers.Configuration.SettingsManagers
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Helpers.Configuration.ConfigurationItems;

    [TestClass]
    public class ConfigurationReaderTests
    {
        private Mock<IConfigurationItemResolver> _conItemsResolver;
        [NotNull] private MockRepository _factory;
        [NotNull] private ConfigurationManager _manager;
        private Mock<ISettingsReaderFactory> _settingsReaderFactory;
        private Mock<ISettingsWriterFactory> _settingsWriterFactory;

        [TestInitialize]
        public void Init()
        {
            _factory = new MockRepository(MockBehavior.Strict);
            _settingsReaderFactory = _factory.Create<ISettingsReaderFactory>();
            _settingsWriterFactory = _factory.Create<ISettingsWriterFactory>();
            _conItemsResolver = _factory.Create<IConfigurationItemResolver>();
            _manager = new ConfigurationManager(_settingsReaderFactory.Object, _settingsWriterFactory.Object, _conItemsResolver.Object);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorArgumentException1() => new ConfigurationManager(null, _settingsWriterFactory.Object, _conItemsResolver.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorArgumentException2() => new ConfigurationManager(_settingsReaderFactory.Object, null, _conItemsResolver.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorArgumentException3() => new ConfigurationManager(_settingsReaderFactory.Object, _settingsWriterFactory.Object, null);

        [TestMethod]
        public void Constructor() => new ConfigurationManager(_settingsReaderFactory.Object, _settingsWriterFactory.Object, _conItemsResolver.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConfigurationReaderArgumentExceptionTest() => _manager.Read(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConfigurationWriterArgumentExceptionTest() => _manager.Write(null, "123");

        [TestMethod]
        public void ReadTest()
        {
            const string expected = "321";
            const string name = "123";
            var ci = new ConfigurationItem(name);
            var reader = _factory.Create<ISettingsReader>();
            _settingsReaderFactory.Setup(x => x.Create(ci)).Returns(reader.Object).Verifiable();
            reader.Setup(x => x.Read(name)).Returns(expected).Verifiable();
            var res = _manager.Read(ci);
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void WriteTest()
        {
            const string name = "123";
            const string value = "sdf;lgmd";
            var ci = new ConfigurationItem(name);
            var writer = _factory.Create<ISettingsWriter>();
            _conItemsResolver.Setup(x => x.Resolve(name)).Returns(ci).Verifiable();
            _settingsWriterFactory.Setup(x => x.Create(ci)).Returns(writer.Object).Verifiable();
            writer.Setup(x => x.Write(name, value)).Verifiable();
            _manager.Write(name, value);
        }

        public void Clean()
        {
            _factory.VerifyAll();
        }

        private class Ci : ConfigurationItem
        {
            public Ci([NotNull] string name) : base(name)
            {
            }
        }
    }
}