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
        [ExpectedException(typeof (NotSupportedException))]
        public void BindTest1()
        {
            var bindingToSyntax = new Mock<IBindingToSyntax<object>>(MockBehavior.Strict);
            var bindingNamed = new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>(MockBehavior.Strict);
            var bInfo = new Mock<IBindInfo>(MockBehavior.Strict);
            var typesFrom = new List<Type>();
            var typeTo = typeof (string);

            bInfo.SetupGet(x => x.From).Returns(typesFrom).Verifiable();
            bInfo.SetupGet(x => x.To).Returns(typeTo).Verifiable();
            bInfo.SetupGet(x => x.Scope).Returns(new BindingScope("sdrjklgnlsdjkg")).Verifiable();

            bindingToSyntax.Setup(x => x.To(typeTo)).Returns(bindingNamed.Object).Verifiable();


            new NinjectBinder(types => bindingToSyntax.Object).Bind(new[] {bInfo.Object});
        }
    }
}