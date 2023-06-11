using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV2
{
    internal class T01
    {
        public static void Run(string[] args)
        {
            Array.Sort(args);

            int counter = 1;
            foreach (var item in args)
            {
                Console.WriteLine(counter + ": " + item.Length + " " + item);
                counter++;
            }

            Console.WriteLine();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in args)
            {
                stringBuilder.Append(item);
            }
            Console.WriteLine(stringBuilder.ToString());
        }
    }
}
