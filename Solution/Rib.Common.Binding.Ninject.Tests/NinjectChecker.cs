namespace Rib.Common.Binding.Ninject
{
    using System.Linq;
    using global::Ninject;
    using global::Ninject.Modules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.DependencyInjection;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Helpers.Configuration;
    using Rib.Common.Helpers.Configuration.Services;
    using Rib.Common.Helpers.Configuration.SettingsManagers;
    using Rib.Common.Helpers.CorrelationId;
    using Rib.Common.Helpers.DateAndTime;
    using Rib.Common.Helpers.Encrypting.Symmetric;
    using Rib.Common.Helpers.Tmp;
    using Rib.Common.Models.Binding;
    using Rib.Tests.Ninject;

    [TestClass]
    public class NinjectCheckerTest
    {
        public class RibCommonModule : NinjectModule
        {
            public override void Load()
            {
                var bHelper = new BinderHelper();
                var binder = new NinjectBinder(Bind);
                binder.Bind(bHelper.ReadFromTypes(typeof(IResolver).Assembly.GetTypes(), BindingScope.Singleton));
                binder.Bind(bHelper.ReadFromTypes(typeof(NinjectResolver).Assembly.GetTypes(), BindingScope.Singleton));
            }
        }

        [TestMethod]
        public void Test()
        {
            var kernel = TestKernelCreator.Create(() => new StandardKernel(new RibCommonModule()));
            var errors = new NinjectChecker(typeof (IResolver).Assembly, kernel)
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
                .Exclude<IBinder>()
                .CheckAllInterfacesCanCreate()
                .Errors;

            if (errors.Any())
            {
                Assert.Fail(string.Join("\r\n\r\n", errors.Select(x => x.Message)));
            }
        }
    }
}