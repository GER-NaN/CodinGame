using Common.Core;
using Common.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    /*
     * https://www.codingame.com/ide/puzzle/encryptiondecryption-of-enigma-machine
     * 
     * During World War II, the Germans were using an encryption code called Enigma – which 
     * was basically an encryption machine that encrypted messages for transmission. The Enigma 
     * code went many years unbroken. Here's How the basic machine works:

        First Caesar shift is applied using an incrementing number:
        If String is AAA and starting number is 4 then output will be EFG.
        A + 4 = E
        A + 4 + 1 = F
        A + 4 + 1 + 1 = G

        Now map EFG to first ROTOR such as:
        ABCDEFGHIJKLMNOPQRSTUVWXYZ
        BDFHJLCPRTXVZNYEIWGAKMUSQO
        So EFG becomes JLC. Then it is passed through 2 more rotors to get the final value.

        If the second ROTOR is AJDKSIRUXBLHWTMCQGZNPYFVOE, we apply the substitution step again thus:
        ABCDEFGHIJKLMNOPQRSTUVWXYZ
        AJDKSIRUXBLHWTMCQGZNPYFVOE
        So JLC becomes BHD.

        If the third ROTOR is EKMFLGDQVZNTOWYHXUSPAIBRCJ, then the final substitution is:
        ABCDEFGHIJKLMNOPQRSTUVWXYZ
        EKMFLGDQVZNTOWYHXUSPAIBRCJ
        So BHD becomes KQF.

        Final output is sent via Radio Transmitter.

        ----------------------------------------------------

        Input
        Line 1: ENCODE or DECODE
        Line 2: Starting shift N
        Lines 3-5:
        BDFHJLCPRTXVZNYEIWGAKMUSQO ROTOR I
        AJDKSIRUXBLHWTMCQGZNPYFVOE ROTOR II
        EKMFLGDQVZNTOWYHXUSPAIBRCJ ROTOR III
        Line 6: Message to Encode or Decode

        Output
        Encoded or Decoded String

        Constraints
        0 ≤ N < 26
        Message consists only of uppercase letters (A-Z)
        1 ≤ Message length < 50

        */
    class Solution
    {
        static void Main(string[] args)
        {

            //Inputs
            /********************************************/
            string operation = Console.ReadLine();
            int shift = int.Parse(Console.ReadLine());

            string[] rotors = new string[3];
            for (int i = 0; i < 3; i++)
            {
                rotors[i] = Console.ReadLine();
            }

            string message = Console.ReadLine();
            /********************************************/

            var enigmaMachine = new EnigmaMachine(message, shift, rotors[0], rotors[1], rotors[2]);

            if (operation == "ENCODE")
            {
                Console.WriteLine(enigmaMachine.Encode());
            }
            else
            {
                Console.WriteLine(enigmaMachine.Decode());
            }
        }
    }
}
