namespace Rib.Common.Helpers.DateAndTime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Collections;

    internal class WeekPaddingCalculator : IWeekPaddingCalculator
    {
        [NotNull] private static readonly List<DayOfWeek> DaysOfWeek = Enum.GetValues(typeof (DayOfWeek)).Cast<DayOfWeek>().ToList();
        [NotNull] private readonly LinkedList<DayOfWeek> _days;
        [NotNull] private readonly HashSet<DayOfWeek> _daysHash;

        public WeekPaddingCalculator()
        {
            _days = new LinkedList<DayOfWeek>(DaysOfWeek);
            _daysHash = new HashSet<DayOfWeek>(DaysOfWeek);
        }

        public int Calculate(DayOfWeek firstDayOfWeek, DayOfWeek dow)
        {
            if (!_daysHash.Contains(firstDayOfWeek))
            {
                throw new ArgumentOutOfRangeException(nameof(firstDayOfWeek), $"Day of week {firstDayOfWeek} could not be found");
            }
            if (!_daysHash.Contains(dow))
            {
                throw new ArgumentOutOfRangeException(nameof(firstDayOfWeek), $"Day of week {dow} could not be found");
            }
            var node = _days.Find(firstDayOfWeek);
            var i = 0;
            for (; node.Value != dow; node = node.NextOrFirst())
            {
                ++i;
            }
            return i;
        }
    }
}