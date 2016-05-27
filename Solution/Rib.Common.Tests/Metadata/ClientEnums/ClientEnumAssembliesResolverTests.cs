using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rib.Common.Metadata.ClientEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rib.Common.Metadata.ClientEnums
{
    using Moq;

    [TestClass]
    public class ClientEnumAssembliesResolverTests
    {
        [TestMethod]
        public void ResolveTest()
        {
            var store = new Mock<IClientEnumAssemblyStore>(MockBehavior.Strict);
            var resolver = new ClientEnumAssembliesResolver(store.Object);
            store.Setup(x => x.Assemblies).Returns(new[] {GetType().Assembly}).Verifiable();
            var asms = resolver.Resolve().ToList();
            Assert.AreEqual(1, asms.Count);
            Assert.AreEqual(GetType().Assembly, asms[0]);
        }
    }
}