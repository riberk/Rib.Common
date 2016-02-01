namespace Rib.Common.Helpers.Tmp
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TmpFolderTests
    {
        [TestMethod]
        public void DisposeTest()
        {
            string path;
            using (var folder = TmpFolder.Create("C:\\tmp"))
            {
                path = folder.Path;
                var di = new DirectoryInfo(folder.Path);
                Assert.IsTrue(di.Exists);
            }
            Assert.IsFalse(Directory.Exists(path));
        }

        [TestMethod]
        [ExpectedException(typeof (IOException))]
        public void CanNotDeleteTest()
        {
            using (var folder = TmpFolder.Create("C:\\tmp"))
            {
                var di = new DirectoryInfo(folder.Path);
                di.Delete(true);
            }
        }
    }
}