namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MatrixRain matrixRain = new ();
            Console.Clear();
            bool help = false;                          //namiesto programu spusta help 

            for (int i = 0; i < args.Length; i++)       //prehladavanie moznych argumentov (ignoruje akykolvek chybny vstup)
            {
                string argument = args[i];

                if (args.Contains("--help"))            //help automaticky preskoci prehladavanie a zobrazi ponuku moznosti
                {
                    help = true;
                    break;
                }

                if (argument == "--direction-up")       //direction up sa automaticky zmeni ak sa najde v argumentoch
                {
                    matrixRain.ChangedDirection(true);
                    continue;
                }

                if (argument == "--color")              //farba sa zmeni len ak za argumentom nasleduje platny enum
                {
                    if (i != args.Length - 1)
                    {
                        if (Enum.IsDefined(typeof(ConsoleColor), args[i+1]))
                        {
                            matrixRain.ChangeColor((ConsoleColor)Enum.Parse(typeof(ConsoleColor), args[i + 1]));
                            i++;
                        }
                    }
                    continue;
                }

                if (argument == "--characters")         //generovanie sa zmeni len ak za argumentom nasleduje platny enum 
                {
                    if (i != args.Length - 1)
                    {
                        if (Enum.IsDefined(typeof(MatrixRain.Characters), args[i+1]))
                        {
                            matrixRain.ChangeGeneration((MatrixRain.Characters)Enum.Parse(typeof(MatrixRain.Characters), args[i+1]));
                            i++;
                        }
                    }
                    continue;
                }

                if (argument == "--delay-speed")        //rychlost sa zmeni len ak za argumentom nasleduje cislica
                {
                    if (i != args.Length - 1)
                    {
                        if (int.TryParse(args[i + 1], out int number))
                        {
                            matrixRain.ChangeDelay(number);
                        }
                    }
                    continue;
                }
            }

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    Console.ResetColor();
                    Console.CursorVisible = true;
                    Console.Clear();
                    break;
                }

                if (help)
                {
                    MatrixRain.Help();
                    Console.ReadKey(true);
                    Console.ResetColor();
                    Console.CursorVisible = true;
                    Console.Clear();
                    break;
                } else
                {
                    matrixRain.Rain();
                }
                             
            }
        }
    }
}   