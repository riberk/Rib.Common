namespace Rib.Common.Ninject
{
    using global::Ninject;
    using global::Ninject.Modules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.Models.Exceptions;
    using Rib.Common.Models.Metadata;
    using TestForNinjectBindingsErrorNamed.Ninject;

    [TestClass]
    public class RibNinjectModuleTests
    {
        [TestMethod]
        public void LoadTest()
        {
            var kernel = new StandardKernel(new TestModule());
            var i1 = kernel.TryGet<I1>();
            var i2 = kernel.TryGet<I2>("adawd");
            var i3 = kernel.TryGet<I2>("adawdd");

            Assert.IsNotNull(i1);
            Assert.IsNotNull(i2);
            Assert.IsNotNull(i3);

            Assert.AreEqual(typeof(C1), i1.GetType());
            Assert.AreEqual(typeof(C2), i2.GetType());
            Assert.AreEqual(typeof(C3), i3.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(MetadataException))]
        public void ManyUnnamedAttributeTest()
        {
            new StandardKernel(new TestNinjectModule());
        }

        public class TestModule : RibNinjectModule
        {
        }

        public class InternalNinjectModule : NinjectModule
        {
            /// <summary>
            ///     Loads the module into the kernel.
            /// </summary>
            public override void Load()
            {
                Bind<I1>().To<C1>();
            }
        }

        public interface I1
        {
        }

        [BindTo(typeof (C2), "adawd")]
        [BindTo(typeof (C3), "adawdd")]
        public interface I2
        {
        }

        public class C1 : I1
        {
        }

        public class C2 : I2
        {
        }

        public class C3 : I2
        {
        }
    }
}