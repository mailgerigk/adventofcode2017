using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14
{
    class Program
    {
        static int countHex(char c)
        {
            switch (c)
            {
                case '0': return 0;
                case '1': return 1;
                case '2': return 1;
                case '3': return 2;
                case '4': return 1;
                case '5': return 2;
                case '6': return 2;
                case '7': return 3;
                case '8': return 1;
                case '9': return 2;
                case 'a': return 2;
                case 'b': return 3;
                case 'c': return 2;
                case 'd': return 3;
                case 'e': return 3;
                case 'f': return 4;
            }
            throw new ArgumentException();
        }

        static int countRegions(int[,] map)
        {
            int[,] region = new int[map.GetLength(0), map.GetLength(1)];
            int val = 0;
            for (int y = 0; y < region.GetLength(1); y++)
            {
                for (int x = 0; x < region.GetLength(0); x++)
                {
                    if (map[x, y] == 1)
                        region[x, y] = -1;
                    else
                        region[x, y] = -2;
                }
            }

            bool done = false;
            int px = 0;
            int py = 0;
            do
            {
                done = true;
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        if (region[x, y] == -1)
                        {
                            px = x;
                            py = y;
                            done = false;
                            goto foo;
                        }
                    }
                }
                foo:
                if (!done)
                {
                    countRegionsRec(map, region, px, py, val++);
                }
            } while (!done);

            return val;
        }

        static void countRegionsRec(int[,] map, int[,] region, int x, int y, int val)
        {
            if (map[x, y] == 1 && region[x, y] == -1)
            {
                region[x, y] = val;
            }
            else
            {
                return;
            }
            if (y - 1 >= 0)
            {
                countRegionsRec(map, region, x, y - 1, val);
            }
            if (y + 1 < map.GetLength(1))
            {
                countRegionsRec(map, region, x, y + 1, val);
            }
            if (x - 1 >= 0)
            {
                countRegionsRec(map, region, x - 1, y, val);
            }
            if (x + 1 < map.GetLength(0))
            {
                countRegionsRec(map, region, x + 1, y, val);
            }
        }

        static void test(string line)
        {
            int ones = 0;
            int[,] map = new int[128 * 8, 128];
            for (int i = 0; i < 128; i++)
            {
                string hashString = _10.Program.test2($"{line}-{i}");
                ones += hashString.Sum(c => countHex(c));

                ulong a = ulong.Parse(hashString.Substring(0, 16), System.Globalization.NumberStyles.HexNumber);
                ulong b = ulong.Parse(hashString.Substring(16, 16), System.Globalization.NumberStyles.HexNumber);
                for (int j = 0; j < 64; j++)
                {
                    map[j, i] = (int)((a >> (63 - j)) & 1);
                    map[j + 64, i] = (int)((b >> (63 - j)) & 1);
                }
            }

            Console.WriteLine(ones);
            Console.WriteLine(countRegions(map));
        }

        static void Main(string[] args)
        {
            string line = File.ReadAllText("input.txt");
            test(line);
        }
    }
}
