namespace Rib.Common.Helpers.DateAndTime
{
    using System;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(WeekPaddingCalculator))]
    internal interface IWeekPaddingCalculator
    {
        int Calculate(DayOfWeek firstDayOfWeek, DayOfWeek dow);
    }
}