namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class PropertyStringResolverTests
    {
        private MockRepository _mockFactory;
        private Mock<IStringToPathProvider> _stringToPathProvider;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _stringToPathProvider = _mockFactory.Create<IStringToPathProvider>();
        }

        private PropertyStringResolver Create()
        {
            return new PropertyStringResolver(_stringToPathProvider.Object);
        }

        [TestMethod]
        public void Constructor() => new PropertyStringResolver(_stringToPathProvider.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new PropertyStringResolver(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetPropertiesArgNull1()
        {
            Create().GetProperties(null, "123");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetPropertiesArgNull2()
        {
            Create().GetProperties(typeof (string), null);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetPropertiesInvalidString()
        {
            Create().GetProperties(typeof (string), "123");
        }

        [TestMethod]
        public void GetPropertiesTest()
        {
            var currentType = typeof (string);
            var prop = typeof (string).GetProperty("Length");
            _stringToPathProvider.Setup(x => x.GetProperties(currentType, It.IsAny<IEnumerable<string>>()))
                .Returns((Type t, IEnumerable<string> ss) =>
                {
                    Assert.AreEqual("Length", ss.Single());
                    return new [] { prop };
                });

            var actual = Create().GetProperties(currentType, "arg.Length").ToList();

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(prop, actual[0]);
        }


        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}