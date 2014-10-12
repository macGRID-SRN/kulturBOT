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
            Markov m = new Markov(144, 3);
            Console.WriteLine(m.formatText());
            Console.ReadKey();
        }
    }
}
