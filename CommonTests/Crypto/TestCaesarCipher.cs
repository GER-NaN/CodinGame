using System;
using Common.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTests.Crypto
{
    [TestClass]
    public class TestCaesarCipher
    {
        /// <summary>An array of Lowercase characters </summary>
        public static readonly char[] Lowers = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        /// <summary>An array of Uppercase characters </summary>
        public static readonly char[] Uppers = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public static readonly Random Random = new Random();

        [TestMethod]
        public void TestTextNotInAlphabet()
        {
            var plaintext = "hello world";
            var shift = 1;
            var alphabet1 = new char[] {'a','b','c','1','2','3'};
            var alphabet2 = Lowers;
            var alphabet3 = Uppers;

            //None of these have a space character so all will fail
            Assert.ThrowsException<InvalidOperationException>(() => CaesarCipher.Encode(plaintext, shift, alphabet1));
            Assert.ThrowsException<InvalidOperationException>(() => CaesarCipher.Encode(plaintext, shift, alphabet2));
            Assert.ThrowsException<InvalidOperationException>(() => CaesarCipher.Encode(plaintext, shift, alphabet3));

            //Empty string is not part of the alphabet
            Assert.ThrowsException<InvalidOperationException>(() => CaesarCipher.Encode("", shift, Uppers));
        }

        [TestMethod]
        public void TestShiftByZero()
        {
            var shift = 0;
            var alphabet = Uppers;
            
            
            Assert.AreEqual("A", CaesarCipher.Encode("A", shift, alphabet), "Shift by zero fails at beginning of alphabet");
            Assert.AreEqual("Z", CaesarCipher.Encode("Z", shift, alphabet), "Shift by zero fails at end of alphabet");
            Assert.AreEqual("ABC", CaesarCipher.Encode("ABC", shift, alphabet), "Shift by zero should not change the text");
            Assert.AreEqual("HELLOWORLD", CaesarCipher.Encode("HELLOWORLD", shift, alphabet), "Shift by zero should not change the text");
        }

        [TestMethod]
        public void TestShiftByOne()
        {
            var shift = 1;
            var alphabet = Uppers;

            Assert.AreEqual("A", CaesarCipher.Encode("Z", shift, alphabet), "Shift by 1 fails at end of alphabet (wrap around), Z shifted by 1 should equal A");
            Assert.AreEqual("B", CaesarCipher.Encode("A", shift, alphabet), "A shifted by 1 should equal B");
            Assert.AreEqual("C", CaesarCipher.Encode("B", shift, alphabet), "B shifted by 1 should equal C");
            Assert.AreEqual("D", CaesarCipher.Encode("C", shift, alphabet), "C shifted by 1 should equal D");
            Assert.AreEqual("BCD", CaesarCipher.Encode("ABC", shift, alphabet), "ABC shifted by 1 should equal BCD");
        }

        [TestMethod]
        public void TestShiftByNegativeOne()
        {
            var shift = -1;
            var alphabet = Uppers;

            Assert.AreEqual("Z", CaesarCipher.Encode("A", shift, alphabet), "Shift by -1 fails at beginning of alphabet (wrap around), A shifted by -1 should equal Z");
            Assert.AreEqual("A", CaesarCipher.Encode("B", shift, alphabet), "B shifted by -1 should equal A");
            Assert.AreEqual("Y", CaesarCipher.Encode("Z", shift, alphabet), "Z shifted by -1 should equal Y");
            Assert.AreEqual("ABC", CaesarCipher.Encode("BCD", shift, alphabet), "BCD shifted by -1 should equal ABC");
        }

        [TestMethod]
        public void TestReversals()
        {
            var alphabet = Uppers;

            //Do a bunch of shift sizes
            for(int shift = 0;shift < 100;shift ++)
            {
                //Generate a random plaintext
                var plaintext = GenerateRandomString(Lowers);

                //Generate cipher by shifting
                var cipher = CaesarCipher.Encode(plaintext, shift, Lowers);

                //Reverse the cipher by doing a negative shift on it, this should return the plaintext
                var reversal = CaesarCipher.Encode(cipher, -1 * shift, Lowers);

                Assert.AreEqual(plaintext, reversal, $"Cipher reversal failed to return the plaintext. Shift={shift}, Plaintext={plaintext}, cipher={cipher}, reversal={reversal}");
            }
        }

        [TestMethod]
        public void TestWrapArounds()
        {
            var alphabet = Uppers;

            //Wrap around 1 time
            Assert.AreEqual("ABC", CaesarCipher.Encode("ABC", 26, alphabet));
            Assert.AreEqual("ABC", CaesarCipher.Encode("ABC", -26, alphabet));

            //Wrap around twice
            Assert.AreEqual("ABC", CaesarCipher.Encode("ABC", 52, alphabet));
            Assert.AreEqual("ABC", CaesarCipher.Encode("ABC", -52, alphabet));

            //Only some of the characters in the plaintext wrap around
            Assert.AreEqual("YZA", CaesarCipher.Encode("ABC", 24, alphabet),"Wrap around failed, only C should wrap around and become A");
            Assert.AreEqual("ZAB", CaesarCipher.Encode("XYZ", -24, alphabet), "Wrap around failed, only X should wrap around and become Z");
        }



        /// <summary> Generates a random string from the given alphabet</summary>
        private string GenerateRandomString(char[] alphabet)
        {
            var stringLength = Random.Next(1,20);
            var randomString = "";

            for(int i = 0;i<stringLength;i++)
            {
                randomString += alphabet[Random.Next(0, alphabet.Length)];
            }

            return randomString;
        }
    }
}
