using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rib.Common.Models.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rib.Common.Models.Binding
{
    [TestClass]
    public class BindingScopeTests
    {
        [TestMethod]
        public void BindingScopeTest()
        {
            new BindingScope("123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BindingScopeNullArgTest()
        {
            new BindingScope(null);
        }

        [TestMethod]
        public void EqualsTest()
        {
            var s1 = new BindingScope("123");
            var s2 = new BindingScope("123");
            var s3 = new BindingScope("321");

            Assert.IsTrue(s1 == s2);
            Assert.IsTrue(s2 == s1);
            Assert.IsFalse(s1 == s3);

            Assert.IsFalse(s1 != s2);
            Assert.IsFalse(s2 != s1);
            Assert.IsTrue(s1 != s3);

            Assert.IsTrue(s1.Equals(s2));
            Assert.IsTrue(s1.Equals((object)s2));

            Assert.IsTrue(s1.Equals(s1));
            Assert.IsTrue(s1.Equals((object)s1));

            Assert.IsFalse(s1.Equals(null));
            Assert.IsFalse(s1.Equals((object)null));
            Assert.IsFalse(s1.Equals((object)s3));
            Assert.IsFalse(s1.Equals("123"));
        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            var s1 = new BindingScope("123");
            var s2 = new BindingScope("123");
            var s3 = new BindingScope("321");

            Assert.AreEqual(s1.GetHashCode(), s2.GetHashCode());
            Assert.AreNotEqual(s1.GetHashCode(), s3.GetHashCode());
            Assert.AreNotEqual(s2.GetHashCode(), s3.GetHashCode());
        }
    }
}