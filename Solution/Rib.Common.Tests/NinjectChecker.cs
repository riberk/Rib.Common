namespace Rib.Common
{
    using System.Linq;
    using global::Ninject;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Helpers.Configuration;
    using Rib.Common.Helpers.Configuration.Services;
    using Rib.Common.Helpers.Configuration.SettingsManagers;
    using Rib.Common.Helpers.CorrelationId;
    using Rib.Common.Helpers.DateAndTime;
    using Rib.Common.Helpers.Encrypting.Symmetric;
    using Rib.Common.Helpers.Tmp;
    using Rib.Common.Ninject;
    using Rib.Tests.Ninject;

    [TestClass]
    public class NinjectCheckerTest
    {
        [TestMethod]
        public void Test()
        {
            var errors = new NinjectChecker(typeof (IResolver).Assembly, TestKernelCreator.Create(() => new StandardKernel(new RibCommonModule())))
                .Exclude<ISymmetricAlgorithmTypeReader>()
                .Exclude<ICorrelationIdStore>()
                .Exclude<ISettingsManager>()
                .Exclude<ISettingsReader>()
                .Exclude(typeof(ISettingsReader<>))
                .Exclude<ISettingsWriter>()
                .Exclude<ISettingsWriterFactory>()
                .Exclude<ISettingsReaderFactory>()
                .Exclude<IConfigurationTypeResolver>()
                .Exclude<ICanEditItemChecker>()
                .Exclude<ITmpFolder>()
                .Exclude(typeof(IConfigurationEntryCreator<>))
                .Exclude(typeof(ICacher<>))
                .Exclude<IFirstDayOfWeekResolver>()
                .Exclude<IBindInfo>()
                .CheckAllInterfacesCanCreate()
                .Errors;
            if (errors.Any())
            {
                Assert.Fail(string.Join("\r\n\r\n", errors.Select(x => x.Message)));
            }
        }
    }
}