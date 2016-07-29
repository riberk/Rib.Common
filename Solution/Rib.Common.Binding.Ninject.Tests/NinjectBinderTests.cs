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
            var bindingToSyntax = new Mock<IBindingToSyntax<object>>(MockBehavior.Loose);
            var bindingNamed = new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>(MockBehavior.Loose);
            var bInfo = new Mock<IBindInfo>(MockBehavior.Loose);
            var scopeBinder = new Mock<IScopeBinder>(MockBehavior.Loose);
            var scoped = new Mock<IBindingNamedWithOrOnSyntax<object>>(MockBehavior.Loose);
            var typesFrom = new List<Type>();
            var typeTo = typeof (string);
            var bindingScope = new BindingScope("sdrjklgnlsdjkg");

            bindingToSyntax.Name = "bindingToSyntax";
            bindingNamed.Name = "bindingNamed";
            bInfo.Name = "bInfo";
            scopeBinder.Name = "scopeBinder";
            scoped.Name = "scoped";

            bInfo.SetupGet(x => x.From).Returns(typesFrom).Verifiable();
            bInfo.SetupGet(x => x.To).Returns(typeTo).Verifiable();
            bInfo.SetupGet(x => x.Scope).Returns(bindingScope).Verifiable();
            bInfo.SetupGet(x => x.Name).Returns((string)null).Verifiable();
            bindingNamed.Setup(x => x.GetHashCode()).Returns(100.GetHashCode()).Verifiable();
            bindingNamed.Setup(x => x.ToString()).Returns("100").Verifiable();

            scopeBinder.Setup(x => x.InScope(bindingNamed.Object, bindingScope)).Returns(scoped.Object).Verifiable();
            bindingToSyntax.Setup(x => x.To(typeTo)).Returns(bindingNamed.Object).Verifiable();


            new NinjectBinder(types => bindingToSyntax.Object, scopeBinder.Object).Bind(new[] {bInfo.Object});

        }
    }
}