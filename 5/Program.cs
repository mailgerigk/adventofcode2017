using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5
{
    class Program
    {
        static void test1(string[] lines)
        {
            int steps = 0;
            int offset = 0;
            int[] offsets = lines.Select(int.Parse).ToArray();

            while(offset >= 0 && offset < offsets.Length)
            {
                steps++;
                offset += offsets[offset]++;
            }

            Console.WriteLine(steps);
        }

        static void test2(string[] lines)
        {
            int steps = 0;
            int offset = 0;
            int[] offsets = lines.Select(int.Parse).ToArray();

            while (offset >= 0 && offset < offsets.Length)
            {
                steps++;
                int relative = offsets[offset];
                if (relative >= 3)
                {
                    offsets[offset]--;
                }
                else
                {
                    offsets[offset]++;
                }
                offset += relative;
            }

            Console.WriteLine(steps);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test1(lines);
            test2(lines);
        }
    }
}
