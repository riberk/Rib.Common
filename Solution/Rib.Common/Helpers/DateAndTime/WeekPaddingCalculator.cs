namespace Rib.Common.Helpers.DateAndTime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Collections;

    internal class WeekPaddingCalculator : IWeekPaddingCalculator
    {
        [NotNull] private readonly LinkedList<DayOfWeek> _days = new LinkedList<DayOfWeek>(Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>());

        public int Calculate(DayOfWeek firstDayOfWeek, DayOfWeek dow)
        {
            var node = _days.Find(firstDayOfWeek);
            if (node == null)
            {
                throw new KeyNotFoundException($"Day of week {firstDayOfWeek} could not be found");
            }
            var i = 0;
            for (; node.Value != dow; node = node.NextOrFirst())
            {
                ++i;
            }
            return i;
        }
    }
}