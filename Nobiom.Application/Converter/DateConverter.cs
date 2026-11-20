using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nobiom.Application.Converter
{
    public static class DateConverter
    {
        public static string ToShamsi(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(date) + "/" +
                pc.GetMonth(date).ToString("00") + "/" +
                pc.GetDayOfMonth(date).ToString("00");
        }

        public static string ToShamsiWithTime(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(date) + "/" +
                pc.GetMonth(date).ToString("00") + "/" +
                pc.GetDayOfMonth(date).ToString("00") + " - " +
                pc.GetHour(date).ToString("00") + ":" +
                pc.GetMinute(date).ToString("00");
        }

        public static DateTime ToMiladi(this string date)
        {
            var parts = date.Split('/');
            var pc = new PersianCalendar();

            return pc.ToDateTime(
                int.Parse(parts[0]),
                int.Parse(parts[1]),
                int.Parse(parts[2]),
                0, 0, 0, 0
            );
        }
    }
}
