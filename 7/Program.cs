using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7
{
    class Program
    {
        class Node
        {
            public int weight;
            public string name;
            public string[] children;
        }

        static void lineToNode(string line, List<Node> nodes)
        {
            string name = line.Substring(0, line.IndexOf(' '));
            int weight = int.Parse(line.Substring(line.IndexOf('(')+1, line.IndexOf(')') - 1 - line.IndexOf('(')));
            string[] children = new string[0];
            if (line.Contains('>'))
            {
                children = line.Substring(line.IndexOf('>') + 1).Split(',').Select(s => s.Trim()).ToArray();
            }
            nodes.Add(new Node { name = name, children = children, weight = weight });
        }

        static Node findRoot(List<Node> nodes)
        {
            return nodes.Where(n => nodes.All(o => !o.children.Contains(n.name))).First();
        }

        static Node findNode(string name, List<Node> nodes)
        {
            return nodes.Where(n => n.name == name).First();
        }

        static Node getDeepest(List<Node> nodes)
        {
            Node root = findRoot(nodes);
            Tuple<Node, int> bottom = getDeepestRec(0, root, nodes);
            return bottom.Item1;
        }

        static Tuple<Node, int> getDeepestRec(int depth, Node node, List<Node> nodes)
        {
            Tuple<Node, int> ret = Tuple.Create(node, depth);
            foreach (var child in node.children)
            {
                Tuple<Node, int> childRet = getDeepestRec(depth + 1, node, nodes);
                if(ret.Item2 < childRet.Item2)
                {
                    ret = childRet;
                }
            }
            return ret;
        }

        static int totalWeight(string name, List<Node> nodes)
        {
            Node node = findNode(name, nodes);
            int total = node.weight;
            foreach (var child in node.children)
            {
                total += totalWeight(child, nodes);
            }
            return total;
        }

        static void findUnbalancedParentRec(Node parent, Node node, List<Node> nodes)
        {
            bool pB = isBalanced(node.name, nodes);
            bool allChildren = node.children.All(c => isBalanced(c, nodes));
            if(!pB && allChildren)
            {
                int[] weights = node.children.Select(n => totalWeight(n, nodes)).ToArray();
                int wrongWeight = weights.Where(w => weights.Count(w2 => w == w2) == 1).First();
                int i = 0;
                for (; i < weights.Length; i++)
                {
                    if (weights[i] == wrongWeight) break;
                }
                int expWeight = totalWeight(node.children.First(n => n != node.children[i]), nodes);
                int actWeight = totalWeight(node.children[i], nodes);
                int newWeight = expWeight - (actWeight - findNode(node.children[i], nodes).weight);
                Console.WriteLine(newWeight);
            }
            else
            {
                foreach (var child in node.children)
                {
                    findUnbalancedParentRec(node, findNode(child, nodes), nodes);
                }
            }
        }

        static bool isBalanced(string name, List<Node> nodes)
        {
            int[] weights = findNode(name, nodes).children.Select(n => totalWeight(n, nodes)).ToArray();
            return weights.All(i => weights.All(i2 => i == i2));
        }

        static void test1(string[] lines)
        {
            List<Node> nodes = new List<Node>();
            foreach (var line in lines)
            {
                lineToNode(line, nodes);
            }

            Node root = findRoot(nodes);
            Console.WriteLine(root.name);
        }

        static void test2(string[] lines)
        {
            List<Node> nodes = new List<Node>();
            foreach (var line in lines)
            {
                lineToNode(line, nodes);
            }

            Node root = findRoot(nodes);
            foreach (var child in root.children)
            {
                findUnbalancedParentRec(root, findNode(child, nodes), nodes);
            }
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test1(lines);
            test2(lines);
        }
    }
}
