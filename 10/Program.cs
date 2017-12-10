using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10
{
    class Program
    {
        static void reverse(int[] list, int start, int length)
        {
            int mid = (length + 1) / 2;
            for (int i = 0; i < mid; i++)
            {
                int index = (i + start) % list.Length;
                int counterIndex = (((length - 1) - i) + start) % list.Length;

                int temp = list[index];
                list[index] = list[counterIndex];
                list[counterIndex] = temp;
            }
        }

        static void knothashround(int[] list, int[] lengths, ref int currentPosition, ref int skipSize)
        {
            foreach (var length in lengths)
            {
                reverse(list, currentPosition, length);
                currentPosition += length + skipSize;
                skipSize++;
                currentPosition = currentPosition % list.Length;
            }
        }

        static void test1(string text)
        {
            int[] list = new int[256];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = i;
            }

            int skipSize = 0;
            int currentPosition = 0;
            int[] lengths = text
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            knothashround(list, lengths, ref currentPosition, ref skipSize);

            Console.WriteLine(list[0] * list[1]);
        }

        static void test2(string text)
        {
            int[] list = new int[256];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = i;
            }

            int skipSize = 0;
            int currentPosition = 0;

            int[] suffix = new[] { 17, 31, 73, 47, 23 };
            int[] lengths = text.Select(c => (int)c).Concat(suffix).ToArray();

            for (int i = 0; i < 64; i++)
            {
                knothashround(list, lengths, ref currentPosition, ref skipSize);
            }

            int[] hash = new int[16];
            for (int i = 0; i < hash.Length; i++)
            {
                int value = 0;
                for (int j = 0; j < hash.Length; j++)
                {
                    value ^= list[i * 16 + j];
                }
                hash[i] = value;
            }

            string hashString = hash.Aggregate("", (acc, v) => acc + v.ToString("X2")).ToLower();

            Console.WriteLine(hashString);
        }

        static void Main(string[] args)
        {
            string text = File.ReadAllText("input.txt");
            test1(text);
            test2(text);
        }
    }
}
