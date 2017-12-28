using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21
{
    class Program
    {
        class Block
        {
            public int size
            {
                get
                {
                    return values.GetLength(0);
                }
            }
            public int on
            {
                get
                {
                    int count = 0;
                    for (int h = 0; h < size; h++)
                    {
                        for (int w = 0; w < size; w++)
                        {
                            if(values[w,h])
                            {
                                count++;
                            }
                        }
                    }
                    return count;
                }
            }
            public bool[,] values;

            public Block[,] split(int blockSize)
            {
                int blockCount = size / blockSize;
                Block[,] blocks = new Block[blockCount, blockCount];
                for (int h = 0; h < blockCount; h++)
                {
                    for (int w = 0; w < blockCount; w++)
                    {
                        blocks[w, h] = Block.fromBlock(this, blockSize, w * blockSize, h * blockSize);
                    }
                }
                return blocks;
            }
            public Block rotateRight()
            {
                bool[,] otherValues = new bool[size, size];
                for (int h = 0; h < size; h++)
                {
                    for (int w = 0; w < size; w++)
                    {
                        otherValues[size - h - 1, w] = values[w, h];
                    }
                }
                return new Block { values = otherValues };
            }
            public Block flipVertical()
            {
                bool[,] otherValues = new bool[size, size];
                for (int h = 0; h < size; h++)
                {
                    for (int w = 0; w < size; w++)
                    {
                        otherValues[size - w - 1, h] = values[w, h];
                    }
                }
                return new Block { values = otherValues };
            }
            public bool equal(Block other)
            {
                if (size != other.size)
                {
                    return false;
                }

                for (int h = 0; h < size; h++)
                {
                    for (int w = 0; w < size; w++)
                    {
                        if (values[w, h] != other.values[w, h])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            public static Block fromString(string value)
            {
                string[] lines = value.Split('/');
                int size = lines.Length;
                bool[,] values = new bool[size, size];
                for (int h = 0; h < size; h++)
                {
                    for (int w = 0; w < size; w++)
                    {
                        values[w, h] = lines[h][w] == '#';
                    }
                }
                return new Block { values = values };
            }
            public static Block fromBlock(Block src, int size, int offsetX, int offsetY)
            {
                bool[,] values = new bool[size, size];
                for (int h = 0; h < size; h++)
                {
                    for (int w = 0; w < size; w++)
                    {
                        values[w, h] = src.values[offsetX + w, offsetY + h];
                    }
                }
                return new Block { values = values };
            }
            public static Block fromBlocks(Block[,] blocks)
            {
                int blockCount = blocks.GetLength(0);
                int blockSize = blocks[0, 0].size;
                bool[,] values = new bool[blockCount * blockSize, blockCount * blockSize];
                for (int hB = 0; hB < blockCount; hB++)
                {
                    for (int wB = 0; wB < blockCount; wB++)
                    {
                        for (int h = 0; h < blockSize; h++)
                        {
                            for (int w = 0; w < blockSize; w++)
                            {
                                values[wB * blockSize + w, hB * blockSize + h] = blocks[wB, hB].values[w, h];
                            }
                        }
                    }
                }
                return new Block { values = values };
            }

            public override string ToString()
            {
                string text = "";
                for (int h = 0; h < size; h++)
                {
                    for (int w = 0; w < size; w++)
                    {
                        text += values[w, h] ? '#' : '.';
                    }
                    if(h + 1 < size)
                    {
                        text += '/';
                    }
                }
                return text;
            }
        }

        class Cookbook
        {
            private Block[] srcBlocks;
            private Block[] dstBlocks;

            public Cookbook(string[] lines)
            {
                srcBlocks = new Block[lines.Length];
                dstBlocks = new Block[lines.Length];

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] parts = line.Split(' ');
                    string srcPart = parts.First();
                    string dstPart = parts.Last();

                    srcBlocks[i] = Block.fromString(srcPart);
                    dstBlocks[i] = Block.fromString(dstPart);
                }
            }

            public Block cookBlock(Block src)
            {
                Block[] variants = new Block[8];
                variants[0] = src;
                variants[1] = variants[0].rotateRight();
                variants[2] = variants[1].rotateRight();
                variants[3] = variants[2].rotateRight();
                variants[4] = src.flipVertical();
                variants[5] = variants[4].rotateRight();
                variants[6] = variants[5].rotateRight();
                variants[7] = variants[6].rotateRight();

                for (int i = 0; i < srcBlocks.Length; i++)
                {
                    for (int j = 0; j < variants.Length; j++)
                    {
                        if(srcBlocks[i].equal(variants[j]))
                        {
                            return dstBlocks[i];
                        }
                    }
                }
                throw new ArgumentException();
            }
            public Block cook(Block block)
            {
                Block[,] blocks = null;
                if((block.size % 2) == 0)
                {
                    blocks = block.split(2);
                }
                else if((block.size % 3) == 0)
                {
                    blocks = block.split(3);
                }

                for (int h = 0; h < blocks.GetLength(0); h++)
                {
                    for (int w = 0; w < blocks.GetLength(1); w++)
                    {
                        blocks[w, h] = cookBlock(blocks[w, h]);
                    }
                }

                return Block.fromBlocks(blocks);
            }
        }

        static void test(string[] lines)
        {
            Cookbook book = new Cookbook(lines);
            Block block = new Block
            {
                values = new bool[,]
                {
                    { false, true, false },
                    { false, false, true },
                    { true, true, true },
                },
            };

            for (int i = 0; i < 5; i++)
            {
                block = book.cook(block);
            }
            Console.WriteLine(block.on);

            // part2: really?...
            for (int i = 5; i < 18; i++)
            {
                block = book.cook(block);
            }
            Console.WriteLine(block.on);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test(lines);
        }
    }
}
