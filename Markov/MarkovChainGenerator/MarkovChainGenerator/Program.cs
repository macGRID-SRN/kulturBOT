using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Remoting.Channels;

namespace MarkovChainGenerator
{
    class Program
    {
        private const int NUM_FILES_TO_GEN = 20000;
        private const int MARKOV_CHAR_LEN = 6;
        private const int POEM_CHAR_LEN = 400;

        static void Main(string[] args)
        {
            //First integer sets character limit, second integer sets chain size (bigger number increases context)
            Markov myMarkov = new Markov(POEM_CHAR_LEN, MARKOV_CHAR_LEN);

            for (int i = 0; i < NUM_FILES_TO_GEN; i++)
            {
                List<string> outText = new List<string>();

                outText.Add(WordWrap(myMarkov.formatText(), outText));



                File.WriteAllLines(string.Format(@"out\{0}.txt", i), outText);
            }

            Console.ReadKey();
        }

        public static char[] cars = new char[] { '.', ';', ',' };

        public static string WordWrap(string line, List<string> outList)
        {
            line = cars.Aggregate(line, (current, tire) => current.Replace(tire.ToString() + ' ', tire + _newline));

            return WordWrap(line, 30);
        }


        /*
        THE FOLLOWING IS ADAPTED FROM http://stackoverflow.com/a/3961365
        */
        protected const string _newline = "\n";

        public static string WordWrap(string the_string, int width)
        {
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return the_string;

            // Parse each line of text
            for (pos = 0; pos < the_string.Length; pos = next)
            {
                // Find end of line
                int eol = the_string.IndexOf(_newline, pos);

                if (eol == -1)
                    next = eol = the_string.Length;
                else
                    next = eol + _newline.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;

                        if (len > width)
                            len = BreakLine(the_string, pos, width);

                        sb.Append(the_string, pos, len);
                        sb.Append(_newline);

                        // Trim whitespace following break
                        pos += len;

                        while (pos < eol && Char.IsWhiteSpace(the_string[pos]))
                            pos++;

                    } while (eol > pos);
                }
                else sb.Append(_newline); // Empty line
            }

            return sb.ToString();
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        public static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max - 1;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;
            if (i < 0)
                return max; // No whitespace found; break at maximum length
            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;
            // Return length of text before whitespace
            return i + 1;
        }
    }
}
