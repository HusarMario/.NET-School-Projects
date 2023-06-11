using PipeGame.Library;
using static System.Console;

namespace PipeGame.ConsoleApplication
{
    internal class Program
    {
        private static void Main()
        {
            #pragma warning disable CA1416
            SetWindowSize(LargestWindowWidth, LargestWindowHeight);
            SetBufferSize(LargestWindowWidth, LargestWindowHeight);
            #pragma warning restore CA1416

            var difficulty = Difficulty.Easy;
            var menu = Dictionary();
            Controls(menu, ref difficulty);
        }

        private static void Play(Difficulty difficulty)
        {
            Game game = new (difficulty);
            while (true)
            {
               if (game.Play() == false)
               {
                   break;
               }
            }
        }

        private static void Settings(ref Difficulty difficulty)
        {
            Clear();
            WriteLine("1 : Easy");
            WriteLine("2 : Medium");
            WriteLine("3 : Hard");
            difficulty = ReadLine() switch
            {
                "1" => Difficulty.Easy,
                "2" => Difficulty.Medium,
                "3" => Difficulty.Hard,
                _ => difficulty
            };
        }

        private static void Help()
        {
            Clear();
            WriteLine("To control the game your task is to connect starting points of pipes marked with '▲' with ending point marked with 'O'.");
            WriteLine("To do so you are able to rotate the directions of all remaining tubes in the game.");
            WriteLine("You need to ensure that finished pipe system is closed on every segment and does not leak any water.");
            WriteLine("You need to ensure that all starting points and all ending points are connected together in one pipe system.");
            WriteLine("Remaining tubes are not important as long as they are not wrongly connected with the system.");
            WriteLine("There is always at least 1 solution but there can be more!");
            WriteLine("To control movement in the game you only need arrow keys / 'w,s,a,d' keys to navigate, space to rotate and enter to confirm your solution.");
            WriteLine("You are scored with 100 points upon entering the level, for every rotation you are scored -1 point.");
            WriteLine("HighScores are separated according difficulty, but they all have the same scoring rules!");
            WriteLine("Thanks for playing and good luck.");
            WriteLine("Author : Mário Husár.");
            WriteLine();
            WriteLine("Press any key to return.");
            ReadKey();
        }

        private static void Score()
        {
            Clear();
            FileInfo file = new("Score.txt");
            StreamReader reader = new(file.FullName);
            while (!reader.EndOfStream)
            {
                WriteLine(reader.ReadLine());
            }
            reader.Close();

            WriteLine();
            WriteLine("Press any key to return.");
            ReadKey();
        }

        private static void Controls(Dictionary<int, string> menu, ref Difficulty difficulty)
        {
            var index = 1;
            while (true)
            {
                OutputMenu(menu, index);
                if (ControlMenu(ref index, ref difficulty)) return;
            }
        }

        private static bool ControlMenu(ref int index, ref Difficulty difficulty)
        {
            var input = ReadKey();
            

            switch (input.Key)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow when index == 1:
                    index = 5;
                    break;
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    index--;
                    break;
                case ConsoleKey.S or ConsoleKey.DownArrow when index == 5:
                    index = 1;
                    break;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    index++;
                    break;
                case ConsoleKey.Enter:
                    if (ApplyOption(index, ref difficulty)) return true;
                    break;
                case ConsoleKey.Escape:
                    Clear();
                    return true;
            }
            return false;
        }

        private static bool ApplyOption(int index, ref Difficulty difficulty)
        {
            switch (index)
            {
                case 1:
                    Play(difficulty);
                    break;
                case 2:
                    Settings(ref difficulty);
                    break;  
                case 3:
                    Help();
                    break;
                case 4:
                    Score();
                    break;
                case 5:
                    Clear();
                    return true;
            }

            return false;
        }

        private static void OutputMenu(Dictionary<int, string> menu, int index)
        {
            Clear();
            foreach (var key in menu.Keys)
            {
                ForegroundColor = key == index ? ConsoleColor.Blue : ConsoleColor.White;
                WriteLine(menu[key]);
                ForegroundColor = ConsoleColor.White;
            }
        }

        private static Dictionary<int, string> Dictionary()
        {
            var menu = new Dictionary<int, string>
            {
                { 1, "Play" },
                { 2, "Settings" },
                { 3, "Help" },
                { 4, "HighScore" },
                { 5, "Exit" }
            };
            return menu;
        }
    }
}
