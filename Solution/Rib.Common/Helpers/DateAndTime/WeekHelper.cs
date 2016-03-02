namespace Rib.Common.Helpers.DateAndTime
{
    using System;
    using System.Globalization;
    using JetBrains.Annotations;

    internal class WeekHelper : IWeekHelper
    {
        [NotNull] private static readonly Calendar Calendar = new GregorianCalendar();
        private readonly IFirstDayOfWeekResolver _firstDayOfWeekResolver;
        private readonly IWeekPaddingCalculator _weekPaddingCalculator;

        public WeekHelper([NotNull] IFirstDayOfWeekResolver firstDayOfWeekResolver, [NotNull] IWeekPaddingCalculator weekPaddingCalculator)
        {
            if (firstDayOfWeekResolver == null) throw new ArgumentNullException(nameof(firstDayOfWeekResolver));
            if (weekPaddingCalculator == null) throw new ArgumentNullException(nameof(weekPaddingCalculator));
            _firstDayOfWeekResolver = firstDayOfWeekResolver;
            _weekPaddingCalculator = weekPaddingCalculator;
        }

        public DateTime GetFirstDayOfWeek(DateTime dt)
        {
            return dt.AddDays(-_weekPaddingCalculator.Calculate(_firstDayOfWeekResolver.Resolve(), dt.DayOfWeek));
        }

        public DateTime GetFirstDayOfWeek(int year, int week)
        {
            var res = Calendar.AddWeeks(new DateTime(year, 1, 1), week - 1);
            return GetFirstDayOfWeek(res);
        }
    }
}