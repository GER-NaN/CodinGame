using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core
{
    /// <summary>Provides methods to read input from Console</summary>
    public static class Input
    {
        /// <summary>Reads a line from Console and returns as an int</summary>
        public static int ReadInt()
        {
            return Convert.ToInt32(Console.ReadLine());
        }


        /// <summary>Reads a line from Console and returns the string</summary>
        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        /// <summary>Reads a pair of inputs from Console</summary>
        /// <param name="seperator">The seperator for the inputs</param>
        /// <returns>A Tuple of the specified types</returns>
        public static Tuple<T1, T2> ReadPair<T1,T2>(char seperator = ' ')
        {
            object[] items= Console.ReadLine().Split(seperator);
            T1 item1 = (T1)Convert.ChangeType(items[0], typeof(T1));
            T2 item2 = (T2)Convert.ChangeType(items[1], typeof(T2));
            return new Tuple<T1, T2>(item1, item2);
        }

    }
}
