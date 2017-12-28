using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18
{
    class Program
    {
        class VM
        {
            private Dictionary<char, long> registers = new Dictionary<char, long>();

            public long this[char reg]
            {
                get
                {
                    if (!registers.ContainsKey(reg))
                    {
                        registers.Add(reg, 0);
                    }
                    return registers[reg];
                }
                set
                {
                    if (!registers.ContainsKey(reg))
                    {
                        registers.Add(reg, 0);
                    }
                    registers[reg] = value;
                }
            }

            public long ip;
            public long lastPlayed;

            public bool didYield;
            public Action<VM>[] bytecode;

            public Action<long> Snd;
            public Action<char> Rcv;

            public Queue<long> rcvQueue = new Queue<long>();
            public Queue<long> sndQueue;

            public int sndCount;
            public int rcvCount;

            public bool isFinished
            {
                get
                {
                    return !(ip >= 0 && ip < bytecode.Length);
                }
            }

            public VM(int p, bool isSound)
            {
                this['p'] = p;
                if (isSound)
                {
                    Snd = playSound;
                    Rcv = recoverSound;
                }
                else
                {
                    Snd = send;
                    Rcv = receive;
                }
            }

            private void send(long value)
            {
                sndCount++;
                sndQueue.Enqueue(value);
            }
            private void receive(char dst)
            {
                if (rcvQueue.Any())
                {
                    rcvCount++;
                    this[dst] = rcvQueue.Dequeue();
                }
                else
                {
                    didYield = true;
                }
            }

            private void playSound(long src)
            {
                lastPlayed = src;
            }
            private void recoverSound(char src)
            {
                if (this[src] != 0)
                {
                    Console.WriteLine(lastPlayed);
                    // exit here
                    ip = -1;
                }
            }

            public void jump(long offset)
            {
                ip += offset;
            }

            public void run()
            {
                while (ip >= 0 && ip < bytecode.Length)
                {
                    step();
                }
            }
            public void step()
            {
                long tempIP = ip;
                didYield = false;
                bytecode[ip](this);
                if (tempIP == ip && !didYield)
                {
                    ip++;
                }
            }
        }

        static Func<VM, long> fetch(string value)
        {
            if (value.Length == 1 && !char.IsDigit(value.First()))
            {
                return (vm) => vm[value.First()];
            }
            return (vm) => int.Parse(value);
        }

        static Action<VM> set(char dst, Func<VM, long> src)
        {
            return (vm) => vm[dst] = src(vm);
        }

        static Action<VM> decode(string line)
        {
            int index1 = line.IndexOf(' ');
            int index2 = line.LastIndexOf(' ');

            string opcode = line.Substring(0, line.IndexOf(' '));
            string arg1 = null;
            if (index1 < index2)
            {
                arg1 = line.Substring(index1 + 1, index2 - (index1 + 1));
            }
            else
            {
                arg1 = line.Substring(index1 + 1);
            }
            string arg2 = null;
            if (index1 < index2)
            {
                arg2 = line.Substring(index2 + 1);
            }

            switch (opcode)
            {
                case "snd":
                    return (vm) => vm.Snd(fetch(arg1)(vm));
                case "set":
                    return set(arg1.First(), fetch(arg2));
                case "add":
                    return set(arg1.First(), (_vm) => _vm[arg1.First()] + fetch(arg2)(_vm));
                case "mul":
                    return set(arg1.First(), (_vm) => _vm[arg1.First()] * fetch(arg2)(_vm));
                case "mod":
                    return set(arg1.First(), (_vm) => _vm[arg1.First()] % fetch(arg2)(_vm));
                case "rcv":
                    return (vm) => vm.Rcv(arg1.First());
                case "jgz":
                    return (vm) =>
                    {
                        if (fetch(arg1)(vm) > 0)
                        {
                            vm.jump(fetch(arg2)(vm));
                        }
                    };
            }
            throw new InvalidOperationException();
        }

        static void test(string[] lines)
        {
            VM vm = new VM(0, true);
            vm.bytecode = lines.Select(decode).ToArray();

            vm.run();

            VM vm1 = new VM(0, false);
            VM vm2 = new VM(1, false);

            vm1.bytecode = vm.bytecode;
            vm2.bytecode = vm.bytecode;

            vm1.sndQueue = vm2.rcvQueue;
            vm2.sndQueue = vm1.rcvQueue;

            while ((!vm1.didYield || !vm2.didYield) && (!vm1.isFinished && !vm2.isFinished))
            {
                vm1.step();
                vm2.step();
            }

            Console.WriteLine(vm2.sndCount);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            test(lines);
        }
    }
}
