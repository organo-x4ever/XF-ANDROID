using com.organo.x4ever.Models;
using System;

namespace com.organo.x4ever.Converters
{
    public static class PeriodIntervalToDatetime
    {
        public static DateTime Convert(int value, IntervalPeriodType intervalPeriodType)
        {
            if (!int.TryParse(value.ToString() as string, out int interval))
                interval = 7;
            switch (intervalPeriodType)
            {
                case IntervalPeriodType.Years:
                    return DateTime.Now.AddYears(interval);

                case IntervalPeriodType.Months:
                    return DateTime.Now.AddMonths(interval);

                case IntervalPeriodType.Days:
                    return DateTime.Now.AddDays(interval);

                case IntervalPeriodType.Hours:
                    return DateTime.Now.AddHours(interval);

                case IntervalPeriodType.Minutes:
                    return DateTime.Now.AddMinutes(interval);

                default:
                    return DateTime.Now.AddDays(interval);
            }
        }

        public static DateTime ConvertBack(int value, IntervalPeriodType intervalPeriodType)
        {
            if (!int.TryParse(value.ToString() as string, out int interval))
                interval = 7;
            switch (intervalPeriodType)
            {
                case IntervalPeriodType.Years:
                    return DateTime.Now.AddYears(-interval);

                case IntervalPeriodType.Months:
                    return DateTime.Now.AddMonths(-interval);

                case IntervalPeriodType.Days:
                    return DateTime.Now.AddDays(-interval);

                case IntervalPeriodType.Hours:
                    return DateTime.Now.AddHours(-interval);

                case IntervalPeriodType.Minutes:
                    return DateTime.Now.AddMinutes(-interval);

                default:
                    return DateTime.Now.AddDays(-interval);
            }
        }
    }
}