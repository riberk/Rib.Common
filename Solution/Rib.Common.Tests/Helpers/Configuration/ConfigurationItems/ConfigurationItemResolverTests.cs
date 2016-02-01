namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ConfigurationItemResolverTests
    {
        private ConfigurationItemResolver _configurationItemResolver;
        private Mock<IConfigurationItemsReader> _confItemsReader;
        private MockRepository _mockFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _confItemsReader = _mockFactory.Create<IConfigurationItemsReader>();
            _configurationItemResolver = new ConfigurationItemResolver(_confItemsReader.Object);
        }


        [TestMethod]
        public void Constructor() => new ConfigurationItemResolver(_confItemsReader.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new ConfigurationItemResolver(null);

        [TestMethod]
        public void ResolveTest()
        {
            var configurationItems = new[] {new ConfigurationItem("1"), new ConfigurationItem("2")};
            _confItemsReader.Setup(x => x.ReadAll()).Returns(configurationItems).Verifiable();
            var actual = _configurationItemResolver.Resolve("1");
            Assert.AreEqual((object) configurationItems[0], actual);
        }

        [TestMethod]
        [ExpectedException(typeof (KeyNotFoundException))]
        public void ResolveWithoutValueTest()
        {
            var configurationItems = new[] {new ConfigurationItem("1"), new ConfigurationItem("2")};
            _confItemsReader.Setup(x => x.ReadAll()).Returns(configurationItems).Verifiable();
            var actual = _configurationItemResolver.Resolve("10");
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}