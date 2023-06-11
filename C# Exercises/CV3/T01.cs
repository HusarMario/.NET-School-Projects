using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV3
{
    internal class T01
    {
        public static void Run()
        {

            string input = File.ReadAllText("numbers.txt");
            string[] numbers = input.Split('\n');
            int[] ints = new int[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                ints[i] = int.Parse(numbers[i]);
            }

            /*foreach (string number in numbers)
            {
                Console.WriteLine(number);
            }*/

            Console.WriteLine(numbers.Length);
            Console.WriteLine(numbers[0]);
            Console.WriteLine(numbers[5000]);
            Console.WriteLine(numbers[9999]);
            Console.WriteLine(numbers.Length ^ 0x01);
            Console.WriteLine(numbers[(numbers.Length ^ 0x01) - 2]); // rozlisovanie parne a neparne

            PrintStatistics(ints);
            Span<int> span = new(ints,0,300);
            for (int i = 0;i < span.Length;i++)
            {
                span[i] = 0;
            }
            PrintStatistics(ints);
            span = new(ints, 4000, 2001);
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = 500;
            }
            PrintStatistics(ints);
            span = ints.AsSpan(5000);
            PrintStatistics(span);
        }

        private static void PrintStatistics(ReadOnlySpan<int> numbers)
        {
            int sum = 0;
            double mean = 0.0;
            double variance = 0.0;

            for (int i = 0; i < numbers.Length;i++)
            {
                sum += numbers[i];
                mean += numbers[i];
            }
            mean /= numbers.Length;


            for (int i = 0;i < numbers.Length;i++) 
            {
                
                variance += Math.Pow(numbers[i] - mean, 2);
            }
            variance /= numbers.Length;

            Console.WriteLine($"Suma = {sum}");
            Console.WriteLine($"Priemer = {mean}");
            Console.WriteLine($"Rozptyl = {variance}");
            Console.WriteLine();
        }
    }
}
    