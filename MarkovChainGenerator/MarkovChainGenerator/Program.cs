using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovChainGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //First integer sets character limit, second integer sets chain size (bigger number increases context)
            Markov m = new Markov(144, 3);
            for (int i = 0; i < 11; i++)
            {
                Console.WriteLine(m.formatText());
            }
            Console.ReadKey();
        }
    }
}
