namespace Rib.Common.Application
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ninject;
    using Rib.Common.Application.Wrappers;
    using Rib.Common.Binding.Ninject;
    using Rib.Common.DependencyInjection;
    using Rib.Common.Helpers.Metadata.Enums;
    using Rib.Tests.Ninject;

    [TestClass]
    public class EnumsTest
    {
        [TestMethod]
        public void CheckAll()
        {
            var kernel = new StandardKernel();
            var bh = new BinderHelper();
            var b = new NinjectBinder(kernel.Bind);
            b.Bind(bh.ReadFromTypes(typeof(BinderHelper).Assembly.GetTypes() ));

            var errors = new EnumsChecker(new[]
            {
                typeof (IWrappersDisposer).Assembly,
            }, kernel.Get<IEnumFieldReader>()).CheckDescription().CheckIdentityValues().Errors;

            if (errors.Any())
            {
                Assert.Fail(string.Join("\r\n", errors));
            }
        }
    }
}