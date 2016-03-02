namespace Rib.Common.Helpers.DateAndTime
{
    using System;

    internal interface IFirstDayOfWeekResolver
    {
        DayOfWeek Resolve();
    }
}