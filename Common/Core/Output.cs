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

        /// <summary>
        /// Outputs the object with the label prefixed using WriteLine. No space or seperating character is included.
        /// </summary>
        /// <param name="label">The label to prefix to the object</param>
        /// <param name="o">The object to print</param>
        public static void Print(string label, object o)
        {
            Print(label + o);
        }
    }
}
