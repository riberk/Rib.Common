namespace Rib.Common.Application
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.Application.Rest;
    using Rib.Common.Application.Wrappers;
    using Rib.Tests.Mock;

    [TestClass]
    public class NullArgumentTest
    {
        [TestMethod]
        public void AllNullArgumentThrow()
        {
            var res =
                    ArgumentsVerifier.Builder(typeof(IWrappersDisposer))
                            //Потому что инжектится ApplicationUserStore, а его невозможно создать моком
                                     .ToVerifier()
                                     .CheckAllVoidCtorsCreateObject()
                                     .CheckNullArgumentsOnConstructors()
                                     .CheckNullArgumentsOnConstructors<RestUpdateService<string, string>>()
                                     .Errors;
            if (res.Any())
            {
                Assert.Fail(string.Join("\r\n\r\n", res));
            }
        }
    }
}