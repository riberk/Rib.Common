namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    //TODO REflected type and declaring type
    [TestClass]
    public class AttributesReaderKeyFactoryTests
    {
        private AttributesReader.AttributesReaderKeyFactory Create()
        {
            return new AttributesReader.AttributesReaderKeyFactory();
        }

        [TestMethod]
        public void CreateByAssemblyTest()
        {
            var provider = GetType().Assembly;
            var attrType = typeof (DescriptionAttribute);
            var str = Create().Create(attrType, provider);
            Assert.AreEqual($"{attrType.FullName}|{provider.FullName}", str);
        }

        [TestMethod]
        public void CreateByModuleTest()
        {
            var provider = GetType().Module;
            var attrType = typeof (DescriptionAttribute);
            var str = Create().Create(attrType, provider);
            Assert.AreEqual($"{attrType.FullName}|{provider.FullyQualifiedName}", str);
        }

        [TestMethod]
        public void CreateByTypeTest()
        {
            var provider = GetType();
            var attrType = typeof (DescriptionAttribute);
            var str = Create().Create(attrType, provider);
            Assert.AreEqual($"{attrType.FullName}|{provider.FullName}", str);
        }

        [TestMethod]
        public void CreateByMemberInfoTest()
        {
            var provider = GetType().GetMethods().First();
            var attrType = typeof (DescriptionAttribute);
            var str = Create().Create(attrType, provider);
            Assert.AreEqual($"{attrType.FullName}|{provider.DeclaringType.FullName}|{provider.Name}", str);
        }

        [TestMethod]
        public void CreateByParameterInfoTest()
        {
            var provider = GetType().GetMethod("MethodWithParameter").GetParameters()[0];
            var attrType = typeof (DescriptionAttribute);
            var str = Create().Create(attrType, provider);
            Assert.AreEqual($"{attrType.FullName}|{provider.Member.DeclaringType}|{provider.Member.Name}|{provider.Name}", str);
        }

        [TestMethod]
        public void CreateByOther()
        {
            var provider = new ToStringer();
            var attrType = typeof (DescriptionAttribute);
            var str = Create().Create(attrType, provider);
            Assert.AreEqual($"{attrType.FullName}|{ToStringer.Name}", str);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void CreateNullArg1Test() => Create().Create(null, typeof (string));

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void CreateNullArg2Test() => Create().Create(typeof (DescriptionAttribute), null);

        public void MethodWithParameter(int a)
        {
        }

        private class ToStringer : ICustomAttributeProvider
        {
            public const string Name = "dfflkgndfhkln";

            public override string ToString()
            {
                return Name;
            }

            public object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public object[] GetCustomAttributes(bool inherit)
            {
                throw new NotImplementedException();
            }

            public bool IsDefined(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }
        }
    }
}