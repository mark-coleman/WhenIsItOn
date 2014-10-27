using System;

namespace Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateOnly(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }

        public static string ToDayAndDate(this DateTime dateTime)
        {
            return dateTime.ToString("ddd dd/MM/yyyy");
        }
    }
}
