using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3
{
    class Program
    {
        struct P2
        {
            public int X, Y;

            public static P2 operator +(P2 a, P2 b)
            {
                return new P2
                {
                    X = a.X + b.X,
                    Y = a.Y + b.Y,
                };
            }

            public static P2 operator * (P2 a, int b)
            {
                return new P2
                {
                    X = a.X * b,
                    Y = a.Y * b,
                };
            }
        }

        const int input = 325489;
        // part 2: https://oeis.org/A141481/b141481.txt

        static void Main(string[] args)
        {
            P2[] offsets = new[]
            {
                new P2 { X =  1, Y =  0 },
                new P2 { X =  0, Y =  1 },
                new P2 { X = -1, Y =  0 },
                new P2 { X =  0, Y = -1 },
            };
            P2 current = new P2();
            int offsetIndex = 0;
            int scalar = 0;
            int acc = 1;
            int num = 1;

            while(num != input)
            {
                switch (offsetIndex)
                {
                    case 0:
                        scalar++;
                        break;
                    case 2:
                        scalar++;
                        break;
                }
                acc = scalar;
                while (acc > 0)
                {
                    num++;
                    acc--;
                    current += offsets[offsetIndex];
                    if(num == input)
                    {
                        break;
                    }
                }
                offsetIndex = (offsetIndex + 1) % offsets.Length;
            }

            Console.WriteLine($"{current.X} :: {current.Y}");
            Console.WriteLine($"{Math.Abs(current.X) + Math.Abs(current.Y)}");
        }
    }
}
