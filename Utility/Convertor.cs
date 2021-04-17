using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDAL.Utility
{
    public static class Convertor
    {
        public static string ToPersianDate(this DateTime date)
        {
            if (date == DateTime.MinValue)
                return string.Empty;
            System.Globalization.PersianCalendar calendar = new System.Globalization.PersianCalendar();

            return $"{calendar.GetYear(date)}/{calendar.GetMonth(date).ToString().PadLeft(2, '0')}/{calendar.GetDayOfMonth(date).ToString().PadLeft(2, '0')}";
        }



        public static byte[] ToByteArray(this string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                return null;
            }
            return System.IO.File.ReadAllBytes(fileName);
        }

        public static bool IsPrimitive(Type t)
        {
            // TODO: put any type here that you consider as primitive as I didn't
            // quite understand what your definition of primitive type is
            return new[] {
            typeof(string),
            typeof(char),
            typeof(byte),
            typeof(sbyte),
            typeof(ushort),
            typeof(short),
            typeof(uint),
            typeof(int),
            typeof(ulong),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(Boolean),
            typeof(DateTime),
                 }.Contains(t);
        }
    }
}
