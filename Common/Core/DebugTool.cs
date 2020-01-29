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

        public static void Print(string label, object o)
        {
            Print(label + ": " + o);
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
