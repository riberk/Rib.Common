namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Collections.Generic;
    using Rib.Common.Helpers.Cache;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AttributesReaderTests
    {
        private Mock<ICacherFactory> _cacherFactory;
        private MockRepository _factory;

        [TestInitialize]
        public void Init()
        {
            _factory = new MockRepository(MockBehavior.Strict);
            _cacherFactory = _factory.Create<ICacherFactory>();
        }

        [TestMethod]
        public void Constructor() => new AttributesReader(_cacherFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument() => new AttributesReader(null);

        [TestMethod]
        public void ReadWithoutResultTest()
        {
            var reader = new AttributesReader(_cacherFactory.Object);
            var cacher = _factory.Create<ICacher<DescriptionAttribute>>();

            _cacherFactory.Setup(x => x.Create<DescriptionAttribute>(null, null)).Returns(cacher.Object).Verifiable();
            var key = $"{typeof (DescriptionAttribute).FullName}|{typeof (string).FullName}";
            cacher.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, DescriptionAttribute>>())).Returns(
                (string s, Func<string, DescriptionAttribute> f) =>
                {
                    Assert.IsNull(f(s));
                    return null;
                }).Verifiable();
            var res = reader.Read<DescriptionAttribute>(typeof (string));
            Assert.IsNull(res);
        }

        [TestMethod]
        public void ReadWithResultTest()
        {
            var reader = new AttributesReader(_cacherFactory.Object);
            var cacher = _factory.Create<ICacher<TestAttribute>>();

            _cacherFactory.Setup(x => x.Create<TestAttribute>(null, null)).Returns(cacher.Object).Verifiable();
            var key = $"{typeof (TestAttribute).FullName}|{typeof (OneAttribute).FullName}";
            cacher.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, TestAttribute>>())).Returns(
                (string s, Func<string, TestAttribute> f) =>
                {
                    var testAttribute = f(s);
                    Assert.IsNotNull(testAttribute);
                    Assert.AreEqual("One", testAttribute.V);
                    return testAttribute;
                }).Verifiable();
            var res = reader.Read<TestAttribute>(typeof (OneAttribute));
            Assert.IsNotNull(res);
            Assert.AreEqual("One", res.V);
        }

        [TestMethod]
        public void ReadWithResultOnPropertyTest()
        {
            var reader = new AttributesReader(_cacherFactory.Object);
            var cacher = _factory.Create<ICacher<TestAttribute>>();

            _cacherFactory.Setup(x => x.Create<TestAttribute>(null, null)).Returns(cacher.Object).Verifiable();
            var key = $"{typeof (TestAttribute).FullName}|{typeof (OneAttribute).FullName}|p";
            cacher.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, TestAttribute>>())).Returns(
                (string s, Func<string, TestAttribute> f) =>
                {
                    var testAttribute = f(s);
                    Assert.IsNotNull(testAttribute);
                    Assert.AreEqual("Property", testAttribute.V);
                    return testAttribute;
                }).Verifiable();
            var res = reader.Read<TestAttribute>(typeof (OneAttribute).GetProperty("p"));
            Assert.IsNotNull(res);
            Assert.AreEqual("Property", res.V);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadNullArgument() => new AttributesReader(_cacherFactory.Object).Read<DescriptionAttribute>(null);

        [TestMethod]
        public void ReadManyWithoutResultTest()
        {
            var reader = new AttributesReader(_cacherFactory.Object);
            var cacher = _factory.Create<ICacher<IReadOnlyCollection<TestAttribute>>>();

            _cacherFactory.Setup(x => x.Create<IReadOnlyCollection<TestAttribute>>(null, null)).Returns(cacher.Object).Verifiable();
            var key = $"{typeof (IReadOnlyCollection<TestAttribute>).FullName}|{typeof (string).FullName}";
            cacher.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, IReadOnlyCollection<TestAttribute>>>())).Returns(
                (string s, Func<string, IReadOnlyCollection<TestAttribute>> f) =>
                {
                    var testAttributes = f(s);
                    Assert.IsNotNull(testAttributes);
                    Assert.AreEqual(0, testAttributes.Count);
                    return testAttributes;
                }).Verifiable();
            var res = reader.ReadMany<TestAttribute>(typeof (string));
            Assert.IsNotNull(res);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void ReadManyWithResultTest()
        {
            var reader = new AttributesReader(_cacherFactory.Object);
            var cacher = _factory.Create<ICacher<IReadOnlyCollection<TestAttribute>>>();

            _cacherFactory.Setup(x => x.Create<IReadOnlyCollection<TestAttribute>>(null, null)).Returns(cacher.Object).Verifiable();
            var key = $"{typeof (IReadOnlyCollection<TestAttribute>).FullName}|{typeof (ManyAttributes).FullName}";
            cacher.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, IReadOnlyCollection<TestAttribute>>>())).Returns(
                (string s, Func<string, IReadOnlyCollection<TestAttribute>> f) =>
                {
                    var testAttributes = f(s);
                    Assert.IsNotNull(testAttributes);
                    Assert.AreEqual(2, testAttributes.Count);
                    return testAttributes;
                }).Verifiable();
            var res = reader.ReadMany<TestAttribute>(typeof (ManyAttributes));
            Assert.IsNotNull(res);
            Assert.AreEqual(2, res.Count);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadManyNullArgument() => new AttributesReader(_cacherFactory.Object).ReadMany<DescriptionAttribute>(null);

        [TestCleanup]
        public void Clean()
        {
            _factory.VerifyAll();
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        public class TestAttribute : Attribute
        {
            public readonly string V;

            public TestAttribute(string v)
            {
                V = v;
            }
        }

        [Test("One")]
        public class OneAttribute
        {
            [Test("Property")]
            public string p { get; set; }
        }

        [Test("Many1")]
        [Test("Many2")]
        public class ManyAttributes
        {
        }
    }
}