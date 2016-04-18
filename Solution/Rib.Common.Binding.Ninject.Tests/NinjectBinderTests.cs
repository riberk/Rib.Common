namespace Rib.Common.Binding.Ninject
{
    using System;
    using System.Collections.Generic;
    using global::Ninject.Syntax;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Models.Binding;

    [TestClass]
    public class NinjectBinderTests
    {
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void NinjectBinderTest() => new NinjectBinder(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void BindTest() => new NinjectBinder(types => null).Bind(null);

        [TestMethod]
        public void BindTest1()
        {
            var bindingToSyntax = new Mock<IBindingToSyntax<object>>(MockBehavior.Strict);
            var bindingNamed = new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>(MockBehavior.Strict);
            var bInfo = new Mock<IBindInfo>(MockBehavior.Strict);
            var scopeBinder = new Mock<IScopeBinder>(MockBehavior.Strict);
            var scoped = new Mock<IBindingNamedWithOrOnSyntax<object>>(MockBehavior.Strict);
            var typesFrom = new List<Type>();
            var typeTo = typeof (string);
            var bindingScope = new BindingScope("sdrjklgnlsdjkg");

            bInfo.SetupGet(x => x.From).Returns(typesFrom).Verifiable();
            bInfo.SetupGet(x => x.To).Returns(typeTo).Verifiable();
            bInfo.SetupGet(x => x.Scope).Returns(bindingScope).Verifiable();
            bInfo.SetupGet(x => x.Name).Returns((string)null).Verifiable();

            scopeBinder.Setup(x => x.InScope(bindingNamed.Object, bindingScope)).Returns(scoped.Object).Verifiable();
            bindingToSyntax.Setup(x => x.To(typeTo)).Returns(bindingNamed.Object).Verifiable();


            new NinjectBinder(types => bindingToSyntax.Object, scopeBinder.Object).Bind(new[] {bInfo.Object});

        }
    }
}