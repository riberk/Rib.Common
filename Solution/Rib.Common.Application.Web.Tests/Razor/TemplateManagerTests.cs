namespace Rib.Common.Application.Web.Razor
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using RazorEngine.Templating;

    [TestClass]
    [DeploymentItem("Razor/Tests/1.txt")]
    public class TemplateManagerTests
    {
        private MockRepository _mockRepo;
        private TemplateManager _templateManager;
        private Mock<IViewsPathResolver> _viewsPathResolver;

        [TestInitialize]
        public void Init()
        {
            _mockRepo = new MockRepository(MockBehavior.Strict);
            _viewsPathResolver = _mockRepo.Create<IViewsPathResolver>();
            _templateManager = new TemplateManager(_viewsPathResolver.Object);
        }

        [TestMethod]
        public void ResolveTest()
        {
            var tk = _mockRepo.Create<ITemplateKey>();
            const string value = "str";
            var text = File.ReadAllText("1.txt");
            var fi = new FileInfo("1.txt");

            tk.SetupGet(x => x.Name).Returns(value).Verifiable();
            _viewsPathResolver.Setup(x => x.ResolveFullPath(value)).Returns(fi.FullName).Verifiable();

            var res = _templateManager.Resolve(tk.Object);

            Assert.AreEqual(text, res.Template);
        }

        [TestMethod]
        public void GetKeyTest()
        {
            var tk = _mockRepo.Create<ITemplateKey>();
            const string name = "123321";
            const ResolveType resolveType = ResolveType.Global;

            var res = _templateManager.GetKey(name, resolveType, tk.Object);

            Assert.AreEqual(name, res.Name);
            Assert.AreEqual(tk.Object, res.Context);
            Assert.AreEqual(resolveType, res.TemplateType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void AddDynamicTest() => _templateManager.AddDynamic(null, null);
    }
}