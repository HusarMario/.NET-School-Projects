using System.Text;

namespace Task1
{
    internal class MatrixRain
    {
        //nastavenie konzoly
        readonly int sirka;            
        readonly int vyska;
        readonly int prostrednyRiadok;   //riadok za ktory sa viac negeneruje 
        readonly char[,] priestor;
        readonly Random random;

        //nastavenie generovanych znakov
        public enum Characters
        {
            Alpha,
            Numeric,
            AlphaNumeric
        }
        char[] generation;
        int randomIndex;

        //nastavenie generovania a posun znakov
        int generujuciRiadok;   //riadok generovania znakov       
        int konecnyRiadok;      //riadok v ktorom sa pas straca
        int smer;               //zaporna alebo kladna jednotka pre pracu so smerom
        ConsoleColor color;
        int delay;

        public MatrixRain()
        {
            Console.Clear();
            Console.CursorVisible = false;
            sirka = Console.WindowWidth;
            vyska = Console.WindowHeight;
            priestor = new char[vyska,sirka];
            prostrednyRiadok = vyska / 2;

            for (int i = 0; i < vyska; i++)
            {
                for (int j = 0; j < sirka; j++)
                {
                    priestor[i, j] = ' ';
                }
            }

            random = new Random();
            generation = Array.Empty<char>();
            ChangeGeneration(Characters.AlphaNumeric);
            ChangedDirection(false);
            ChangeColor(ConsoleColor.Green);
            ChangeDelay(1);
        }

        

        public void Rain()
        {          
            System.Threading.Thread.Sleep(delay);
            for (int i = 0; i < sirka; i++)
            {               
                //generacia
                if (priestor[generujuciRiadok + smer,i] != ' ')             //pas je uz vytvoreny
                {
                    if (priestor[generujuciRiadok + 2*smer, i] == ' ')      //povinne generovanie aby bol retazec dlhy aspon 2 znaky
                    {
                        priestor[generujuciRiadok,i] = generation[random.Next(0,randomIndex)];
                        Console.SetCursorPosition(i, generujuciRiadok);
                        Console.ForegroundColor = color;
                        Console.Write(priestor[generujuciRiadok, i]);
                        Console.ResetColor();
                    } 
                    else                                                    //generovanie ak nie je presiahnuta dlzka po polovicu
                    {
                        for (int j = generujuciRiadok + 2*smer; j != prostrednyRiadok; j += smer)
                        {
                            if (priestor[j,i] == ' ')
                            {
                                if (random.Next(0, 100) < 90)               //generovanie dlhsieho retazca so sancou 90%
                                {
                                    priestor[generujuciRiadok, i] = generation[random.Next(0, randomIndex)];
                                    Console.SetCursorPosition(i, generujuciRiadok);
                                    Console.ForegroundColor = color;
                                    Console.Write(priestor[generujuciRiadok, i]);
                                    Console.ResetColor();
                                }
                                break;
                            }
                        }                    
                    }
                }
                else                                                        //pas este nie je vytvoreny         
                {
                    if (random.Next(0, 100) < 1)                            //generovanie noveho retazca so sancou 1%
                    {
                        priestor[generujuciRiadok, i] = generation[random.Next(0, randomIndex)];
                        Console.SetCursorPosition(i, generujuciRiadok);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(priestor[generujuciRiadok, i]);
                        Console.ResetColor();
                    }
                }


                //posun
                for (int j = konecnyRiadok; j != generujuciRiadok; j -= smer)
                {
                    bool firstWhite = true;     //zabezpecuje aby prvy prvok bol vzdy biely (normalne porovna miesto pod - nesmie na poslednom riadku)
                    if (j == konecnyRiadok)                                 //vymazavanie prvkov na poslednom riadku
                    {
                        if (priestor[j,i] == ' ')
                        {
                            firstWhite = true;
                        } else
                        {
                            firstWhite = false;
                            priestor[j, i] = ' ';
                            Console.SetCursorPosition(i, j);
                            Console.Write(priestor[j, i]);
                        }                       
                    }

                    if (priestor[j - smer, i] != ' ')                       //posun prvkov vzdy o jedno policko dalej
                    {
                        if (random.Next(0,100) < 10)                        //10% sanca na zmenu znaku
                        {
                            priestor[j, i] = generation[random.Next(0, randomIndex)];
                        } else
                        {
                            priestor[j, i] = priestor[j - smer, i];
                        }                        
                        priestor[j - smer, i] = ' ';

                        Console.SetCursorPosition(i, j - smer);
                        Console.ForegroundColor = color;
                        Console.Write(priestor[j - smer, i]);
                        Console.ResetColor();

                        if (j != konecnyRiadok)
                        {
                            if (priestor[j + smer, i] == ' ')               //kontrola sfarbenia prveho prvku pri pade
                            {
                                Console.SetCursorPosition(i, j);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(priestor[j, i]);
                                Console.ResetColor();
                            } else
                            {
                                Console.SetCursorPosition(i, j);
                                Console.ForegroundColor = color;
                                Console.Write(priestor[j, i]);
                                Console.ResetColor();
                            }
                        } else                                              //zabezpecnie sfarbenia prveho prvku pri poslednom riadku
                        {
                            if (firstWhite)
                            {
                                Console.SetCursorPosition(i, j);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(priestor[j, i]);
                                Console.ResetColor();
                            } else
                            {
                                Console.SetCursorPosition(i, j);
                                Console.ForegroundColor = color;
                                Console.Write(priestor[j, i]);
                                Console.ResetColor();
                            }                            
                        }             
                    }
                }
            }   
        }

        public static void Help()   //metoda pre vypisovanie helpu
        {
            Console.WriteLine("Description:");
            Console.WriteLine(" Matrix digital rain - a simplified version of the falling code of letters from the Matrix movie.");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine(" MatrixRain [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine($"{" --direction-up",-50} {"Direction of falling code up [default: False]"}");
            Console.WriteLine($"{" --color",-50} {"Color of falling code [default: Green]"}");

            StringBuilder colors = new();
            for (int i = 0; i < Enum.GetValues(typeof(ConsoleColor)).Length; i++)
            {
                if (i == 0)
                {
                    colors.Append('<');
                }
                colors.Append(Enum.GetValues(typeof(ConsoleColor)).GetValue(i));
                if (i != Enum.GetValues(typeof(ConsoleColor)).Length - 1)
                {
                    colors.Append('|');
                }
                if (i == Enum.GetValues(typeof(ConsoleColor)).Length - 1)
                {
                    colors.Append('>');
                }
            }
            string colorString = colors.ToString();
            for (int i = 0; i < colorString.Length; i += 50)
            {
                Console.WriteLine($"{colorString.Substring(i, Math.Min(50, (colorString.Length) - i)),-50}");
            }

            Console.WriteLine($"{" --delay-speed",-50} {"Delay speed of falling in miliseconds [default: 1]"}");
            Console.WriteLine($"{"<delay-speed>",-50}");
            Console.WriteLine($"{" --characters",-50} {"Set of characters generating falling code [default: AlphaNumeric]"}");

            StringBuilder characters = new();
            for (int i = 0; i < Enum.GetValues(typeof(Characters)).Length; i++)
            {
                if (i == 0)
                {
                    characters.Append('<');
                }
                characters.Append(Enum.GetValues(typeof(Characters)).GetValue(i));
                if (i != Enum.GetValues(typeof(Characters)).Length - 1)
                {
                    characters.Append('|');
                }
                if (i == Enum.GetValues(typeof(Characters)).Length - 1)
                {
                    characters.Append('>');
                }
            }
            string characterString = characters.ToString();
            for (int i = 0; i < characterString.Length; i += 50)
            {
                Console.WriteLine($"{characterString.Substring(i, Math.Min(50, (characterString.Length) - i)),-50}");
            }
        }

        public void ChangeDelay(int newDelay)
        {
            if (newDelay < 0)   //osetrenie zapornej hodnoty
            {
                delay = 1;
            }
            delay = newDelay;
        }
        public void ChangeColor(ConsoleColor newColor)
        {
            color = newColor;
        }
        public void ChangedDirection(bool switched)
        {
            if (switched)  //zdola nahor
            {
                generujuciRiadok = this.vyska - 1;
                konecnyRiadok = 0;
                smer = -1;
            }
            else          //zhora nadol
            {                
                generujuciRiadok = 0;
                konecnyRiadok = this.vyska - 1;
                smer = 1;
            }
        }

        public void ChangeGeneration(Characters characters)
        {
            switch (characters)    //vyber generovania znakov
            {
                case Characters.Alpha:
                    {
                        randomIndex = 0;
                        generation = new char[26];
                        for (int i = 65; i < 91; i++)
                        {
                            generation[randomIndex] = (char)i;
                            randomIndex++;
                        }
                        break;
                    }
                case Characters.Numeric:
                    {
                        randomIndex = 0;
                        generation = new char[10];
                        for (int i = 48; i < 58; i++)
                        {
                            generation[randomIndex] = (char)i;
                            randomIndex++;
                        }
                        break;
                    }
                case Characters.AlphaNumeric:
                    {
                        randomIndex = 0;
                        generation = new char[36];
                        for (int i = 48; i < 91; i++)
                        {
                            if (i < 58 || i >= 65)
                            {
                                generation[randomIndex] = (char)i;
                                randomIndex++;
                            }
                        }
                        break;
                    }
            }
        }
    }
}
