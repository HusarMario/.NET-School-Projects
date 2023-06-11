namespace CV1
{
    internal class T02
    {
        public static void Run()
        {
            Console.WriteLine("Vypis kalendar na zaklade dnesneho dna :");
            var currentDay = new DateTime(2023, 3, 1); //DateTime.Today;
            var month = currentDay.Month;
            Console.WriteLine($"{currentDay:MMMM} {currentDay.Year}");
            while (currentDay.DayOfWeek != DayOfWeek.Monday)
            {
                currentDay = currentDay.AddDays(-1);
            }
            for (var i = 0; i < 7; i++)
            {
                Console.Write($"{currentDay:ddd} ");
                currentDay = currentDay.AddDays(1);
            }
            Console.WriteLine();
            currentDay = currentDay.AddDays(-7);
            while (true)
            {
                for (var i = 0; i < 7; i++)
                {
                    Console.Write($"{currentDay.Day,2} ");
                    currentDay = currentDay.AddDays(1);
                }
                Console.WriteLine();

                if (!currentDay.Month.Equals(month))
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Vypis ascii tabulku :");
            for (int i = 32; i < 127; i++)
            {
                var binary = Convert.ToInt32(Convert.ToString(i, 2));
                var octa = Convert.ToInt32(Convert.ToString(i, 8));
                Console.WriteLine($"{i,3} {binary,0:D7} {octa,0:D3} 0x{i:X} {(char)i}");
            }
        }
    }
}
