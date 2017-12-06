using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6
{
    class Program
    {
        class ArrayComp : IEqualityComparer<int[]>
        {
            public bool Equals(int[] x, int[] y)
            {
                if (x.Length != y.Length) return false;

                for (int i = 0; i < x.Length; i++)
                {
                    if(x[i] != y[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(int[] obj)
            {
                return 0;
            }
        }

        class ArrayTupleComp : IEqualityComparer<Tuple<int[], int>>
        {
            public bool Equals(Tuple<int[], int> x, Tuple<int[], int> y)
            {
                if (x.Item1.Length != y.Item1.Length) return false;

                for (int i = 0; i < x.Item1.Length; i++)
                {
                    if (x.Item1[i] != y.Item1[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(Tuple<int[], int> obj)
            {
                return 0;
            }
        }

        static void test1(string[] lines)
        {
            int i = 0;
            HashSet<int[]> foundSets = new HashSet<int[]>(new ArrayComp());

            List<int> currentSet = lines[0].Split('\t').Select(int.Parse).ToList();
            while(foundSets.Add(currentSet.ToArray()))
            {
                int max = currentSet.Max();
                int maxIndex = currentSet.IndexOf(max);
                currentSet[maxIndex] = 0;
                do
                {
                    maxIndex = (maxIndex + 1) % currentSet.Count;
                    currentSet[maxIndex]++;
                    max--;
                } while (max > 0);
                ++i;
            }

            Console.WriteLine(i);
        }

        static void test2(string[] lines)
        {
            int i = 0;
            HashSet<Tuple<int[], int>> foundSets = new HashSet<Tuple<int[], int>>(new ArrayTupleComp());

            List<int> currentSet = lines[0].Split('\t').Select(int.Parse).ToList();
            while (foundSets.Add(Tuple.Create(currentSet.ToArray(), i)))
            {
                int max = currentSet.Max();
                int maxIndex = currentSet.IndexOf(max);
                currentSet[maxIndex] = 0;
                do
                {
                    maxIndex = (maxIndex + 1) % currentSet.Count;
                    currentSet[maxIndex]++;
                    max--;
                } while (max > 0);
                ++i;
            }

            var tup = Tuple.Create(currentSet.ToArray(), i);
            Console.WriteLine(i - foundSets.Where(t => foundSets.Comparer.Equals(t, tup)).First().Item2);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test1(lines);
            test2(lines);
        }
    }
}
