namespace Rib.Common.Models.Binding
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BindInfoTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BindInfoArgNullTest1() => new BindInfo(null, new Type[0], typeof(string));

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BindInfoArgNullTest2() => new BindInfo(new BindingScope("123"), null, typeof(string));

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BindInfoArgNullTest3() => new BindInfo(new BindingScope("123"), new Type[0], null);

        [TestMethod]
        public void BindInfoTest1()
        {
            var types = new [] {typeof(int)};
            var bindingScope = new BindingScope("123");
            var type = typeof(string);

            var bi = new BindInfo(bindingScope, types, type);

            Assert.AreEqual(bindingScope, bi.Scope);
            Assert.AreEqual(types, bi.From);
            Assert.AreEqual(type, bi.To);
            Assert.IsNull(bi.Name);
        }

        [TestMethod]
        public void BindInfoTest2()
        {
            var types = new[] { typeof(int) };
            var bindingScope = new BindingScope("123");
            var type = typeof(string);
            const string name = "123222";

            var bi = new BindInfo(bindingScope, types, type, name);

            Assert.AreEqual(bindingScope, bi.Scope);
            Assert.AreEqual(types, bi.From);
            Assert.AreEqual(type, bi.To);
            Assert.AreEqual(name, bi.Name);
        }
    }
}