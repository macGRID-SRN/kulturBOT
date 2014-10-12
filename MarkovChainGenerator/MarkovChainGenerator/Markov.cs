using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovChainGenerator
{
    class Markov
    {
        CorpusDictionary cD;
        int numChar;

        public Markov(int characterCount, int numChars)
        {
            cD = new CorpusDictionary("corpus.txt", numChars);
            this.numChar = numChars;
        }

        private StringBuilder generateText()
        {
            StringBuilder sb = new StringBuilder();

            Random seed = new Random();

            List<char> charList = new List<char>();
            var dic = cD.dic;
            var keys = cD.keyCollection;

            var key = keys[seed.Next(keys.Count)];
            var character = dic[key][seed.Next(dic[key].Count)];

            sb.Append(character);

            while(sb.Length < numChar - 1)
            {
                charList.Add(character);
                charList.RemoveAt(0);
                character = dic[charList][seed.Next(dic[key].Count)];
                sb.Append(character);

            }

            return sb;
        }

        public string formatText()
        {
            StringBuilder sb = this.generateText();
            return sb.ToString();
        }
    }
}
