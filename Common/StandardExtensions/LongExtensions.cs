using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.StandardExtensions
{
    public static class LongExtensions
    {
        public static bool IsEven(this long val)
        {
            return (val % 2 == 0);
        }

        public static bool IsOdd(this long val)
        {
            return !IsEven(val);
        }
    }
}
