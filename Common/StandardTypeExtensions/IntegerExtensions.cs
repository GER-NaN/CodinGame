using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.StandardExtensions
{
    public static class IntegerExtensions
    {
        public static bool IsEven(this int val)
        {
            return (val % 2 == 0);
        }

        public static bool IsOdd(this int val)
        {
            return !IsEven(val);
        }
    }
}
