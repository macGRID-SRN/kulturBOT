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
        public List<string> keyCollection { get; set; }
        public Dictionary<string, List<char>> dic { get; set; }
        int numChar;

        public CorpusDictionary(string filePath, int numChars)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                futuristManifesto = sr.ReadToEnd();
                futuristManifesto = futuristManifesto.Replace("\r", "");
                futuristManifesto = futuristManifesto.Replace("\n", "");

            }
            dic = new Dictionary<string, List<char>>();
            this.numChar = numChars;
            keyCollection = new List<string>();
            this.parseCorpus();
        }

        private void parseCorpus()
        {
            string tempArray;

            for (int i = 0; i < futuristManifesto.Length - numChar; i++)
            {
                tempArray = string.Empty;

                for (int j = 0; j < numChar; j++)
                {
                    tempArray += futuristManifesto[i + j];
                    if(i + j == i + numChar - 1 && j == numChar - 1)
                    {
//
                   }
                }
                keyCollection.Add(tempArray);
                bool containsKey = dic.ContainsKey(tempArray);
                
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
