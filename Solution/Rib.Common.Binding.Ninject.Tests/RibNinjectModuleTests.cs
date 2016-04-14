namespace Rib.Common.Binding.Ninject
{
    using global::Ninject;
    using global::Ninject.Modules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.Models.Binding;
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
            var i23 = kernel.TryGet<I2>("adawdd");
            var i3 = kernel.TryGet<I3>();
            var i4 = kernel.TryGet<I4>();
            var i5 = kernel.TryGet<I5>();
            var i6 = kernel.TryGet<I6>();
            var i7 = kernel.TryGet<I7>();


            Assert.IsNotNull(i1);
            Assert.IsNotNull(i2);
            Assert.IsNotNull(i23);
            Assert.IsNotNull(i3);
            Assert.IsNotNull(i4);
            Assert.IsNotNull(i5);
            Assert.IsNotNull(i6);
            Assert.IsNotNull(i7);

            Assert.AreEqual(typeof (C1), i1.GetType());
            Assert.AreEqual(typeof (C2), i2.GetType());
            Assert.AreEqual(typeof (C3), i23.GetType());
            Assert.AreEqual(typeof (C4), i3.GetType());
            Assert.AreEqual(typeof (C4), i4.GetType());
            Assert.AreEqual(typeof (C5), i5.GetType());
            Assert.AreEqual(typeof (C6), i6.GetType());
            Assert.AreEqual(typeof (C7), i7.GetType());

            Assert.AreNotEqual(i1, kernel.TryGet<I1>());
            Assert.AreEqual(i2, kernel.TryGet<I2>("adawd"));
            Assert.AreNotEqual(i23, kernel.TryGet<I2>("adawdd"));
            Assert.AreNotEqual(i3, i4);
            Assert.AreNotEqual(i3, kernel.TryGet<I3>());
            Assert.AreNotEqual(i4, kernel.TryGet<I4>());
            Assert.AreEqual(i5, kernel.TryGet<I5>());
            Assert.AreEqual(i6, kernel.TryGet<I6>());
            Assert.AreEqual(i7, kernel.TryGet<I7>());
        }

        [TestMethod]
        [ExpectedException(typeof (MetadataException))]
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
        [BindTo(typeof (C3), "adawdd", Scope = BindingScope.Transient)]
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

        public interface I3
        {
        }

        public interface I4
        {
        }

        public interface I5
        {
        }

        [BindTo(typeof (C6), Scope = BindingScope.Thread)]
        public interface I6
        {
        }

        public interface I7
        {
        }

        public class C6 : I6
        {
        }

        [BindFrom(typeof (I7), Scope = "lsdkfgnsdfoosdfhnolhnol")]
        public class C7 : I7
        {
        }

        [BindFrom(typeof (I3), typeof (I4), Scope = BindingScope.Transient)]
        public class C4 : I3, I4
        {
        }

        [BindFrom(typeof (I5), Scope = BindingScope.Singleton)]
        public class C5 : I5
        {
        }
    }
}