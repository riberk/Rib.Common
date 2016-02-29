namespace Rib.Common.Models.Configuration
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConfigurationItemGroupTableModelTests
    {
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConfigurationItemGroupTableModelNullArgumentTest() => new ConfigurationItemGroupTableModel(null);
    }
}