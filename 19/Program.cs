using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19
{
    class Program
    {
        static void nextDir(char[][] map, int posX, int posY, ref int dirX, ref int dirY)
        {
            if(dirX != 0)
            {
                dirX = 0;
                if(posY + 1 < map[posX].Length && map[posX][posY + 1] != ' ')
                {
                    dirY = 1;
                }
                else
                {
                    dirY = -1;
                }
            }
            else
            {
                dirY = 0;
                if(posX + 1 < map.Length && map[posX + 1][posY] != ' ')
                {
                    dirX = 1;
                }
                else
                {
                    dirX = -1;
                }
            }
        }

        static void test(string[] lines)
        {
            int width = lines.Max(line => line.Length);
            char[][] map = new char[width][];
            for (int i = 0; i < width; i++)
            {
                map[i] = new char[lines.Length];
                for (int j = 0; j < lines.Length; j++)
                {
                    map[i][j] = lines[j][i];
                }
            }

            int posX = lines.First().IndexOf('|');
            int posY = 0;

            int dirX = 0;
            int dirY = 1;

            string text = "";
            int steps = 0;
            while (true)
            {
                while(posX + dirX < width && posY + dirY < lines.Length && map[posX + dirX][posY + dirY] != ' ')
                {
                    if(!"|-+".Contains(map[posX][posY]))
                    {
                        text += map[posX][posY];
                    }

                    posX += dirX;
                    posY += dirY;
                    steps++;
                }

                nextDir(map, posX, posY, ref dirX, ref dirY);
                if(map[posX + dirX][posY + dirY] == ' ')
                {
                    break;
                }
            }

            if (!"|-+".Contains(map[posX][posY]))
            {
                text += map[posX][posY];
            }
            steps++;

            Console.WriteLine(text);
            Console.WriteLine(steps);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test(lines);
        }
    }
}
