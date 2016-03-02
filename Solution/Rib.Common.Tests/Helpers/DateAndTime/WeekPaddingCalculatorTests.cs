namespace Rib.Common.Helpers.DateAndTime
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WeekPaddingCalculatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CalculateNotFoundTest1()
        {
            new WeekPaddingCalculator().Calculate((DayOfWeek)1000, DayOfWeek.Friday);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CalculateNotFoundTest2()
        {
            new WeekPaddingCalculator().Calculate(DayOfWeek.Friday, (DayOfWeek)1000);
        }

        [TestMethod]
        public void CalculateTest()
        {
            var calc = new WeekPaddingCalculator();

            Assert.AreEqual(0, calc.Calculate(DayOfWeek.Monday, DayOfWeek.Monday));
            Assert.AreEqual(1, calc.Calculate(DayOfWeek.Monday, DayOfWeek.Tuesday));
            Assert.AreEqual(2, calc.Calculate(DayOfWeek.Monday, DayOfWeek.Wednesday));
            Assert.AreEqual(3, calc.Calculate(DayOfWeek.Monday, DayOfWeek.Thursday));
            Assert.AreEqual(4, calc.Calculate(DayOfWeek.Monday, DayOfWeek.Friday));
            Assert.AreEqual(5, calc.Calculate(DayOfWeek.Monday, DayOfWeek.Saturday));
            Assert.AreEqual(6, calc.Calculate(DayOfWeek.Monday, DayOfWeek.Sunday));

            Assert.AreEqual(6, calc.Calculate(DayOfWeek.Tuesday, DayOfWeek.Monday));
            Assert.AreEqual(0, calc.Calculate(DayOfWeek.Tuesday, DayOfWeek.Tuesday));
            Assert.AreEqual(1, calc.Calculate(DayOfWeek.Tuesday, DayOfWeek.Wednesday));
            Assert.AreEqual(2, calc.Calculate(DayOfWeek.Tuesday, DayOfWeek.Thursday));
            Assert.AreEqual(3, calc.Calculate(DayOfWeek.Tuesday, DayOfWeek.Friday));
            Assert.AreEqual(4, calc.Calculate(DayOfWeek.Tuesday, DayOfWeek.Saturday));
            Assert.AreEqual(5, calc.Calculate(DayOfWeek.Tuesday, DayOfWeek.Sunday));

            Assert.AreEqual(5, calc.Calculate(DayOfWeek.Wednesday, DayOfWeek.Monday));
            Assert.AreEqual(6, calc.Calculate(DayOfWeek.Wednesday, DayOfWeek.Tuesday));
            Assert.AreEqual(0, calc.Calculate(DayOfWeek.Wednesday, DayOfWeek.Wednesday));
            Assert.AreEqual(1, calc.Calculate(DayOfWeek.Wednesday, DayOfWeek.Thursday));
            Assert.AreEqual(2, calc.Calculate(DayOfWeek.Wednesday, DayOfWeek.Friday));
            Assert.AreEqual(3, calc.Calculate(DayOfWeek.Wednesday, DayOfWeek.Saturday));
            Assert.AreEqual(4, calc.Calculate(DayOfWeek.Wednesday, DayOfWeek.Sunday));

            Assert.AreEqual(4, calc.Calculate(DayOfWeek.Thursday, DayOfWeek.Monday));
            Assert.AreEqual(5, calc.Calculate(DayOfWeek.Thursday, DayOfWeek.Tuesday));
            Assert.AreEqual(6, calc.Calculate(DayOfWeek.Thursday, DayOfWeek.Wednesday));
            Assert.AreEqual(0, calc.Calculate(DayOfWeek.Thursday, DayOfWeek.Thursday));
            Assert.AreEqual(1, calc.Calculate(DayOfWeek.Thursday, DayOfWeek.Friday));
            Assert.AreEqual(2, calc.Calculate(DayOfWeek.Thursday, DayOfWeek.Saturday));
            Assert.AreEqual(3, calc.Calculate(DayOfWeek.Thursday, DayOfWeek.Sunday));

            Assert.AreEqual(3, calc.Calculate(DayOfWeek.Friday, DayOfWeek.Monday));
            Assert.AreEqual(4, calc.Calculate(DayOfWeek.Friday, DayOfWeek.Tuesday));
            Assert.AreEqual(5, calc.Calculate(DayOfWeek.Friday, DayOfWeek.Wednesday));
            Assert.AreEqual(6, calc.Calculate(DayOfWeek.Friday, DayOfWeek.Thursday));
            Assert.AreEqual(0, calc.Calculate(DayOfWeek.Friday, DayOfWeek.Friday));
            Assert.AreEqual(1, calc.Calculate(DayOfWeek.Friday, DayOfWeek.Saturday));
            Assert.AreEqual(2, calc.Calculate(DayOfWeek.Friday, DayOfWeek.Sunday));

            Assert.AreEqual(2, calc.Calculate(DayOfWeek.Saturday, DayOfWeek.Monday));
            Assert.AreEqual(3, calc.Calculate(DayOfWeek.Saturday, DayOfWeek.Tuesday));
            Assert.AreEqual(4, calc.Calculate(DayOfWeek.Saturday, DayOfWeek.Wednesday));
            Assert.AreEqual(5, calc.Calculate(DayOfWeek.Saturday, DayOfWeek.Thursday));
            Assert.AreEqual(6, calc.Calculate(DayOfWeek.Saturday, DayOfWeek.Friday));
            Assert.AreEqual(0, calc.Calculate(DayOfWeek.Saturday, DayOfWeek.Saturday));
            Assert.AreEqual(1, calc.Calculate(DayOfWeek.Saturday, DayOfWeek.Sunday));

            Assert.AreEqual(1, calc.Calculate(DayOfWeek.Sunday, DayOfWeek.Monday));
            Assert.AreEqual(2, calc.Calculate(DayOfWeek.Sunday, DayOfWeek.Tuesday));
            Assert.AreEqual(3, calc.Calculate(DayOfWeek.Sunday, DayOfWeek.Wednesday));
            Assert.AreEqual(4, calc.Calculate(DayOfWeek.Sunday, DayOfWeek.Thursday));
            Assert.AreEqual(5, calc.Calculate(DayOfWeek.Sunday, DayOfWeek.Friday));
            Assert.AreEqual(6, calc.Calculate(DayOfWeek.Sunday, DayOfWeek.Saturday));
            Assert.AreEqual(0, calc.Calculate(DayOfWeek.Sunday, DayOfWeek.Sunday));
        }
    }
}