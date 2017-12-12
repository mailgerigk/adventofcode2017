using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12
{
    class Program
    {
        class HashSetEQ : IEqualityComparer<HashSet<int>>
        {
            public bool Equals(HashSet<int> x, HashSet<int> y)
            {
                if(x.Count == y.Count)
                {
                    return x.Intersect(y).Count() == x.Count;
                }
                return false;
            }

            public int GetHashCode(HashSet<int> obj)
            {
                return obj.Count;
            }
        }

        static void test(string[] lines)
        {
            Dictionary<int, HashSet<int>> graph = new Dictionary<int, HashSet<int>>();
            foreach (var line in lines)
            {
                int input = int.Parse(line.Substring(0, line.IndexOf(' ')));
                int[] outputs = null;
                if (line.Contains(','))
                {
                    outputs = line.Substring(line.IndexOf('>') + 1).Split(',').Select(s => int.Parse(s.Trim())).ToArray();
                }
                else
                {
                    outputs = new int[] { int.Parse(line.Substring(line.LastIndexOf(' ')).Trim()) };
                }
                outputs = outputs.Concat(new int[] { input }).ToArray();

                for (int i = 0; i < outputs.Length; i++)
                {
                    HashSet<int> set;
                    if (!graph.TryGetValue(outputs[i], out set))
                    {
                        set = new HashSet<int>();
                        graph.Add(outputs[i], set);
                    }
                    for (int j = 0; j < outputs.Length; j++)
                    {
                        set.Add(outputs[j]);
                    }
                }
            }

            bool anyChange = true;
            while (anyChange)
            {
                anyChange = false;
                for (int i = 0; i < graph.Count; i++)
                {
                    HashSet<int> set = graph[graph.Keys.ElementAt(i)];
                    for (int j = 0; j < graph.Count; j++)
                    {
                        HashSet<int> set2 = graph[graph.Keys.ElementAt(j)];
                        if (set2.Contains(graph.Keys.ElementAt(i)))
                        {
                            foreach (var item in set)
                            {
                                anyChange |= set2.Add(item);
                            }
                        }
                    }
                }
            }

            int groupCount = 0;
            HashSet<HashSet<int>> groups = new HashSet<HashSet<int>>(new HashSetEQ());
            foreach (var item in graph)
            {
                if(groups.Add(item.Value))
                {
                    groupCount++;
                }
            }

            Console.WriteLine(graph[0].Count);
            Console.WriteLine(groupCount);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test(lines);
        }
    }
}
