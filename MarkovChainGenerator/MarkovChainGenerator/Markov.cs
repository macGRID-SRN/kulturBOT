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
        int characterCount;
        int numChar;

        public Markov(int characterCount, int numChars)
        {
            cD = new CorpusDictionary("corpus.txt", numChars);
            this.numChar = numChars;
            this.characterCount = characterCount;
        }

        private StringBuilder generateText()
        {
            StringBuilder sb = new StringBuilder();

            Random seed = new Random();

            var dic = cD.dic;
            var keys = cD.keyCollection;

            var key = keys[seed.Next(keys.Count)];
            var character = dic[key][seed.Next(dic[key].Count)];
 
            sb.Append(character);
            
            while(sb.Length < characterCount - 1)
            {
                key += character;
                key= key.Substring(1,key.Length -1);
                try
                {
                    character = dic[key][seed.Next(dic[key].Count)];
                }catch(KeyNotFoundException e)
                {
                    key = keys[seed.Next(keys.Count)];
                    character = dic[key][seed.Next(dic[key].Count)];
                }
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
