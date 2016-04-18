namespace Rib.Common.Binding.Ninject
{
    using System;
    using global::Ninject.Syntax;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Models.Binding;

    [TestClass]
    public class DefaultScopeBinderTests
    {
        [TestMethod]
        public void InSingletonScopeTest()
        {
            var mock = new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>(MockBehavior.Strict);
            var scoped = new Mock<IBindingNamedWithOrOnSyntax<object>>(MockBehavior.Strict);
            mock.Setup(x => x.InSingletonScope()).Returns(scoped.Object).Verifiable();

            var res = new NinjectBinder.DefaultScopeBinder().InScope(mock.Object, BindingScope.SingletonScope);

            Assert.AreEqual(scoped.Object, res);
        }

        [TestMethod]
        public void InTransientScopeTest()
        {
            var mock = new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>(MockBehavior.Strict);
            var scoped = new Mock<IBindingNamedWithOrOnSyntax<object>>(MockBehavior.Strict);
            mock.Setup(x => x.InTransientScope()).Returns(scoped.Object).Verifiable();

            var res = new NinjectBinder.DefaultScopeBinder().InScope(mock.Object, BindingScope.TransientScope);

            Assert.AreEqual(scoped.Object, res);
        }

        [TestMethod]
        public void InThreadScopeTest()
        {
            var mock = new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>(MockBehavior.Strict);
            var scoped = new Mock<IBindingNamedWithOrOnSyntax<object>>(MockBehavior.Strict);
            mock.Setup(x => x.InThreadScope()).Returns(scoped.Object).Verifiable();

            var res = new NinjectBinder.DefaultScopeBinder().InScope(mock.Object, BindingScope.ThreadScope);

            Assert.AreEqual(scoped.Object, res);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void InUndefinedScopeTest()
        {
            var mock = new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>(MockBehavior.Strict);

            new NinjectBinder.DefaultScopeBinder().InScope(mock.Object, new BindingScope("flksngsdjklrgjksdrgnjksdrg"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgNull1() => new NinjectBinder.DefaultScopeBinder().InScope(null, new BindingScope("flksngsdjklrgjksdrgnjksdrg"));

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ArgNull2()
                => new NinjectBinder.DefaultScopeBinder().InScope(new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>(MockBehavior.Strict).Object, null);
    }
}