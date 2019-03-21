using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingGame.Core
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
    }
}
