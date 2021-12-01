using System;
using Common.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTests.Crypto
{
    [TestClass]
    public class TestEnigmaMachine
    {

        /// <summary>An array of Uppercase characters </summary>
        public static readonly char[] Uppers = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        [TestMethod]
        public void TestEncodeAndDecodeRandoms()
        {
            for (int startingShift = 1; startingShift < 100; startingShift++)
            {
                //Generate a random message and random rotors
                var message = TestUtilities.GenerateRandomString(Uppers);
                var rotor1 = TestUtilities.GenerateRandomString(Uppers);
                var rotor2 = TestUtilities.GenerateRandomString(Uppers);
                var rotor3 = TestUtilities.GenerateRandomString(Uppers);

                //Encode the message
                var encodingMachine = new EnigmaMachine(message, startingShift, rotor1, rotor2, rotor3);
                var encodedMessage = encodingMachine.Encode();

                //Decode the encoded message
                var decodingMachine = new EnigmaMachine(encodedMessage, startingShift, rotor1, rotor2, rotor3);
                var decodedMessage = decodingMachine.Encode();

                //Decoded message must equal original message
                Assert.AreEqual(message, decodedMessage,$"Decoding failed for Message={message}, r1={rotor1}, r2={rotor2}, r3={rotor3}");
            }

        }
    }
}
