namespace CV1
{
    internal class T01
    {
        public static void Run()
        {
            Console.WriteLine("Vypis hello world :");
            Console.WriteLine("Hello, World!");

            Console.WriteLine("Vypis pozdrav na zaklade casu :");
            DateTime time = DateTime.Now;
            if (time.Hour >= 4 && time.Hour < 8)
            {
                Console.WriteLine("Dobre rano!");
            }
            else if (time.Hour >= 8 && time.Hour < 18)
            {
                Console.WriteLine("Dobry den!");
            }
            else if (time.Hour >= 18 && time.Hour < 22)
            {
                Console.WriteLine("Dobry vecer!");
            }
            else
            {
                Console.WriteLine("Dobru noc!");
            }
        }
    }
}
