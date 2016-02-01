namespace Rib.Common.Helpers.Configuration.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Helpers.Configuration.ConfigurationItems;
    using Rib.Common.Helpers.Configuration.SettingsManagers;
    using Rib.Common.Helpers.Metadata;
    using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

    [TestClass]
    public class ConfigurationServiceTests
    {
        private const string InnerConfig = "InnerConfig";
        private const string InnerConfig2 = "InnerConfig2";
        private const string InnerItem1 = "InnerItem1";
        private const string InnerItem2 = "InnerItem2";
        private const string InnerItem3 = "InnerItem3";
        [NotNull] private Mock<IAttributesReader> _attributesReader;
        [NotNull] private Mock<ICanEditItemChecker> _canEditChecker;
        [NotNull] private Mock<IConfigurationItemsHelper> _confHelper;
        [NotNull] private Mock<IConfigurationReader> _configurationReader;
        [NotNull] private MockRepository _mockFactory;
        [NotNull] private ConfigurationService _service;
        [NotNull] private Mock<IConfigurationTypeResolver> _typeResolver;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _attributesReader = _mockFactory.Create<IAttributesReader>();
            _configurationReader = _mockFactory.Create<IConfigurationReader>();
            _typeResolver = _mockFactory.Create<IConfigurationTypeResolver>();
            _confHelper = _mockFactory.Create<IConfigurationItemsHelper>();
            _canEditChecker = _mockFactory.Create<ICanEditItemChecker>();
            _service = new ConfigurationService(_attributesReader.Object, _configurationReader.Object, _typeResolver.Object, _confHelper.Object,
                                                _canEditChecker.Object);
        }


        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ReadWithoutDescriptionException()
        {
            _typeResolver.Setup(x => x.Resolve()).Returns(typeof (ConfigWithoutDescription)).Verifiable("Не вызван метод получения типа конфигурации");
            _attributesReader.Setup(x => x.Read<DescriptionAttribute>(typeof (ConfigWithoutDescription.InnerConfigClass)))
                             .Returns(new DescriptionAttribute(null))
                             .Verifiable();
            _confHelper.Setup(x => x.GroupedTypes(typeof (ConfigWithoutDescription)))
                       .Returns(new[] {typeof (ConfigWithoutDescription.InnerConfigClass)})
                       .Verifiable();

            _service.Read();
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ReadWithNullFieldException()
        {
            _typeResolver.Setup(x => x.Resolve()).Returns(typeof (ConfigWithNull)).Verifiable("Не вызван метод получения типа конфигурации");
            _attributesReader.Setup(x => x.Read<DescriptionAttribute>(typeof (ConfigWithNull.InnerConfigClass)))
                             .Returns(new DescriptionAttribute("123"))
                             .Verifiable();
            _confHelper.Setup(x => x.GroupedTypes(typeof (ConfigWithNull))).Returns(new[] {typeof (ConfigWithNull.InnerConfigClass)}).Verifiable();
            _confHelper.Setup(x => x.Items(typeof (ConfigWithNull.InnerConfigClass)))
                       .Returns(new[] {typeof (ConfigWithNull.InnerConfigClass).GetField("InnerItem1Field")})
                       .Verifiable();

            _service.Read();
        }

        [TestMethod]
        public void ReadTest()
        {
            _typeResolver.Setup(x => x.Resolve()).Returns(typeof (Config)).Verifiable("Не вызван метод получения типа конфигурации");
            _attributesReader.Setup(x => x.Read<DescriptionAttribute>(typeof (Config.InnerConfigClass)))
                             .Returns(new DescriptionAttribute(InnerConfig))
                             .Verifiable("Не получен атрибут описания класса");
            _attributesReader.Setup(x => x.Read<DescriptionAttribute>(typeof (Config.InnerConfigClass.InnerConfigClass2)))
                             .Returns(new DescriptionAttribute(InnerConfig2))
                             .Verifiable("Не получен атрибут описания класса");
            var bf = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
            _attributesReader.Setup(x => x.Read<DescriptionAttribute>(typeof (Config.InnerConfigClass).GetField("InnerItem1Field", bf)))
                             .Returns(new DescriptionAttribute(InnerItem1))
                             .Verifiable("Не получен атрибут описания поля");
            _attributesReader.Setup(x => x.Read<DescriptionAttribute>(typeof (Config.InnerConfigClass).GetField("InnerItem2Field", bf)))
                             .Returns(new DescriptionAttribute(InnerItem2))
                             .Verifiable("Не получен атрибут описания поля");
            _attributesReader.Setup(
                                    x => x.Read<DescriptionAttribute>(typeof (Config.InnerConfigClass.InnerConfigClass2).GetField("InnerItem3Field", bf)))
                             .Returns(new DescriptionAttribute(InnerItem3))
                             .Verifiable("Не получен атрибут описания поля");
            var value1 = "1";
            var value2 = "2";
            var value3 = "3";
            _configurationReader.Setup(x => x.Read(Config.InnerConfigClass.InnerItem1Field)).Returns(value1).Verifiable();
            _configurationReader.Setup(x => x.Read(Config.InnerConfigClass.InnerItem2Field)).Returns(value2).Verifiable();
            _configurationReader.Setup(x => x.Read(Config.InnerConfigClass.InnerConfigClass2.InnerItem3Field)).Returns(value3).Verifiable();
            _confHelper.Setup(x => x.GroupedTypes(typeof (Config))).Returns(new[] {typeof (Config.InnerConfigClass)}).Verifiable();
            _confHelper.Setup(x => x.GroupedTypes(typeof (Config.InnerConfigClass)))
                       .Returns(new[] {typeof (Config.InnerConfigClass.InnerConfigClass2)})
                       .Verifiable();
            _confHelper.Setup(x => x.GroupedTypes(typeof (Config.InnerConfigClass.InnerConfigClass2))).Returns(new Type[0]).Verifiable();
            var fieldInfo1 = typeof (Config.InnerConfigClass).GetField("InnerItem1Field");
            var fieldInfo2 = typeof (Config.InnerConfigClass).GetField("InnerItem2Field");
            var fieldInfo3 = typeof (Config.InnerConfigClass.InnerConfigClass2).GetField("InnerItem3Field");
            _confHelper.Setup(x => x.Items(typeof (Config.InnerConfigClass)))
                       .Returns(new[]
                       {
                           fieldInfo1,
                           fieldInfo2
                       }).Verifiable();
            _confHelper.Setup(x => x.Items(typeof (Config.InnerConfigClass.InnerConfigClass2)))
                       .Returns(new[]
                       {
                           fieldInfo3
                       }).Verifiable();
            _canEditChecker.Setup(x => x.CanEdit(fieldInfo1, Config.InnerConfigClass.InnerItem1Field)).Returns(true).Verifiable();
            _canEditChecker.Setup(x => x.CanEdit(fieldInfo2, Config.InnerConfigClass.InnerItem2Field)).Returns(false).Verifiable();
            _canEditChecker.Setup(x => x.CanEdit(fieldInfo3, Config.InnerConfigClass.InnerConfigClass2.InnerItem3Field)).Returns(true).Verifiable();

            var result = _service.Read();

            Assert.AreEqual(1, result.Count);
            var g1 = result.First();
            Assert.AreEqual(InnerConfig, g1.Description);
            Assert.IsNotNull(g1.Groups);
            Assert.AreEqual(1, g1.Groups.Count);
            Assert.AreEqual(2, g1.Items.Count);
            var i1 = g1.Items.First();
            var i2 = g1.Items.Last();

            Assert.AreEqual((object) Config.InnerConfigClass.InnerItem1Field.Name, i1.Name);
            Assert.AreEqual(InnerItem1, i1.Description);
            Assert.AreEqual(true, i1.CanChange);
            Assert.AreEqual(value1, i1.Value);

            Assert.AreEqual((object) Config.InnerConfigClass.InnerItem2Field.Name, i2.Name);
            Assert.AreEqual(InnerItem2, i2.Description);
            Assert.AreEqual(false, i2.CanChange);
            Assert.AreEqual(value2, i2.Value);

            var g2 = g1.Groups.First();
            Assert.AreEqual(InnerConfig2, g2.Description);
            Assert.IsNotNull(g2.Groups);
            Assert.AreEqual(0, g2.Groups.Count);
            Assert.AreEqual(1, g2.Items.Count);

            var i3 = g2.Items.First();

            Assert.AreEqual((object) Config.InnerConfigClass.InnerConfigClass2.InnerItem3Field.Name, i3.Name);
            Assert.AreEqual(InnerItem3, i3.Description);
            Assert.AreEqual(true, i3.CanChange);
            Assert.AreEqual(value3, i3.Value);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        private class ConfigWithoutDescription
        {
            [UsedImplicitly]
            public static class InnerConfigClass
            {
            }
        }

        private class ConfigWithNull
        {
            [UsedImplicitly]
            public static class InnerConfigClass
            {
                [UsedImplicitly] public static readonly ConfigurationItem InnerItem1Field;
            }
        }

        private class Config
        {
            [UsedImplicitly]
            public static class InnerConfigClass
            {
                [UsedImplicitly] public static readonly ConfigurationItem InnerItem1Field = new ConfigurationItem("123");

                [UsedImplicitly] public static readonly ConfigurationItem InnerItem2Field = new ConfigurationItem("3123");

                [UsedImplicitly]
                public static class InnerConfigClass2
                {
                    [UsedImplicitly] public static readonly ConfigurationItem InnerItem3Field = new ConfigurationItem("12sdgsdrg3");
                }
            }
        }
    }
}