namespace Rib.Common.Helpers.DateAndTime
{
    using System;

    public interface IFirstDayOfWeekResolver
    {
        DayOfWeek Resolve();
    }
}