using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MarkovChainGenerator
{
    class CorpusDictionary
    {
        string futuristManifesto;
        public List<List<char>> keyCollection { get; set; }
        public Dictionary<List<char>, List<char>> dic { get; set; }
        int numChar;

        public CorpusDictionary(string filePath, int numChars)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                futuristManifesto = sr.ReadToEnd();
            }
            dic = new Dictionary<List<char>, List<char>>();
            this.numChar = numChars;
            keyCollection = new List<List<char>>();
            this.parseCorpus();
        }

        private void parseCorpus()
        {
            List<char> tempArray;

            for (int i = 0; i < futuristManifesto.Length - numChar; i++)
            {
                tempArray = new List<char>();
                for (int j = 0; j < numChar - 1; j++)
                {
                    tempArray.Add(futuristManifesto[i + j]);
                }
                keyCollection.Add(tempArray);
                bool containsKey = dic.ContainsKey(tempArray);
                if(containsKey)
                    Console.WriteLine(containsKey);
                if (containsKey)
                {
                    dic[tempArray].Add(futuristManifesto[i + numChar]);
                }
                else
                {
                    dic.Add(tempArray, new List<char> { futuristManifesto[i + numChar] });
                }
            }
      }
    }
}
