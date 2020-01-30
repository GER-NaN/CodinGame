using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core
{
    /// <summary>
    /// Helper methods for writing to standard output for CodinGame.
    /// </summary>
    public static class Output
    {
        /// <summary>
        /// Outputs the object to using WriteLine
        /// </summary>
        /// <param name="o"></param>
        public static void Print(object o)
        {
            Console.Out.WriteLine(o?.ToString());
        }

        /// <summary>Prints the objects to Console with the specified seperator </summary>
        /// <param name="a">An object</param>
        /// <param name="b">An object</param>
        /// <param name="seperator">The seperator to place between each object</param>
        public static void Print(object a, object b, string seperator = " ")
        {
            Console.WriteLine($"{a}{seperator}{b}");
        }

        /// <summary>Prints the objects to Console with the specified seperator </summary>
        /// <param name="a">An object</param>
        /// <param name="b">An object</param>
        /// <param name="c">An object</param>
        /// <param name="seperator">The seperator to place between each object</param>
        public static void Print(object a, object b, object c, string seperator = " ")
        {
            Console.WriteLine($"{a}{seperator}{b}{seperator}{c}");
        }
    }
}
