using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MarkovChainGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //First integer sets character limit, second integer sets chain size (bigger number increases context)
            Markov m;
            StreamWriter sw = new StreamWriter(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\markovs\\markov.txt");
            for (int i = 2; i < 7; i++)
            {
                m = new Markov(144, i);                
                sw.WriteLine(m.formatText() + " //chain length: " + i);

                sw.Flush();
                
            }
            sw.Close();
            Console.ReadKey();
        }
    }
}
