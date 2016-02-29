namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Models.Metadata;

    [TestClass]
    public class AssemblyInfoRetrieverTests
    {
        private Mock<ICacherFactory> _cacherFactory;
        private MockRepository _mockFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _cacherFactory = _mockFactory.Create<ICacherFactory>();
        }

        private AssemblyInfoRetriever Create()
        {
            return new AssemblyInfoRetriever(_cacherFactory.Object);
        }

        [TestMethod]
        [DeploymentItem(@"Helpers\Metadata\ClassLibrary1.dll")]
        public void RetrieveTest()
        {
            var cacher = _mockFactory.Create<ICacher<IAssemblyInfo>>();
            var location = Assembly.GetExecutingAssembly().Location;
            var path = Path.Combine(Path.GetDirectoryName(location), "ClassLibrary1.dll");
            _cacherFactory.Setup(x => x.Create<IAssemblyInfo>(null, null)).Returns(cacher.Object).Verifiable();
            cacher.Setup(x => x.GetOrAdd(It.IsAny<string>(), It.IsAny<Func<string, IAssemblyInfo>>()))
                  .Returns((string s, Func<string, IAssemblyInfo> f) => f(s))
                  .Verifiable();

            var result = Create().Retrieve(Assembly.LoadFile(path));

            Assert.AreEqual(new DateTime(635923404290000000l), result.BuildAt);
            Assert.AreEqual("1.0.0.0", result.Version);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}