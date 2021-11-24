using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class DebugTool
    {
        private static Dictionary<string, Stopwatch> Timers = new Dictionary<string, Stopwatch>();

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

        /// <summary> Starts a timer</summary>
        /// <param name="name">The name of the timer. If a timer with the same name exists it is reset and started again.</param>
        public static void StartTimer(string name)
        {
            if(Timers.ContainsKey(name))
            {
                Timers[name] = Stopwatch.StartNew();
            }
            else
            {
                Timers.Add(name, Stopwatch.StartNew());
            }
        }

        /// <summary>Stops a timer with the given name and prints the ElapsedMilliseconds</summary>
        /// <param name="name">The name of the timer to stop</param>
        public static void StopTimer(string name)
        {
            if (Timers.ContainsKey(name))
            {
                Timers[name].Stop();
                DebugTool.Print($"Timer {name}: {Timers[name].ElapsedMilliseconds}ms");
            }
        }

        /// <summary>Checks the elapsed time on the given timer name. Does not stop the timer</summary>
        /// <param name="name">The name of the timer</param>
        /// <param name="label">A label to add to the timer output so you can customize the print statement</param>
        public static void CheckTimer(string name, string label)
        {
            if (Timers.ContainsKey(name))
            {
                DebugTool.Print($"Timer {name} {label}: {Timers[name].ElapsedMilliseconds}ms");
            }
        }
    }
}
