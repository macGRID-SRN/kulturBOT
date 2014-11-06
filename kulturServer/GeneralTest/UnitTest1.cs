using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GeneralTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMarkovForTwitter()
        {
            for (int i = 0; i < 100; i++)
            {
                string markov = kulturServer.Helpers.Markov.GetNextTwitterMarkov();
                Assert.IsTrue(markov.Length <= 140);
                //System.Diagnostics.Debug.WriteLine(markov.Length + " " + markov);
            }

            foreach (byte myByte in kulturServer.Helpers.Hashing.GetMd5HashBytes(new byte[] { 12, 1 }))
            {
                System.Diagnostics.Debug.WriteLine(myByte);
            }
        }

        [TestMethod]
        public void TestMarkovForSpecialCharacters()
        {
            Assert.AreEqual(kulturServer.Helpers.Markov.textHelper("Ridle, and celestial grime, amidst the monotonous railway stations devouring smoking serpents; factories where you sleep side by side for t."), "Ridle and celestial grime amidst the monotonous railway stations devouring smoking serpents factories where you sleep side by side for t.");
        }

        

    }
}
