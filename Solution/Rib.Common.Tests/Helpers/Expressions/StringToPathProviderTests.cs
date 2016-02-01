namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class StringToPathProviderTests
    {
        private Mock<ICacherFactory> _cacherFactory;
        private MockRepository _mockFactory;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _cacherFactory = _mockFactory.Create<ICacherFactory>();
        }

        private StringToPathProvider Create()
        {
            return new StringToPathProvider(_cacherFactory.Object);
        }

        [TestMethod]
        public void Constructor() => new StringToPathProvider(_cacherFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new StringToPathProvider(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetPropertiesWithNullArgument1()
        {
            Create().GetProperties(null, new string[0]).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetPropertiesWithNullArgument2()
        {
            Create().GetProperties(typeof (string), null).ToList();
        }

        [TestMethod]
        public void GetPropertiesEmpty()
        {
            var cacher = _mockFactory.Create<ICacher<PropertyInfo>>();
            _cacherFactory.Setup(x => x.Create<PropertyInfo>(typeof (StringToPathProvider).FullName, null)).Returns(cacher.Object).Verifiable();
            var res = Create().GetProperties(typeof (string), new string[0]).ToList();
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetPropertiesNullName()
        {
            var cacher = _mockFactory.Create<ICacher<PropertyInfo>>();
            _cacherFactory.Setup(x => x.Create<PropertyInfo>(typeof (StringToPathProvider).FullName, null)).Returns(cacher.Object).Verifiable();
            Create().GetProperties(typeof (string), new string[] {null}).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetPropertiesWhiteSpaceName()
        {
            var cacher = _mockFactory.Create<ICacher<PropertyInfo>>();
            _cacherFactory.Setup(x => x.Create<PropertyInfo>(typeof (StringToPathProvider).FullName, null)).Returns(cacher.Object).Verifiable();
            Create().GetProperties(typeof (string), new[] {"  "}).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetPropertiesEmptyName()
        {
            var cacher = _mockFactory.Create<ICacher<PropertyInfo>>();
            _cacherFactory.Setup(x => x.Create<PropertyInfo>(typeof (StringToPathProvider).FullName, null)).Returns(cacher.Object).Verifiable();
            Create().GetProperties(typeof (string), new[] {""}).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetPropertiesNullCache()
        {
            var propNames = new[] {"Parent"};
            var cacher = _mockFactory.Create<ICacher<PropertyInfo>>();
            _cacherFactory.Setup(x => x.Create<PropertyInfo>(typeof (StringToPathProvider).FullName, null)).Returns(cacher.Object).Verifiable();
            var key = $"{typeof (TestClass).FullName}|{propNames[0]}";
            cacher.Setup(x => x.GetOrAdd(key, It.IsAny<Func<string, PropertyInfo>>())).Returns((PropertyInfo) null).Verifiable();

            Create().GetProperties(typeof (TestClass), propNames).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetPropertiesWithoutProperty()
        {
            var propNames = new[] {"NotFoundInClass"};
            var cacher = _mockFactory.Create<ICacher<PropertyInfo>>();
            _cacherFactory.Setup(x => x.Create<PropertyInfo>(typeof (StringToPathProvider).FullName, null)).Returns(cacher.Object).Verifiable();
            cacher.Setup(x => x.GetOrAdd($"{typeof (TestClass).FullName}|{propNames[0]}", It.IsAny<Func<string, PropertyInfo>>()))
                .Returns((string s, Func<string, PropertyInfo> f) => f(s)).Verifiable();

            Create().GetProperties(typeof (TestClass), propNames).ToList();
        }

        [TestMethod]
        public void GetProperties()
        {
            var propNames = new[] {"Parent", "Name"};
            var cacher = _mockFactory.Create<ICacher<PropertyInfo>>();
            _cacherFactory.Setup(x => x.Create<PropertyInfo>(typeof (StringToPathProvider).FullName, null)).Returns(cacher.Object).Verifiable();
            cacher.Setup(x => x.GetOrAdd($"{typeof (TestClass).FullName}|{propNames[0]}", It.IsAny<Func<string, PropertyInfo>>()))
                .Returns(typeof (TestClass).GetProperty("Parent"))
                .Verifiable();
            cacher.Setup(x => x.GetOrAdd($"{typeof (TestClass).FullName}|{propNames[1]}", It.IsAny<Func<string, PropertyInfo>>()))
                .Returns((string s, Func<string, PropertyInfo> f) => f(s)).Verifiable();

            var properties = Create().GetProperties(typeof (TestClass), propNames).ToList();

            Assert.AreEqual(2, properties.Count);
            Assert.AreEqual(typeof (TestClass).GetProperty("Parent"), properties[0]);
            Assert.AreEqual(typeof (TestClass).GetProperty("Name"), properties[1]);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        public class TestClass
        {
            public TestClass Parent { get; set; }

            public string Name { get; set; }
        }
    }
}