namespace CV1
{
    internal class T03
    {
        public static void Run()
        {
            Console.WriteLine("Vypis ascii tabulku vstupnych znakov:");
            while (true)
            {
                var line = Console.ReadLine();
                for (int i = 0; i < line.Length; i++)
                {
                    char s = line[i];
                    var binary = Convert.ToInt32(Convert.ToString(s, 2));
                    var octa = Convert.ToInt32(Convert.ToString(s, 8));
                    Console.WriteLine($"{i,3} {binary,0:D7} {octa,0:D3} 0x{s:X} {s}");
                }
            }
        }
    }
}
