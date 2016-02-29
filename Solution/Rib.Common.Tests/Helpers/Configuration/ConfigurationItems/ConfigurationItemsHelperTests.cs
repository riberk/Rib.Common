namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConfigurationItemsHelperTests
    {
        private ConfigurationItemsHelper _helper;

        [TestInitialize]
        public void Init()
        {
            _helper = new ConfigurationItemsHelper();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GroupedTypesNullArgument1() => _helper.GroupedTypes(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GroupedTypesNullArgument2() => _helper.Items(null);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ItemNullArgument1() => _helper.Item(null, "fasef");

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ItemNullArgument2() => _helper.Item(typeof(string), null);

        [TestMethod]
        public void GroupedTypesTest()
        {
            var grouped = _helper.GroupedTypes(typeof (Root)).ToArray();

            var groupedHash = new HashSet<Type>(grouped);
            Assert.AreEqual(2, grouped.Length);
            Assert.IsTrue(groupedHash.Contains(typeof (Root.NestedStatic1)));
            Assert.IsTrue(groupedHash.Contains(typeof (Root.NestedStatic2)));
        }

        [TestMethod]
        public void ItemsTest()
        {
            var items = _helper.Items(typeof (Root.NestedStatic1)).ToArray();

            var hashItems = new HashSet<FieldInfo>(items);
            Assert.AreEqual(2, items.Length);
            Assert.AreEqual(2, hashItems.Count);
            Assert.IsTrue(hashItems.Contains(typeof (Root.NestedStatic1).GetField("Item1")));
            Assert.IsTrue(hashItems.Contains(typeof (Root.NestedStatic1).GetField("Item2")));
        }

        [TestMethod]
        public void ItemTest()
        {
            var item = _helper.Item(typeof (Root.NestedStatic1), "Item1");

            Assert.AreEqual(typeof(Root.NestedStatic1).GetField("Item1"), item);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ItemExceptionTest()
        {
            _helper.Item(typeof(Root.NestedStatic1), "Item4");
        }

        [TestCleanup]
        public void Clean()
        {
        }

        public class Root
        {
            public static class NestedStatic1
            {
                public static readonly ConfigurationItem Item1 = new ConfigurationItem("123");
                public static readonly ConfigurationItem Item2 = new ConfigurationItem("123");
                public static ConfigurationItem Item4 = new ConfigurationItem("123");

                public static class NestedStatic3
                {
                    public static ConfigurationItem Item3 = new ConfigurationItem("123");
                }
            }

            public static class NestedStatic2
            {
            }

            public class NestedNotStatic
            {
            }
        }
    }
}