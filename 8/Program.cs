using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8
{
    class Program
    {
        static void test(string[] lines)
        {
            Dictionary<string, int> values = new Dictionary<string, int>();
            Dictionary<string, Func<string, int, bool>> ops = new Dictionary<string, Func<string, int, bool>>();
            ops["=="] = (n, a) => values[n] == a;
            ops["!="] = (n, a) => values[n] != a;
            ops["<="] = (n, a) => values[n] <= a;
            ops[">="] = (n, a) => values[n] >= a;
            ops[">"] = (n, a) => values[n] > a;
            ops["<"] = (n, a) => values[n] < a;
            int max = int.MinValue;

            foreach (var line in lines)
            {
                string[] splits = line.Split(' ');
                string name = splits[0];
                if(!values.ContainsKey(name))
                {
                    values.Add(name, 0);
                }
                string ifname = splits[4];
                if (!values.ContainsKey(ifname))
                {
                    values.Add(ifname, 0);
                }
            }

            foreach (var line in lines)
            {
                max = Math.Max(max, values.Values.Max());
                string[] splits = line.Split(' ');
                string name = splits[0];
                bool inc = splits[1] == "inc";
                int amount = int.Parse(splits[2]);
                string ifname = splits[4];
                string cmpop = splits[5];
                int cmpamount = int.Parse(splits[6]);

                if(ops[cmpop](ifname, cmpamount))
                {
                    values[name] += inc ? amount : -amount;
                }
            }

            Console.WriteLine(values.Values.Max());
            Console.WriteLine(max);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test(lines);
        }
    }
}
