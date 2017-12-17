using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15
{
    class Program
    {
        static long generatorA(long prev)
        {
            return (prev * 16807) % 2147483647;
        }

        static long generatorB(long prev)
        {
            return (prev * 48271) % 2147483647;
        }

        static void test1(string[] lines)
        {
            long A = long.Parse(lines[0].Substring(lines[0].LastIndexOf(' ')));
            long B = long.Parse(lines[1].Substring(lines[1].LastIndexOf(' ')));

            int judge = 0;
            for (int i = 0; i < 40_000_000; i++)
            {
                A = generatorA(A);
                B = generatorB(B);

                if((A & 0xFFFF) == (B & 0xFFFF))
                {
                    judge++;
                }
            }

            Console.WriteLine(judge);
        }

        static void test2(string[] lines)
        {
            long A = long.Parse(lines[0].Substring(lines[0].LastIndexOf(' ')));
            long B = long.Parse(lines[1].Substring(lines[1].LastIndexOf(' ')));

            int judge = 0;
            for (int i = 0; i < 5_000_000; i++)
            {
                while (((A = generatorA(A)) % 4) != 0) ;
                while (((B = generatorB(B)) % 8) != 0) ;

                if ((A & 0xFFFF) == (B & 0xFFFF))
                {
                    judge++;
                }
            }

            Console.WriteLine(judge);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test1(lines);
            test2(lines);
        }
    }
}
