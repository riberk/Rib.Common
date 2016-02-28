namespace Rib.Common.Models
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.Models.Contract;
    using Rib.Common.Models.Exceptions;
    using Rib.Tests.Mock;

    [TestClass]
    public class NullArgumentTest
    {
        [TestMethod]
        public void AllNullArgumentThrow()
        {
            var res =
                ArgumentsVerifier.Builder(typeof (PagedResponse<>))
                    .Exclude<AttributeException>()
                    .Exclude<AttributeNotFoundException>()
                    .ToVerifier()
                    .CheckAllVoidCtorsCreateObject()
                    .CheckNullArgumentsOnConstructors()
                    .Errors;
            if (res.Any())
            {
                Assert.Fail(string.Join("\r\n\r\n", res));
            }
        }
    }
}