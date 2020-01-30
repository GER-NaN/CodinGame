using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class DebugTool
    {
        public static void Print(object o)
        {
            Console.Error.WriteLine(o?.ToString());
        }

        public static void Print(object a, object b, string seperator = " ")
        {
            Print($"{a}{seperator}{b}");
        }

        public static void Print(object a, object b, object c, string seperator = " ")
        {
            Print($"{a}{seperator}{b}{seperator}{c}");
        }

        public static void Print(object a, object b, object c, object d, string seperator = " ")
        {
            Print($"{a}{seperator}{b}{seperator}{c}{seperator}{d}");
        }

        /// <summary>Prints each item on its own line</summary>
        public static void PrintAll(object[] items)
        {
            foreach(var item in items)
            {
                Print(item);
            }
        }
    }
}
