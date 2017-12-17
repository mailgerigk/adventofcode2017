using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _16
{
    class Program
    {
        static void swap(char[] array, int a, int b)
        {
            char temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }

        static void test(string line)
        {
            string[] commands = line.Split(',');
            char[] programs = Enumerable.Range(0, 16).Select(i => (char)(i + 'a')).ToArray();
            List<string> cache = new List<string>();
            cache.Add(programs.Aggregate("", (a, c) => a + c));

            int iteration = 0;
            while (true)
            {
                iteration++;
                foreach (var command in commands)
                {
                    if (command.StartsWith("s"))
                    {
                        int count = int.Parse(command.Substring(1));
                        for (int i = 0; i < count; i++)
                        {
                            int index = programs.Length - count + i;
                            for (int j = index; j > i; j--)
                            {
                                swap(programs, j, j - 1);
                            }
                        }
                    }
                    else if (command.StartsWith("x"))
                    {
                        int[] indices = command.Substring(1).Split('/').Select(s => int.Parse(s)).ToArray();
                        swap(programs, indices[0], indices[1]);
                    }
                    else if (command.StartsWith("p"))
                    {
                        int[] indices = command
                            .Substring(1)
                            .Split('/')
                            .Select(s => s.First())
                            .Select(s => programs
                                .Select((n, i) => new { n, i })
                                .First(u => u.n == s).i)
                            .ToArray();
                        swap(programs, indices[0], indices[1]);
                    }
                }
                string order = programs.Aggregate("", (a, c) => a + c);
                if (cache.Count == 1)
                {
                    Console.WriteLine(order);
                }
                if(!cache.Contains(order))
                {
                    cache.Add(order);
                }
                else
                {
                    Console.WriteLine(cache[1_000_000_000 % iteration]);
                    break;
                }
            }

            
        }

        static void Main(string[] args)
        {
            string line = File.ReadAllText("input.txt");
            test(line);
        }
    }
}
