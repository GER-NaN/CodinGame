using Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Crypto
{
    /// <summary>
    /// An EnigmaMachine as described in this puzzle: https://www.codingame.com/ide/puzzle/encryptiondecryption-of-enigma-machine
    /// </summary>
    public class EnigmaMachine
    {
        private readonly string Message;
        private readonly int StartingShift;
        private readonly char[] Rotor1;
        private readonly char[] Rotor2;
        private readonly char[] Rotor3;
         
        public EnigmaMachine(string message, int startingShift, string rotor1, string rotor2, string rotor3)
        {
            Message = message;
            StartingShift = startingShift;
            Rotor1 = rotor1.ToCharArray();
            Rotor2 = rotor2.ToCharArray();
            Rotor3 = rotor3.ToCharArray();
        }

        public string Encode()
        {
            //Start by Caesar Shift each character using an incrementing number
            var encodedMessage = Message.ToCharArray();

            for(int i=0;i<encodedMessage.Length;i++)
            {
                encodedMessage[i] = CaesarCipher.Encode(encodedMessage[i].ToString(), StartingShift + i, Alphabet.Uppers)[0];
            }

            //Rotor1
            for(int i =0;i<encodedMessage.Length;i++)
            {
                encodedMessage[i] = Rotor1[Array.IndexOf(Alphabet.Uppers, encodedMessage[i])];
            }

            //Rotor2
            for (int i = 0; i < encodedMessage.Length; i++)
            {
                encodedMessage[i] = Rotor2[Array.IndexOf(Alphabet.Uppers, encodedMessage[i])];
            }

            //Rotor3
            for (int i = 0; i < encodedMessage.Length; i++)
            {
                encodedMessage[i] = Rotor3[Array.IndexOf(Alphabet.Uppers, encodedMessage[i])];
            }

            return new string(encodedMessage);
        }

        public string Decode()
        {

            var decodedMessage = Message.ToCharArray();

            //Start by Decoding Rotor3 
            for(int i = 0; i< decodedMessage.Length; i++)
            {
                decodedMessage[i] = Alphabet.Uppers[Array.IndexOf(Rotor3, decodedMessage[i])];
            }

            //Decoding Rotor2
            for (int i = 0; i < decodedMessage.Length; i++)
            {
                decodedMessage[i] = Alphabet.Uppers[Array.IndexOf(Rotor2, decodedMessage[i])];
            }

            //Decoding Rotor1
            for (int i = 0; i < decodedMessage.Length; i++)
            {
                decodedMessage[i] = Alphabet.Uppers[Array.IndexOf(Rotor1, decodedMessage[i])];
            }

            //Caesar Shifting each character using a incrementing counter (but in subtracting, because were decoding)
            for (int i = 0; i < decodedMessage.Length; i++)
            {
                decodedMessage[i] = CaesarCipher.Encode(decodedMessage[i].ToString(), (-1) * (StartingShift + i), Alphabet.Uppers)[0];
            }

            return new string(decodedMessage);
        }
    }
}
