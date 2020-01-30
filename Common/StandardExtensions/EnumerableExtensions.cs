using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.StandardExtensions
{
    public static class EnumerableExtensions
    {
        /// <summary>Calculates the median (middle value) on the list of long integers</summary>
        /// <returns>The middle value if the number of elements is Odd. An average of the two middle elements if the number of elements is even.</returns>
        public static long Median(this IEnumerable<long> list)
        {
            //Copy the list
            List<long> copy = list.ToList();

            //Sort it
            copy.Sort();

            int count = copy.Count();
            int mid = count / 2;

            //Assume its odd and the mid is correct.
            long median = copy[mid];

            //Gotta average the 2 middles when even
            if (count % 2 == 0)
            {
                long var1 = copy[mid];
                long var2 = copy[mid - 1];
                median = (var1 + var2) / 2;
            }
            return median;
        }
    }
}
