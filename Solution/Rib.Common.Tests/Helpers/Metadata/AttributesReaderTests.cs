namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Cache;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AttributesReaderTests
    {
        private Mock<ICacherFactory> _cacherFactory;
        private MockRepository _factory;
        private Mock<AttributesReader.IAttributesReaderKeyFactory> _keyCreator;

        [TestInitialize]
        public void Init()
        {
            _factory = new MockRepository(MockBehavior.Strict);
            _cacherFactory = _factory.Create<ICacherFactory>();
            _keyCreator = _factory.Create<AttributesReader.IAttributesReaderKeyFactory>();
        }

        [TestMethod]
        public void Constructor() => new AttributesReader(_cacherFactory.Object, _keyCreator.Object);

        [NotNull]
        private AttributesReader Create() => new AttributesReader(_cacherFactory.Object, _keyCreator.Object);

        [TestMethod]
        public void ReadWithoutResultTest()
        {
            var reader = Create();
            var cacher = _factory.Create<ICacher<IReadOnlyCollection<object>>>();
            const string key = "sdfgsdfg";
            _keyCreator.Setup(x => x.Create(typeof (DescriptionAttribute), typeof (string))).Returns(key).Verifiable();
            _cacherFactory.Setup(x => x.Create<IReadOnlyCollection<object>>(null, null)).Returns(cacher.Object).Verifiable();
            cacher.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, IReadOnlyCollection<object>>>())).Returns(
                (string s, Func<string, IReadOnlyCollection<object>> f) => f(s)).Verifiable();

            var res = reader.ReadMany(typeof(DescriptionAttribute), typeof (string));

            Assert.IsNotNull(res);
            Assert.IsFalse(res.Any());
        }

        [TestMethod]
        public void ReadWithResultTest()
        {
            var reader = Create();
            var cacher = _factory.Create<ICacher<IReadOnlyCollection<object>>>();

            _cacherFactory.Setup(x => x.Create<IReadOnlyCollection<object>>(null, null)).Returns(cacher.Object).Verifiable();
            const string key = "sdfgsdfg";
            _keyCreator.Setup(x => x.Create(typeof (TestAttribute), typeof (OneAttribute))).Returns(key).Verifiable();
            cacher.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, IReadOnlyCollection<object>>>())).Returns(
                (string s, Func<string, IReadOnlyCollection<object>> f) =>
                {
                    var testAttributes = f(s);
                    return testAttributes;
                }).Verifiable();
            var res = reader.ReadMany(typeof(TestAttribute), typeof (OneAttribute));
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Any());
            Assert.AreEqual(1, res.Count);
            var ta = res.Single() as TestAttribute;
            Assert.IsNotNull(ta);
            Assert.AreEqual("One", ta.V);
        }

        
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadNullArgument1() => Create().ReadMany(null, typeof(string));

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadNullArgument2() => Create().ReadMany(typeof(DescriptionAttribute), null);

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ReadNullResult()
        {
            var reader = Create();
            var cacher = _factory.Create<ICacher<IReadOnlyCollection<object>>>();

            _cacherFactory.Setup(x => x.Create<IReadOnlyCollection<object>>(null, null)).Returns(cacher.Object).Verifiable();
            const string key = "sdfgsdfg";
            _keyCreator.Setup(x => x.Create(typeof (TestAttribute), typeof (OneAttribute))).Returns(key).Verifiable();
            cacher.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, IReadOnlyCollection<object>>>())).Returns(() => null).Verifiable();
            reader.ReadMany(typeof(TestAttribute), typeof(OneAttribute));
        }

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