using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.StandardTypeExtensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }

        public static decimal ToDecimal(this string str)
        {
            return decimal.Parse(str);
        }

        public static double ToDouble(this string str)
        {
            return double.Parse(str);
        }

        public static long ToLong(this string str)
        {
            return long.Parse(str);
        }

        public static bool ToBool(this string str)
        {
            return bool.Parse(str);
        }

        public static char ToChar(this string str)
        {
            return char.Parse(str);
        }
    }
}
