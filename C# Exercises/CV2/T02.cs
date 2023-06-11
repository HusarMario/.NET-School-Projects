using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV2
{
    internal class T02
    {
        public static void Run(string[] args)
        {
            Boolean indexing = false;
            if (args.Contains("-n") || args.Contains("--number"))
            {
                indexing = true;
            }

            String s_occurance = "";
            if (args.Contains("-s") || args.Contains("--search"))
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Equals("-s") || args[i].Equals("--search"))
                    {
                        if (i + 1 != args.Length)
                        {
                            s_occurance = args[i + 1].Substring(1, args[i + 1].Length - 1);
                        }
                    }
                }
            }


            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-n") || args[i].Equals("--number"))
                {
                    continue;
                }

                if (args[i].Equals("-s") || args[i].Equals("--search"))
                {
                    i++;
                    continue;
                }

                Console.WriteLine(args[i]);
                if (File.Exists(args[i]))
                {
                    int indexCount = 1;
                    foreach (var line in File.ReadAllLines(args[i]))
                    {
                        if (line.Contains(s_occurance))
                        {
                            Console.WriteLine($"{indexCount}    {line}");
                        }
                        indexCount++;
                    }
                }
                else
                {
                    Console.WriteLine(args[i] + ": No such file or directory");
                }
                Console.WriteLine();
            }
        }
    }
}
