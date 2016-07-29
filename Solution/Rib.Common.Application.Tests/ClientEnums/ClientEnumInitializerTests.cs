namespace Rib.Common.Application.ClientEnums
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Metadata.ClientEnums;

    [TestClass]
    public class ClientEnumInitializerTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            var store = new Mock<IClientEnumAssemblyStore>(MockBehavior.Loose);
            var initializer = new ClientEnumInitializer(store.Object);
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var t = GetType();

            foreach (var domainAssembly in domainAssemblies)
            {
                store.Setup(x => x.Add(domainAssembly)).Verifiable();
            }
            store.Setup(x => x.Add(t.Assembly)).Verifiable();

            store.Setup(x => x.Close());
            initializer.Initialize(t);

            store.VerifyAll();
        }
    }
}