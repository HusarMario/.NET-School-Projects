namespace CV1
{
    internal class T04
    {
        public static void Run()
        {
            Console.Write("Zadajte cislo: ");
            if (int.TryParse(Console.ReadLine(), out int number))
                Console.WriteLine($"Konverzia prebehla uspesne - nacitane cislo je: {number}");
            else
                Console.WriteLine("Zadane cislo nie je cislo!");

            while (number > 0)
            {
                Console.Beep();
                Console.WriteLine($"{number}");
                number--;
            }

            Console.WriteLine("after");
        }
    }
}
