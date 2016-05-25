namespace Rib.Common
{
    using System.Linq;
    using Rib.Common.Helpers.Cache;
    using Rib.Tests.Mock;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.Helpers.Expressions;

    [TestClass]
    public class NullArgumentTest
    {
        [TestMethod]
        public void AllNullArgumentThrow()
        {
            var res =
                ArgumentsVerifier.Builder(typeof (ICacher<>))
                    .Exclude<ParameterRebinderVisiter>()
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