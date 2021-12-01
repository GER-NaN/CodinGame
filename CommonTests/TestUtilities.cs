using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTests
{
    public static class TestUtilities
    {
        public static readonly Random Random = new Random();

        /// <summary> Generates a random string from the given alphabet</summary>
        public static string GenerateRandomString(char[] alphabet)
        {
            var stringLength = Random.Next(1, 20);
            var randomString = "";

            for (int i = 0; i < stringLength; i++)
            {
                randomString += alphabet[Random.Next(0, alphabet.Length)];
            }

            return randomString;
        }
    }
}
