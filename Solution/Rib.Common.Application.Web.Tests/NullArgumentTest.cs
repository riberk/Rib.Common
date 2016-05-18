namespace Rib.Common.Application.Web
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.Application.Web.Owin;
    using Rib.Common.Application.Web.Owin.Modules;
    using Rib.Common.Application.Web.WebApi.Filters;
    using Rib.Tests.Mock;

    [TestClass]
    public class NullArgumentTest
    {
        [TestMethod]
        public void AllNullArgumentThrow()
        {
            var res = ArgumentsVerifier.Builder(typeof(AppBuilderExtensions))
                                       .Exclude<InitializeWrappersMiddleware>()
                                       .Exclude<LoggerMiddleware>()
                                       .Exclude<ExceptionHandlerMiddleware>()
                                       .Exclude<SetCorrelationIdHeaderMiddleware>()
                                       .Exclude<SettingsIncludeErrorDetailsResolverImpl>()
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