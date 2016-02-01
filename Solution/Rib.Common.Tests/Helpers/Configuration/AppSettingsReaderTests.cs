namespace Rib.Common.Helpers.Configuration
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AppSettingsReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ReadArgumentNullTest() => new AppSettingsManager().Read(null);

        [TestMethod]
        public void ReadTest() => Assert.AreEqual("321", new AppSettingsManager().Read("123"));

        [TestMethod]
        [ExpectedException(typeof (NotSupportedException))]
        public void Writeest() => new AppSettingsManager().Write("1", "2");
    }
}