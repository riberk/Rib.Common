namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Helpers.Configuration.Services;

    [TestClass]
    public class ConfigurationItemsReaderTests
    {
        private Mock<IConfigurationItemsHelper> _helper;
        private MockRepository _mockFactory;
        private ConfigurationItemsReader _reader;
        private Mock<IConfigurationTypeResolver> _resolver;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _helper = _mockFactory.Create<IConfigurationItemsHelper>();
            _resolver = _mockFactory.Create<IConfigurationTypeResolver>();
            _reader = new ConfigurationItemsReader(_resolver.Object, _helper.Object);
        }

        [TestMethod]
        public void Constructor() => new ConfigurationItemsReader(_resolver.Object, _helper.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new ConfigurationItemsReader(null, _helper.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument2() => new ConfigurationItemsReader(_resolver.Object, null);

        [TestMethod]
        public void ReadAllTest()
        {
            var root = typeof (string);
            var rootNested = new[] {typeof (int), typeof (long)};
            var nestedFor0 = new[] {typeof (uint)};

            var intFi1 = _mockFactory.Create<FieldInfo>();
            var intFi2 = _mockFactory.Create<FieldInfo>();
            var longFi1 = _mockFactory.Create<FieldInfo>();
            var uintFi1 = _mockFactory.Create<FieldInfo>();

            var int1Ci = new ConfigurationItem("int1");
            var int2Ci = new ConfigurationItem("int2");
            var long1Ci = new ConfigurationItem("long1");
            var uint1Ci = new ConfigurationItem("uint1");

            _resolver.Setup(x => x.Resolve()).Returns(root).Verifiable();
            _helper.Setup(x => x.GroupedTypes(root)).Returns(rootNested).Verifiable();
            _helper.Setup(x => x.GroupedTypes(rootNested[0])).Returns(nestedFor0).Verifiable();
            _helper.Setup(x => x.GroupedTypes(rootNested[1])).Returns(new Type[0]).Verifiable();
            _helper.Setup(x => x.GroupedTypes(nestedFor0[0])).Returns(new Type[0]).Verifiable();
            _helper.Setup(x => x.Items(rootNested[0])).Returns(new[] {intFi1.Object, intFi2.Object}).Verifiable();
            _helper.Setup(x => x.Items(rootNested[1])).Returns(new[] {longFi1.Object}).Verifiable();
            _helper.Setup(x => x.Items(nestedFor0[0])).Returns(new[] {uintFi1.Object}).Verifiable();
            intFi1.Setup(x => x.GetValue(null)).Returns(int1Ci).Verifiable();
            intFi2.Setup(x => x.GetValue(null)).Returns(int2Ci).Verifiable();
            longFi1.Setup(x => x.GetValue(null)).Returns(long1Ci).Verifiable();
            uintFi1.Setup(x => x.GetValue(null)).Returns(uint1Ci).Verifiable();

            var result = _reader.ReadAll().ToArray();

            var resultHash = new HashSet<ConfigurationItem>(result);
            Assert.AreEqual(4, result.Length);
            Assert.IsTrue(resultHash.Contains(int1Ci));
            Assert.IsTrue(resultHash.Contains(int2Ci));
            Assert.IsTrue(resultHash.Contains(uint1Ci));
            Assert.IsTrue(resultHash.Contains(long1Ci));
        }

        [TestMethod]
        public void ReadAllWithFieldsTest()
        {
            var root = typeof(string);
            var rootNested = new[] { typeof(int), typeof(long) };
            var nestedFor0 = new[] { typeof(uint) };

            var intFi1 = _mockFactory.Create<FieldInfo>();
            var intFi2 = _mockFactory.Create<FieldInfo>();
            var longFi1 = _mockFactory.Create<FieldInfo>();
            var uintFi1 = _mockFactory.Create<FieldInfo>();

            intFi1.Name = "intFi1";
            intFi2.Name = "intFi2";
            longFi1.Name = "longFi1";
            uintFi1.Name = "uintFi1";

            var int1Ci = new ConfigurationItem("int1");
            var int2Ci = new ConfigurationItem("int2");
            var long1Ci = new ConfigurationItem("long1");
            var uint1Ci = new ConfigurationItem("uint1");

            _resolver.Setup(x => x.Resolve()).Returns(root).Verifiable();
            _helper.Setup(x => x.GroupedTypes(root)).Returns(rootNested).Verifiable();
            _helper.Setup(x => x.GroupedTypes(rootNested[0])).Returns(nestedFor0).Verifiable();
            _helper.Setup(x => x.GroupedTypes(rootNested[1])).Returns(new Type[0]).Verifiable();
            _helper.Setup(x => x.GroupedTypes(nestedFor0[0])).Returns(new Type[0]).Verifiable();
            _helper.Setup(x => x.Items(rootNested[0])).Returns(new[] { intFi1.Object, intFi2.Object }).Verifiable();
            _helper.Setup(x => x.Items(rootNested[1])).Returns(new[] { longFi1.Object }).Verifiable();
            _helper.Setup(x => x.Items(nestedFor0[0])).Returns(new[] { uintFi1.Object }).Verifiable();

            intFi1.Setup(x => x.GetValue(null)).Returns(int1Ci).Verifiable();
            intFi2.Setup(x => x.GetValue(null)).Returns(int2Ci).Verifiable();
            longFi1.Setup(x => x.GetValue(null)).Returns(long1Ci).Verifiable();
            uintFi1.Setup(x => x.GetValue(null)).Returns(uint1Ci).Verifiable();

            intFi1.Setup(x => x.GetHashCode()).Returns(1).Verifiable();
            intFi2.Setup(x => x.GetHashCode()).Returns(2).Verifiable();
            longFi1.Setup(x => x.GetHashCode()).Returns(3).Verifiable();
            uintFi1.Setup(x => x.GetHashCode()).Returns(4).Verifiable();

            intFi1.Setup(x => x.ToString()).Returns("1").Verifiable();
            intFi2.Setup(x => x.ToString()).Returns("2").Verifiable();
            longFi1.Setup(x => x.ToString()).Returns("3").Verifiable();
            uintFi1.Setup(x => x.ToString()).Returns("4").Verifiable();

            intFi1.Setup(x => x.Equals(intFi1.Object)).Returns(true).Verifiable();
            intFi2.Setup(x => x.Equals(intFi2.Object)).Returns(true).Verifiable();
            longFi1.Setup(x => x.Equals(longFi1.Object)).Returns(true).Verifiable();
            uintFi1.Setup(x => x.Equals(uintFi1.Object)).Returns(true).Verifiable();

            var result = _reader.ReadAllWithFields();

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(int1Ci, result[intFi1.Object]);
            Assert.AreEqual(int2Ci, result[intFi2.Object]);
            Assert.AreEqual(uint1Ci, result[uintFi1.Object]);
            Assert.AreEqual(long1Ci, result[longFi1.Object]);
        }

        [TestCleanup]
        public void Clear()
        {
            _mockFactory.VerifyAll();
        }
    }
}