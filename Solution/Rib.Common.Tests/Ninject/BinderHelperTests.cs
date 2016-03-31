namespace Rib.Common.Ninject
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BinderHelperTests
    {
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadFromAssemblyTypesNullArgTest()
        {
            BinderHelper.ReadFromAssemblyTypes(null);
        }
    }
}