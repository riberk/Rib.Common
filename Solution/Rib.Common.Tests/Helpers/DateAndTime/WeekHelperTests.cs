namespace Rib.Common.Helpers.DateAndTime
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class WeekHelperTests
    {
        private Mock<IFirstDayOfWeekResolver> _fdResolver;
        private MockRepository _mockRepository;
        private Mock<IWeekPaddingCalculator> _weekPadding;

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _fdResolver = _mockRepository.Create<IFirstDayOfWeekResolver>();
            _weekPadding = _mockRepository.Create<IWeekPaddingCalculator>();
        }

        private WeekHelper Create()
        {
            return new WeekHelper(_fdResolver.Object, _weekPadding.Object);
        }

        [TestMethod]
        public void GetFirstDayOfWeekTest()
        {
            _fdResolver.Setup(x => x.Resolve()).Returns(DayOfWeek.Monday).Verifiable();
            _weekPadding.Setup(x => x.Calculate(DayOfWeek.Monday, DayOfWeek.Wednesday)).Returns(2).Verifiable();
            var fdow = Create().GetFirstDayOfWeek(new DateTime(2016, 3, 2));
            Assert.AreEqual(new DateTime(2016, 2, 29), fdow);
        }

        [TestMethod]
        public void GetFirstDayOfWeekTest1()
        {
            _fdResolver.Setup(x => x.Resolve()).Returns(DayOfWeek.Monday).Verifiable();
            _weekPadding.Setup(x => x.Calculate(DayOfWeek.Monday, DayOfWeek.Friday)).Returns(4).Verifiable();
            var fdow = Create().GetFirstDayOfWeek(2016, 10);
            Assert.AreEqual(new DateTime(2016, 2, 29), fdow);
        }
    }
}