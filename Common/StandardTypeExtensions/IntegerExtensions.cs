using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.StandardExtensions
{
    /// <summary>Extensions to the int class</summary>
    public static class IntegerExtensions
    {
        /// <summary>Is the int even</summary>
        public static bool IsEven(this int val)
        {
            return (val % 2 == 0);
        }

        /// <summary>Is the int odd</summary>
        public static bool IsOdd(this int val)
        {
            return !IsEven(val);
        }

        /// <summary>Checks if the int is between the given upper and lower bounds. By default the bounds are included</summary>
        /// <param name="val">The int to check</param>
        /// <param name="lower">The lower bound</param>
        /// <param name="upper">The upper bound</param>
        /// <param name="inclusive">Should the lower and upper bounds be inclusive, default is true</param>
        /// <returns></returns>
        public static bool Between(this int val, int lower, int upper, bool inclusive = true)
        {
            if(inclusive)
            {
                return val >= lower && val <= upper;
            }
            else
            {
                return val > lower && val < upper;
            }
        }
    }
}
