using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.StandardExtensions
{
    public static class DoubleExtensions
    {
        public static bool IsEven(this double val)
        {
            return (val % 2.0 == 0.0);
        }

        public static bool IsOdd(this double val)
        {
            return !IsEven(val);
        }
    }
}
