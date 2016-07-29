namespace Rib.Common.Application.Web.Razor
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using RazorEngine;
    using RazorEngine.Templating;

    [TestClass]
    public class RazorTemplateRunerTests
    {
        private MockRepository _mockFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Run1WithTitleArgumentNull() => new RazorTemplateRuner().Run<int?>(null, 10);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Run2WithTitleArgumentNull() => new RazorTemplateRuner().Run(null, "123", typeof (string), "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Run3WithTitleArgumentNull() => new RazorTemplateRuner().Run("123", null, typeof (string), "321");

        [TestMethod]
        public void RunTest()
        {
            const string view = "Hello, @Model!";
            const string templateKey = "hello";
            Engine.Razor.AddTemplate(templateKey, new LoadedTemplateSource(view));
            var res = new RazorTemplateRuner().Run(templateKey, "World");
            Assert.AreEqual("Hello, World!", res);
        }

        [TestMethod]
        public void RunWithCompiliedTest()
        {
            const string view = "Hello, @Model!";
            const string templateKey = "hello";
            Engine.Razor.AddTemplate(templateKey, new LoadedTemplateSource(view));
            Engine.Razor.Compile(templateKey, typeof (string));

            var res = new RazorTemplateRuner().Run(templateKey, "World");
            Assert.AreEqual("Hello, World!", res);
        }

        [TestMethod]
        public void RunTest1()
        {
            const string view = "Hello, @Model! @ViewBag.Title";
            const string templateKey = "hello1";
            Engine.Razor.AddTemplate(templateKey, new LoadedTemplateSource(view));

            var res = new RazorTemplateRuner().Run(templateKey, "Title", typeof (string), "World");
            Assert.AreEqual("Hello, World! Title", res);
        }

        [TestMethod]
        public void RunTestWithCompilied1()
        {
            const string view = "Hello, @Model! @ViewBag.Title";
            const string templateKey = "hello1";
            Engine.Razor.AddTemplate(templateKey, new LoadedTemplateSource(view));

            var res = new RazorTemplateRuner().Run(templateKey, "Title", typeof (string), "World");
            Assert.AreEqual("Hello, World! Title", res);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void RunTestWithNullResult()
        {
            const string templateKey = "hello2";
            const string view = " @Model ";

            Engine.Razor.AddTemplate(templateKey, new LoadedTemplateSource(view));
            new RazorTemplateRuner().Run(templateKey, "Title", typeof (string), "");
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void RunTestWithNullResult1()
        {
            const string view = " @Model ";
            const string templateKey = "hello3";
            Engine.Razor.AddTemplate(templateKey, new LoadedTemplateSource(view));
            new RazorTemplateRuner().Run(templateKey, "");
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}