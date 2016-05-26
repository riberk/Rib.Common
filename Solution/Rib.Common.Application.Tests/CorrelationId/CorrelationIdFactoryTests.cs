namespace Rib.Common.Application.CorrelationId
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CorrelationIdFactoryTests
    {
        [TestMethod]
        public void CreateTest()
        {
            new CorrelationIdFactory().Create("123");
        }
    }
}