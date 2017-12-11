using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11
{
    class Program
    {
        class Vector
        {
            public float X, Y, Z;

            public Vector(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public float CubeDistance
            {
                get
                {
                    return (float)((Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z)) / 2);
                }
            }

            public static Vector operator + (Vector a, Vector b)
            {
                return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
            }
        }

        enum CardinalDirection
        {
            N,
            NE,
            NW,
            S,
            SE,
            SW,
        }

        static void test(string line)
        {
            List<CardinalDirection> directions = line
                .Split(',')
                .Select(str => (CardinalDirection)Enum.Parse(typeof(CardinalDirection), str.ToUpper()))
                .ToList();

            Dictionary<CardinalDirection, Vector> directionVectors = new Dictionary<CardinalDirection, Vector>();
            directionVectors.Add(CardinalDirection.N, new Vector(-1, 1, 0));
            directionVectors.Add(CardinalDirection.S, new Vector(1, -1, 0));

            directionVectors.Add(CardinalDirection.NE, new Vector(0, 1, -1));
            directionVectors.Add(CardinalDirection.NW, new Vector(-1, 0, 1));

            directionVectors.Add(CardinalDirection.SE, new Vector(1, 0, -1));
            directionVectors.Add(CardinalDirection.SW, new Vector(0, -1, 1));

            Vector point = new Vector(0, 0, 0);
            float max = float.MinValue;
            foreach (var direction in directions)
            {
                point += directionVectors[direction];
                max = Math.Max(max, point.CubeDistance);
            }

            Console.WriteLine(point.CubeDistance);
            Console.WriteLine(max);
        }

        static void Main(string[] args)
        {
            string line = File.ReadAllText("input.txt");
            test(line);
        }
    }
}
