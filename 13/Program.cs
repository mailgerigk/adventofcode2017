using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13
{
    class Program
    {
        class Scanner
        {
            public int depth;
            public int range;
            public int position;
            public bool down;

            public void move()
            {
                if(position == 0)
                {
                    down = true;
                }
                else if(position + 1 == range)
                {
                    down = false;
                }

                if (down)
                {
                    position++;
                }
                else
                {
                    position--;
                }
            }
        }

        static List<Scanner> clone(List<Scanner> scanners)
        {
            return new List<Scanner>(scanners.Select(s => new Scanner { depth = s.depth, down = s.down, position = s.position, range = s.range }));
        }

        static void step(List<Scanner> scanners)
        {
            scanners.ForEach(s => s.move());
        }

        static int sim(List<Scanner> scanners, out bool gotCaught)
        {
            int severity = 0;
            int maxDepth = scanners.Max(s => s.depth);
            gotCaught = false;

            for (int i = 0; i <= maxDepth; i++)
            {
                Scanner scanner = scanners.FirstOrDefault(s => s.depth == i);
                if (scanner != null)
                {
                    if (scanner.position == 0)
                    {
                        severity += i * scanner.range;
                        gotCaught = true;
                    }
                }

                step(scanners);
            }
            return severity;
        }

        static void test(string[] lines)
        {
            List<Scanner> scanners = new List<Scanner>();
            foreach (var line in lines)
            {
                string[] parts = line.Split(':');
                scanners.Add(new Scanner()
                {
                    depth = int.Parse(parts[0]),
                    range = int.Parse(parts[1].Trim())
                });
            }

            bool caught = false;
            int severity = sim(clone(scanners), out caught);
            int delay = 0;
            
            do
            {
                delay++;
                step(scanners);
            } while (sim(clone(scanners), out caught) != 0 || caught);

            Console.WriteLine(severity);
            Console.WriteLine(delay);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test(lines);
        }
    }
}
