﻿namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class PropertyHelperTests
    {
        private Mock<ICacherFactory> _cacherFactory;
        private PropertyInfo _intProp;
        private MockRepository _mockFactory;
        private PropertyInfo _stringProp;


        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _cacherFactory = _mockFactory.Create<ICacherFactory>();
            _stringProp = typeof (TestClass).GetProperty("S");
            _intProp = typeof (TestClass).GetProperty("I");
        }

        private PropertyHelper Create()
        {
            return new PropertyHelper(_cacherFactory.Object);
        }

        [TestMethod]
        public void Constructor() => new PropertyHelper(_cacherFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetWithNullArgument1() => Create().Get(null, new {});

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetWithNullArgument2() => Create().Get(_stringProp, null);


        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void SetWithNullArgument1() => Create().Set(null, new {}, new {});


        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void SetWithNullArgument2() => Create().Set(_stringProp, null, new {});


        [TestMethod]
        public void GetTest()
        {
            var cacher = _mockFactory.Create<ICacher<Delegate>>();
            _cacherFactory.Setup(x => x.Create<Delegate>($"{typeof (PropertyHelper).FullName}::Get", null)).Returns(cacher.Object).Verifiable();
            const string expected = "1234321";
            cacher.Setup(x => x.GetOrAdd($"{_stringProp.DeclaringType.FullName}|{_stringProp.Name}", It.IsAny<Func<string, Delegate>>()))
                .Returns((string s, Func<string, Delegate> f) => f(s))
                .Verifiable();

            var res = Create().Get(_stringProp, new TestClass {S = expected});

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void SetTest()
        {
            var cacher = _mockFactory.Create<ICacher<Delegate>>();
            _cacherFactory.Setup(x => x.Create<Delegate>($"{typeof(PropertyHelper).FullName}::Set", null)).Returns(cacher.Object).Verifiable();
            const string expected = "1234321";
            cacher.Setup(x => x.GetOrAdd($"{_stringProp.DeclaringType.FullName}|{_stringProp.Name}", It.IsAny<Func<string, Delegate>>()))
                .Returns((string s, Func<string, Delegate> f) => f(s))
                .Verifiable();

            var testClass = new TestClass();
            Create().Set(_stringProp, testClass, expected);
            Assert.AreEqual(expected, testClass.S);
        }


        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        public class TestClass
        {
            public string S { get; set; }

            public int I { get; set; }
        }
    }
}