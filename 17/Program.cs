using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17
{
    class Program
    {
        static void test(string line)
        {
            int offset = 0;
            int steps = int.Parse(line);
            LinkedList<int> spinlock = new LinkedList<int>();
            
            spinlock.AddFirst(0);
            LinkedListNode<int> node = spinlock.First;
            for (int i = 1; i <= 2017; i++)
            {
                int newoffset = (offset + steps) % spinlock.Count;
                if(newoffset < offset)
                {
                    node = node.List.First;
                    offset = 0;
                }

                for (int j = offset; j < newoffset; j++)
                {
                    node = node.Next;
                }

                spinlock.AddAfter(node, i);

                node = node.Next;
                offset = newoffset + 1;
            }

            Console.WriteLine(spinlock.Find(2017).Next.Value);

            int afterZero = spinlock.Find(0).Next.Value;
            int count = spinlock.Count;
            for (int i = 2018; i <= 50_000_000; i++)
            {
                int newoffset = (offset + steps) % count;
                if(newoffset == 0)
                {
                    afterZero = i;
                }
                offset = newoffset + 1;
                count++;
            }

            Console.WriteLine(afterZero);
        }

        static void Main(string[] args)
        {
            string line = File.ReadAllText("input.txt");
            test(line);
        }
    }
}
