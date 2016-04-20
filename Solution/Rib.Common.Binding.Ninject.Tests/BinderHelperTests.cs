namespace Rib.Common.Ninject
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.DependencyInjection;

    [TestClass]
    public class BinderHelperTests
    {
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadFromAssemblyTypesNullArgTest()
        {
            new BinderHelper().ReadFromTypes(null);
        }
    }
}