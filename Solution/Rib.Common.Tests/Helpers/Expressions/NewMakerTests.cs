namespace Rib.Common.Helpers.Expressions
{
    using System;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Models.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class NewMakerTests
    {
        private Mock<ICacherFactory> _cacherFactory;
        private MockRepository _mockFactory;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _cacherFactory = _mockFactory.Create<ICacherFactory>();
        }

        private NewMaker Create()
        {
            return new NewMaker(_cacherFactory.Object);
        }

        [TestMethod]
        public void Constructor() => new NewMaker(_cacherFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new NewMaker(null);

        [TestMethod]
        public void CreateWithCacheTest()
        {
            var cache = _mockFactory.Create<ICacher<Func<TestClass>>>();
            _cacherFactory.Setup(x => x.Create<Func<TestClass>>(typeof (NewMaker).FullName, null)).Returns(cache.Object).Verifiable();
            Func<TestClass> func = () => new TestClass();
            cache.Setup(x => x.GetOrAdd(typeof (TestClass).FullName, It.IsAny<Func<string, Func<TestClass>>>())).Returns(func).Verifiable();

            var value = Create().Create<TestClass>();

            Assert.AreEqual(func, value);
        }

        [TestMethod]
        public void CreateWithoutCacheTest()
        {
            var exp = new TestClass();
            var cache = _mockFactory.Create<ICacher<Func<TestClass>>>();
            _cacherFactory.Setup(x => x.Create<Func<TestClass>>(typeof (NewMaker).FullName, null)).Returns(cache.Object).Verifiable();
            cache.Setup(x => x.GetOrAdd(typeof (TestClass).FullName, It.IsAny<Func<string, Func<TestClass>>>()))
                .Returns((string s, Func<string, Func<TestClass>> creator) => creator(s)).Verifiable();

            var value = Create().Create<TestClass>();

            Assert.AreNotEqual(exp, value());
            Assert.AreNotEqual(value(), value());
        }

        [TestMethod]
        [ExpectedException(typeof (MetadataException), AllowDerivedTypes = false)]
        public void WithouParameterlessConstructor()
        {
            var cache = _mockFactory.Create<ICacher<Func<TestClassWithoutConstructor>>>();
            _cacherFactory.Setup(x => x.Create<Func<TestClassWithoutConstructor>>(typeof (NewMaker).FullName, null))
                .Returns(cache.Object)
                .Verifiable();
            cache.Setup(x => x.GetOrAdd(typeof (TestClassWithoutConstructor).FullName, It.IsAny<Func<string, Func<TestClassWithoutConstructor>>>()))
                .Returns((string s, Func<string, Func<TestClassWithoutConstructor>> creator) => creator(s)).Verifiable();

            Create().Create<TestClassWithoutConstructor>();
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        public class TestClass
        {
        }

        public class TestClassWithoutConstructor
        {
            public TestClassWithoutConstructor(string s)
            {
            }
        }
    }
}