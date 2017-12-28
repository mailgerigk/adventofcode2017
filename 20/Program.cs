using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20
{
    class Program
    {
        class Vector3
        {
            public long X, Y, Z;

            public long Distance
            {
                get
                {
                    return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
                }
            }

            public static Vector3 operator +(Vector3 a, Vector3 b)
            {
                return new Vector3
                {
                    X = a.X + b.X,
                    Y = a.Y + b.Y,
                    Z = a.Z + b.Z
                };
            }

            public static bool operator ==(Vector3 a, Vector3 b)
            {
                return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
            }
            public static bool operator !=(Vector3 a, Vector3 b)
            {
                return !(a == b);
            }
        }

        class Particle
        {
            public Vector3 position;
            public Vector3 velocity;
            public Vector3 acceleration;

            public void update()
            {
                velocity += acceleration;
                position += velocity;
            }

            public static bool operator ==(Particle a, Particle b)
            {
                return a.position == b.position;
            }
            public static bool operator !=(Particle a, Particle b)
            {
                return !(a == b);
            }
        }

        static Vector3 parseVector3(string line)
        {
            long[] values = line.Split(',').Select(part => long.Parse(part)).ToArray();
            return new Vector3
            {
                X = values[0],
                Y = values[1],
                Z = values[2],
            };
        }

        static Particle parseParticle(string line)
        {
            string[] parts = line.Split(' ').Select(part => part.Trim()).ToArray();
            parts[0] = parts[0].Substring(parts[0].IndexOf('<') + 1, parts[0].LastIndexOf('>') - parts[0].IndexOf('<') - 1);
            parts[1] = parts[1].Substring(parts[1].IndexOf('<') + 1, parts[1].LastIndexOf('>') - parts[1].IndexOf('<') - 1);
            parts[2] = parts[2].Substring(parts[2].IndexOf('<') + 1, parts[2].LastIndexOf('>') - parts[2].IndexOf('<') - 1);

            return new Particle
            {
                position = parseVector3(parts[0]),
                velocity = parseVector3(parts[1]),
                acceleration = parseVector3(parts[2]),
            };
        }

        static void test(string[] lines)
        {
            Particle[] particles = lines.Select(parseParticle).ToArray();

            for (int j = 0; j < 100_000; j++)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].update();
                }
            }

            Particle min = particles.Where(p => particles.All(p2 => p.position.Distance <= p2.position.Distance)).First();
            int num = particles.Select((p, _i) => new { val = p == min, i = _i }).First(p => p.val).i;
            Console.WriteLine(num);

            List<Particle> particleList = new List<Particle>(lines.Select(parseParticle));
            for (int j = 0; j < 1_000; j++)
            {
                for (int i = 0; i < particleList.Count; i++)
                {
                    particleList[i].update();
                }

                List<Particle> collisions = new List<Particle>();
                for (int i = 0; i < particleList.Count; i++)
                {
                    for (int k = 0; k < particleList.Count; k++)
                    {
                        if (i == k) continue;

                        if(particleList[i] == particleList[k])
                        {
                            collisions.Add(particleList[i]);
                        }
                    }
                }

                collisions.ForEach(p => particleList.Remove(p));
            }

            Console.WriteLine(particleList.Count);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test(lines);
        }
    }
}
