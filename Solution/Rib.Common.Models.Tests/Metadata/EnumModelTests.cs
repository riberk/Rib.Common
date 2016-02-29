namespace Rib.Common.Models.Metadata
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumModelTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnumModelNullArgTest()
        {
            new EnumModel<int>(1, "awdwa", null);
        }
    }
}