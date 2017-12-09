using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _9
{
    class Program
    {
        class Group
        {
            public int score { get { return (parent?.score + 1) ?? 1; } }
            public Group parent;
            public List<Group> children = new List<Group>();
        }

        static void test(string text)
        {
            Group root = new Group();
            Group current = root;
            int i = 0;
            int gbc = 0;
            parse(text, ref i, ref current, ref gbc);

            root = root.children.First();
            root.parent = null;

            Console.WriteLine(allscore(root));
            Console.WriteLine(gbc);
        }

        static int allscore(Group root)
        {
            return root.score + root.children.Sum(allscore);
        }

        static void parse(string text, ref int i, ref Group current, ref int gbc)
        {
            for (; i < text.Length;)
            {
                char c = text[i];
                if (c == '!')
                {
                    i += 2;
                }
                else if (c == '<')
                {
                    for (int j = i + 1; j < text.Length; j++)
                    {
                        c = text[j];
                        if (c == '!')
                        {
                            j++;
                        }
                        else if (c == '>')
                        {
                            j++;
                            i = j;
                            break;
                        }
                        else
                        {
                            gbc++;
                        }
                    }
                }
                else if (c == '{')
                {
                    i++;
                    Group newGroup = new Group();
                    current.children.Add(newGroup);
                    newGroup.parent = current;
                    current = newGroup;
                    parse(text, ref i, ref current, ref gbc);
                }
                else if(c == '}')
                {
                    i++;
                    current = current.parent;
                }
                else if(c == ',')
                {
                    i++;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        static void Main(string[] args)
        {
            string text = File.ReadAllText("input.txt");
            test(text);
        }
    }
}
