using Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Crypto
{
    /// <summary>
    /// Performs Caesar Cipher on a plain text by shifting characters. Supports negative shifts, shifts greater than text length and custom alphabets.
    /// </summary>
    public static class CaesarCipher
    {

        /// <summary>Encodes the plaintext using the Caesar Shift algorithm. If a shift length is specified beyound the bounds of the alphabet, wrap around will occur.</summary>
        /// <param name="plaintext">The plain text to encode</param>
        /// <param name="shift">The length to shift each character</param>
        /// <param name="alphabet">The alphabet to use</param>
        /// <returns>The encoded plaintext, ciphertext</returns>
        /// <remarks>"Encryption with Caesar code is based on an alphabet shift (move of letters further in the alphabet), 
        /// it is a monoalphabetic substitution cipher, ie. a same letter is replaced with only one other (always the same 
        /// for given cipher message)...", https://www.dcode.fr/caesar-cipher </remarks>
        public static string Encode(string plaintext, int shift, char[] alphabet)
        {
            if( !Alphabet.IsTextInAlphabet(plaintext,alphabet))
            {
                throw new InvalidOperationException("The plaintext string cannot be constructed using the supplied alphabet array. All characters in the plaintext must exist in the supplied alphabet array.");
            }

            if(plaintext == string.Empty)
            {
                throw new InvalidOperationException("The plaintext string is empty.");
            }

            var encodedText = "";

            for (int i = 0; i < plaintext.Length; i++)
            {
                //Find the index of our character
                int characterIndex = Array.IndexOf(alphabet, plaintext[i]);

                //Shift its index
                var shiftedIndex = (characterIndex + shift);

                //Account for wrap around, ie(alphabet is only length 5 but we are asked to shift 20)
                shiftedIndex = ((shiftedIndex % alphabet.Length) + alphabet.Length) % alphabet.Length;

                //Grab the new character
                encodedText += alphabet[shiftedIndex];
            }

            return encodedText;
        }
    }
}
