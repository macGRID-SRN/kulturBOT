using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace kulturServer.Helpers
{
    public class Markov
    {
        private const int MARKOV_CHAIN_SIZE = 6;
        private static Random seed = new Random();

        private static Markov GenTwitterTextForPictures = new Markov(117, MARKOV_CHAIN_SIZE);
        private static Markov GenTwitterText = new Markov(140, MARKOV_CHAIN_SIZE);

        CorpusDictionary cD;
        int characterCount;
        int numChar;

        private Markov(int characterCount, int numChars)
        {
            cD = new CorpusDictionary(@"Resources\corpus.txt", numChars);
            this.numChar = numChars;
            this.characterCount = characterCount;
        }

        private StringBuilder generateText()
        {
            StringBuilder sb = new StringBuilder();

            var dic = cD.dic;
            var keys = cD.keyCollection;

            var key = keys[seed.Next(keys.Count)];
            var character = dic[key][seed.Next(dic[key].Count)];

            sb.Append(character);

            while (sb.Length < characterCount - 1)
            {
                key += character;
                key = key.Substring(1, key.Length - 1);
                try
                {
                    character = dic[key][seed.Next(dic[key].Count)];
                }
                catch (KeyNotFoundException e)
                {
                    key = keys[seed.Next(keys.Count)];
                    character = dic[key][seed.Next(dic[key].Count)];
                }
                sb.Append(character);

            }

            return sb;
        }

        private string formatText()
        {
            int puncNum = seed.Next(3);
            string[] punc = new string[] { "!", ".", "?" };
            StringBuilder sb = this.generateText();
            string text = sb.ToString();
            text = text.Substring(0, 1).ToUpper() + text.Substring(1, text.Length - 1) + punc[puncNum];

            return text;
        }

        public static string textHelper(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '!' || c == '?')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string GetNextTwitterMarkov()
        {
            return GenTwitterText.formatText();
        }

        public static string GetNextTwitterPictureMarkov()
        {
            return GenTwitterTextForPictures.formatText();
        }

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
                        if (i + j == i + numChar - 1 && j == numChar - 1)
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
}
