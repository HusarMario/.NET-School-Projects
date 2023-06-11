using System.Globalization;

namespace Uniza.Namedays.ViewerConsoleApp
{

    internal class Program
    {
        static void Main(string[] args)
        {

            NamedayCalendar calendar = new();

            if (args.Length != 0)
            {
                FileInfo file = new(args[0]);
                calendar.Load(file);
            }


            bool end;
            do
            {
                end = MainWindow(calendar);
            } while (!end);

        }

        static bool MainWindow(NamedayCalendar calendar)
        {
            Console.Clear();
            //Zaciatok vypisu
            Console.WriteLine("KALENDÁR MIEN");

            //Vypis dnesnych menin
            Console.Write($"Dnes {DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} ");
            if ((calendar[DateTime.Now].Length == 0) || (calendar[DateTime.Now][0] == " -"))
            {
                Console.WriteLine($"nemá meniny nikto.");
            }
            else
            {
                Console.Write($"má meniny:");
                foreach (var nameday in calendar[DateTime.Now])
                {
                    Console.Write($" {nameday}");
                }
                Console.WriteLine(".");
            }

            //Vypis zajtrajsich menin
            Console.Write($"Zajtra ");
            if ((calendar[DateTime.Now].Length == 0) || (calendar[DateTime.Now.AddDays(1)][0] == " -"))
            {
                Console.WriteLine($"nemá meniny nikto.");
            }
            else
            {
                Console.Write($"má meniny:");
                foreach (var nameday in calendar[DateTime.Now.AddDays(1)])
                {
                    Console.Write($" {nameday}");
                }
                Console.WriteLine(".");
            }

            //Zobrazenie menu
            Console.WriteLine();
            Console.WriteLine("Menu");
            Console.WriteLine("1. Načítať kalendár");
            Console.WriteLine("2. Zobraziť štatistiky");
            Console.WriteLine("3. Vyhľadať mená");
            Console.WriteLine("4. Vyhľadať mená podľa dátumu");
            Console.WriteLine("5. Zobraziť kalendár mien v mesiaci");
            Console.WriteLine("6. Koniec");

            //Vstup do aplikacie -> cita len cisla (ine znaky resetuju MainWindow)
            if (int.TryParse(Console.ReadLine(), out int value))
            {
                switch (value)
                {
                    case 1:
                        {
                            LoadCalendar(calendar);
                            break;
                        }

                    case 2:
                        {
                            Statistics(calendar);
                            break;
                        }
                    case 3:
                        {
                            NameByString(calendar);
                            break;
                        }
                    case 4:
                        {
                            NameByDate(calendar);
                            break;
                        }
                    case 5:
                        {
                            PrintCalendar(calendar);
                            break;
                        }
                    case 6: //Ukoncenie aplikacie
                        {
                            Console.Clear();
                            return true;
                        }
                }
            }
            return false;
        }

        static void LoadCalendar(NamedayCalendar calendar)
        {
            Console.Clear();
            Console.WriteLine("Zadajte cestu k súboru kalendára mien alebo stlačte Enter pre ukončenie.");
            while (true)
            {
                string? path = Console.ReadLine();
                //overenie existencie cesty k suboru
                if (File.Exists(path))
                {
                    //overenie koncovky k suboru
                    if (path.Substring(path.LastIndexOf('.'), 4).ToLower() == ".csv")
                    {
                        FileInfo file = new(path);
                        calendar.Load(file);
                        Console.WriteLine($"Súbor kalendára bol načítaný");
                        Console.WriteLine($"Pre pokračovanie stlačte Enter.");
                        string? input = Console.ReadLine();
                        if (input != null && input.Contains(""))
                        {
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Zadaný súbor {path} nie je typu CSV!");
                    }
                }
                else
                {
                    Console.WriteLine($"Zadaný súbor {path} neexistuje!");
                }
            }
        }

        static void Statistics(NamedayCalendar calendar)
        {
            Console.Clear();
            Console.WriteLine("ŠTATISTIKA");

            Dictionary<string, int> monthNameCount = new();
            SortedDictionary<string, int> letterNameCount = new();
            SortedDictionary<int, int> lengthNameCount = new();

            foreach (var nameday in calendar)
            {
                if(!monthNameCount.ContainsKey(nameday.DayMonth.Month.ToString().ToLower()))
                {
                    monthNameCount.Add(nameday.DayMonth.Month.ToString().ToLower(), 1);
                } else
                {
                    monthNameCount[nameday.DayMonth.Month.ToString().ToLower()]++;
                }

                if (!letterNameCount.ContainsKey(nameday.Name[..1]))
                {
                    letterNameCount.Add(nameday.Name[..1], 1);
                }
                else
                {
                    letterNameCount[nameday.Name[..1]]++;
                }

                if (!lengthNameCount.ContainsKey(nameday.Name.Length))
                {
                    lengthNameCount.Add(nameday.Name.Length, 1);
                }
                else
                {
                    lengthNameCount[nameday.Name.Length]++;
                }
            }

            Console.Write("Celkový počet mien v kalendári: ");
            Console.WriteLine(calendar.NameCount);

            Console.Write("Celkový počet dní obsahujúci mená v kalendári: ");
            Console.WriteLine(calendar.DayCount);

            Console.WriteLine("Celkový počet mien v jednotlivých mesiacoch: ");
            for(int i = 1; i < 13; i++)
            {
                Console.WriteLine($"    {new DateTime(2024, i, 1):MMMM}: {monthNameCount[i.ToString()]}");
            }
            
            Console.WriteLine("Počet mien podľa začiatočného písmena: ");
            foreach (var letter in letterNameCount.Keys)
            {
                Console.WriteLine($"    {letter}: {letterNameCount[letter]}");
            }

            Console.WriteLine("Počet mien podľa dĺžky znakov: ");
            foreach (var length in lengthNameCount.Keys)
            {
                Console.WriteLine($"    {length}: {lengthNameCount[length]}");
            }

            Console.WriteLine("Pre ukončenie stlačte Enter.");
            string? input = Console.ReadLine();
            if (input != null && input.Contains(""))
            {
                return;
            }
        }

        static void NameByString(NamedayCalendar calendar)
        {
            Console.Clear();
            Console.WriteLine("VYHĽADÁVANIE MIEN");
            Console.WriteLine("Pre ukočnenie stlačte enter.");

            while(true)
            {
                Console.WriteLine("Zadaj pattern: ");
                string? input = Console.ReadLine();
                if (input != null)
                {
                    if (input.Contains(""))
                    {
                        return;
                    }

                    var namedays = calendar.GetNamedays(input);
                    int i = 1;
                    if (!namedays.Any())
                    {
                        Console.WriteLine("Neboli nájdené žiadne mená.");
                        continue;
                    }
                    foreach (var nameday in namedays)
                    {
                        Console.WriteLine($"{i}: {nameday.Name} ({nameday.DayMonth.Day}.{nameday.DayMonth.Month}.)");
                        i++;
                    }
                }       
            }
        }

        static void NameByDate(NamedayCalendar calendar)
        {
            Console.Clear();
            Console.WriteLine("VYHĽADÁVANIE MIEN PODĽA DÁTUMU");
            Console.WriteLine("Pre ukočnenie stlačte enter.");

            while(true)
            {
                Console.WriteLine("Zadajte deň a mesiac: ");
                string? input = Console.ReadLine();

                if (input != null)
                {
                    if (input.Contains(""))
                    {
                        return;
                    }

                    var numbers = input.Split(".");
                    var day = int.Parse(numbers[0]);
                    var month = int.Parse(numbers[1]);
                    DayMonth dayMonth = new(day, month);

                    var namedays = calendar[dayMonth];
                    int i = 1;
                    if (namedays.Length == 0)
                    {
                        Console.WriteLine("Neboli nájdené žiadne mená.");
                        continue;
                    }
                    foreach (var name in namedays)
                    {
                        Console.WriteLine($"{i}: {name}");
                        i++;
                    }
                }
            }
        }

        static void PrintCalendar(NamedayCalendar calendar)
        {
            DateTime cas = DateTime.Today;
            while(true)
            {
                Console.Clear();
                Console.WriteLine("KALENDÁR MENÍN: ");
                Console.WriteLine(cas.ToString("MMMM") + " " + cas.Year);

                for (int i = 0; i < DateTime.DaysInMonth(cas.Year, cas.Month); i++)
                {
                    if (new DateTime(cas.Year, cas.Month, i + 1).DayOfWeek.ToString() == "Saturday" || new DateTime(cas.Year, cas.Month, i + 1).DayOfWeek.ToString() == "Sunday")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    if (i + 1 == cas.Day && DateTime.Now.Month == cas.Month && DateTime.Now.Year == cas.Year)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.Write($"{i + 1,2}.{cas.Month}. {new DateTime(cas.Year, cas.Month, i + 1):ddd}");
                    var names = calendar[i + 1, cas.Month];
                    bool prveMeno = true;
                    foreach (var name in names)
                    {
                        if (prveMeno)
                        {
                            Console.Write($" {name}");
                            prveMeno = false;
                        } else
                        {
                            Console.Write($", {name}");
                        }
                        
                    }
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine("Šípka doľava / doprava - mesiac dozadu / dopredu.");
                Console.WriteLine("Šípka dole / hore - rok dozadu / dopredu.");
                Console.WriteLine("Kláves Home alebo D - aktuálny deň.");
                Console.WriteLine("Pre ukončenie stlačte Enter.");
                while (true)
                {
                    var input = Console.ReadKey();
                    if (input.Key == ConsoleKey.Enter)
                    {
                        return;
                    }
                    if (input.Key == ConsoleKey.RightArrow)
                    {
                        cas = cas.AddMonths(1);
                        break;
                    }
                    if (input.Key == ConsoleKey.LeftArrow)
                    {
                        cas = cas.AddMonths(-1);
                        break;
                    }
                    if (input.Key == ConsoleKey.UpArrow)
                    {
                        cas = cas.AddYears(1);
                        break;
                    }
                    if (input.Key == ConsoleKey.DownArrow)
                    {
                        cas = cas .AddYears(-1);
                        break;
                    }
                    if (input.Key == ConsoleKey.Home || input.Key == ConsoleKey.D)
                    {
                        cas = DateTime.Now;
                        break;
                    }
                }
            }
            
        }

    }
}