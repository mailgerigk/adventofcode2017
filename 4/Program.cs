using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4
{
    class Program
    {
        static void test1(string[] lines)
        {
            int valids = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] splits = lines[i].Split(' ');
                int j = 0;
                for (; j < splits.Length; j++)
                {
                    int k = 0;
                    for (; k < splits.Length; k++)
                    {
                        if (j != k && splits[j] == splits[k])
                        {
                            break;
                        }
                    }
                    if (k != splits.Length)
                    {
                        break;
                    }
                }
                if (j == splits.Length)
                {
                    valids++;
                }
            }

            Console.WriteLine($"{valids}");
        }

        static void test2(string[] lines)
        {
            int valids = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] splits = lines[i].Split(' ');
                int j = 0;
                for (; j < splits.Length; j++)
                {
                    int k = 0;
                    for (; k < splits.Length; k++)
                    {
                        if (j == k) continue;

                        if (splits[j].Length != splits[k].Length) continue;

                        var a = splits[j].ToList();
                        a.Sort();
                        var b = splits[k].ToList();
                        b.Sort();

                        int o = 0;
                        for (; o < a.Count; o++)
                        {
                            if(a[o] != b[o])
                            {
                                break;
                            }
                        }
                        if (o == a.Count)
                        {
                            break;
                        }
                    }
                    if (k != splits.Length)
                    {
                        break;
                    }
                }
                if (j == splits.Length)
                {
                    valids++;
                }
            }

            Console.WriteLine($"{valids}");
        }

        static void Main(string[] args)
        {
            string[] lines =  File.ReadAllLines("input.txt");
            test1(lines);
            test2(lines);
        }
    }
}
