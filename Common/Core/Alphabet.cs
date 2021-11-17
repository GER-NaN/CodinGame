using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class Alphabet
    {
        /// <summary>An array of Lowercase characters </summary>
        public static readonly char[] Lowers = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        /// <summary>An array of Uppercase characters </summary>
        public static readonly char[] Uppers = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        /// <summary>Checks if the supplied text can be constructed using the alphabet</summary>
        /// <param name="text">The text to validate</param>
        /// <param name="alphabet">The alphabet that the text should come from</param>
        /// <returns>True if every character in the text can be found in the alphabet</returns>
        public static bool IsTextInAlphabet(string text, char[] alphabet)
        {
            foreach(char c in text)
            {
                if(Array.IndexOf(alphabet,c) == -1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
