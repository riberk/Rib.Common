namespace Rib.Common.Models.Encrypting
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SymmetricKeyTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgument1Test() => new SymmetricKey(null, new byte[0]);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgument2Test() => new SymmetricKey(new byte[0], null);
    }
}