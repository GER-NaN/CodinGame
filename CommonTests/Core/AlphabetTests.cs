using System;
using Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTests
{
    [TestClass]
    public class AlphabetTests
    {
        [TestMethod]
        public void TestLowers()
        {
            foreach(char c in Alphabet.Lowers)
            {
                Assert.IsTrue(char.IsLower(c), "Alphabet.Lowers contains a char that is not lowercase");
            }
        }

        [TestMethod]
        public void TestUppers()
        {
            foreach (char c in Alphabet.Uppers)
            {
                Assert.IsTrue(char.IsUpper(c), "Alphabet.Uppers contains a char that is not uppercase");
            }
        }

        [TestMethod]
        public void TestSingleAlphabetFind()
        {
            char[] testAlphabet = new char[] { '*' };
            string testText = "*";
            string testText2 = "**";

            Assert.IsTrue(Alphabet.IsTextInAlphabet(testText, testAlphabet));
            Assert.IsTrue(Alphabet.IsTextInAlphabet(testText2, testAlphabet));
        }

        [TestMethod]
        public void TestSingleAlphabetMissing()
        {
            char[] testAlphabet = new char[] { '*' };
            string testText = "(";
            string testText2 = "()";

            Assert.IsFalse(Alphabet.IsTextInAlphabet(testText, testAlphabet));
            Assert.IsFalse(Alphabet.IsTextInAlphabet(testText2, testAlphabet));
        }

        [TestMethod]
        public void TestComplexAlphabet()
        {
            char[] testAlphabet = new char[] { '1', '2', 'A', 'a', 'B', 'b', '!', '@', '.'};
            string testText = "12AaBb!@.";
            string testText2 = ".@!bBaA21";

            Assert.IsTrue(Alphabet.IsTextInAlphabet(testText, testAlphabet));
            Assert.IsTrue(Alphabet.IsTextInAlphabet(testText2, testAlphabet));
        }

        [TestMethod]
        public void TestWhitespace()
        {
            char[] testAlphabet = new char[] { ' ' };
            string space = " ";
            string doubleSpace = " ";

            Assert.IsTrue(Alphabet.IsTextInAlphabet(space, testAlphabet));
            Assert.IsTrue(Alphabet.IsTextInAlphabet(doubleSpace, testAlphabet));
        }
    }
}
