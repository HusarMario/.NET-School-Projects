using PipeGame.Library;

namespace PipeGame.ConsoleApplication
{
    public class Game
    {
        private Board _board;
        private readonly Difficulty _difficulty;
        private bool Evaluation { get; set; }
        private int _score;
        private int _rowIndex;
        private int _colIndex;

        public Game(Difficulty difficulty)
        {
            _difficulty = difficulty;
            _score = 100;
            _rowIndex = 0;
            _colIndex = 0;
            Evaluation = false;

            _board = new Board(difficulty);
        }

        public bool Play()
        {
            Draw();
            return ControlGame();
        }

        private void ResetGame()
        {
            _board = new Board(_difficulty);
            _score += 100;
            Evaluation = false;
            _rowIndex = 0;
            _colIndex = 0;
        }

        private bool ControlGame()
        {
            var input = Console.ReadKey();
            switch (input.Key)
            {
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    SafeMove(ref _rowIndex, 0, _board.Rows - 1, 1);
                    break;
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    SafeMove(ref _rowIndex, _board.Rows-1, 0, -1);
                    break;
                case ConsoleKey.A or ConsoleKey.LeftArrow:
                    SafeMove(ref _colIndex, _board.Columns - 1, 0, -1);
                    break;
                case ConsoleKey.D or ConsoleKey.RightArrow:
                    SafeMove(ref _colIndex, 0, _board.Columns - 1, 1);
                    break;
                case ConsoleKey.Spacebar:
                    _board[_rowIndex,_colIndex].Rotate();
                    _score -= 2;
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    SaveScore();
                    return false;
                case ConsoleKey.Enter:
                    return CheckAnswer();
            }
            return true;
        }

        private void SaveScore()
        {
            FileInfo file = new("Score.txt");
            StreamReader reader = new(file.FullName);

            var index = _difficulty switch
            {
                Difficulty.Easy => 0,
                Difficulty.Medium => 4,
                Difficulty.Hard => 8,
                _ => 0
            };

            var values = new string?[12];
            var i = 0;
            while (!reader.EndOfStream)
            {
                values[i] = reader.ReadLine();
                i++;
            }
            reader.Close();

            if (int.Parse(values[index + 3]?.Split(' ')[2] ?? string.Empty) < _score)
            {
                if (int.Parse(values[index + 2]?.Split(' ')[2] ?? string.Empty) < _score)
                {
                    values[index + 3] = values[index + 2];
                    if (int.Parse(values[index + 1]?.Split(' ')[2] ?? string.Empty) < _score)
                    {
                        values[index + 2] = values[index + 1];
                        values[index + 1] = "1. "  + SignScore() + " " + _score ;
                    }
                    else
                    {
                        values[index + 2] = "2. " + SignScore() + " " + _score;
                    }
                }
                else
                {
                    values[index + 3] = "3. " + SignScore() + " " + _score;
                }
            }

            StreamWriter writer = new(file.FullName);
            foreach (var line in values)
            {
                writer.WriteLine(line);
            }
            writer.Close();
        }

        private static string? SignScore()
        {
            Console.WriteLine("Congratulations. You have achieved new record. Please input your name (10 characters).:");
            var name = Console.ReadLine();
            if (name?.Trim() == "")
            {
                name = "Default";
            }
            return name is { Length: > 10 } ? name[0..10] : name;
        }

        private bool CheckAnswer()
        {
            var result = _board.CheckAnswers();
            Evaluation = true;
            Draw();

            if (result)
            {
                Console.WriteLine("Correct!");
                ResetGame();
            }
            else
            {
                Console.WriteLine("Wrong answer sorry.");
                SaveScore();
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            
            return result;
        }

        private void SafeMove(ref int index, int min, int max, int movement)
        {
            while (true)
            {
                if (index == max)
                {
                    index = min;
                }
                else
                {
                    index += movement;
                }

                if (_board[_rowIndex, _colIndex].Pipe is TankPipe)
                {
                    continue;
                }

                break;
            }
        }

        private void Draw()
        {
            Console.Clear();

            Console.WriteLine($"Current score : {_score}");

            for (var i = 0; i < _board.Rows; i++)
            {
                for (var row = 0; row < 5; row++)
                {
                    for (var j = 0; j < _board.Columns; j++)
                    {
                        DrawSegment(row, _board, i, j);
                    }

                    Console.WriteLine();
                }
            }
        }

        private void DrawSegment(int row, Board board, int i, int j)
        {
            
            if (Evaluation)
            {
                if (board.GetCreatedAnswers().Contains(board[i, j]))
                {
                    Console.ForegroundColor = board[i, j].CorrectAnswer ? ConsoleColor.Green : ConsoleColor.Red;
                }
            }
            else
            {
                if (i == _rowIndex && j == _colIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
            }

            switch (row)
            {
                case 0:
                    Console.Write($"┌─{GiveOuterTube(board[i, j].Pipe, Direction.Up)}─┐");
                    break;
                case 1:
                    Console.Write($"│ {GiveInnerTube(board[i, j].Pipe, Direction.Up)} │");
                    break;
                case 2:
                    Console.Write(
                        $"{GiveOuterTube(board[i, j].Pipe, Direction.Left)}" +
                        $"{GiveInnerTube(board[i, j].Pipe, Direction.Left)}" +
                        $"{GivePipe(board[i, j].Pipe)/*board[i ,j]._connected.Count()*/}" +
                        $"{GiveInnerTube(board[i, j].Pipe, Direction.Right)}" +
                        $"{GiveOuterTube(board[i, j].Pipe, Direction.Right)}");
                    break;
                case 3:
                    Console.Write($"│ {GiveInnerTube(board[i, j].Pipe, Direction.Down)} │");
                    break;
                case 4:
                    Console.Write($"└─{GiveOuterTube(board[i, j].Pipe, Direction.Down)}─┘");
                    break;
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static char GivePipe(Pipe? pipe)
        {
            return pipe?.GetType() switch
            {
                { } t when t == typeof(NarrowPipe) => pipe.GetDirections().First() switch
                {
                    Direction.Up or Direction.Down => '║',
                    Direction.Right or Direction.Left => '═',
                    _ => ' ',
                },
                { } t when t == typeof(CornerPipe) => pipe.GetDirections().First() switch
                {
                    Direction.Up => '╚',
                    Direction.Right => '╔',
                    Direction.Down => '╗',
                    Direction.Left => '╝',
                    _ => ' ',
                },
                { } t when t == typeof(TriplePipe) => pipe.GetDirections().First() switch
                {
                    Direction.Up => '╠',
                    Direction.Right => '╦',
                    Direction.Down => '╣',
                    Direction.Left => '╩',
                    _ => ' ',
                },
                { } t when t == typeof(QuadPipe) => '╬',
                { } t when t == typeof(WaterFeed) => pipe.GetDirections().First() switch
                {
                    Direction.Up => '▲',
                    Direction.Right => '►',
                    Direction.Down => '▼',
                    Direction.Left => '◄',
                    _ => ' ',
                },
                { } t when t == typeof(WaterDrain) => 'O',
                _ => '⎕',
            };
        }

        private static char GiveOuterTube(Pipe? pipe, Direction direction)
        {
            if (pipe != null && pipe.HasDirection(direction))
            {
                return direction switch
                {
                    Direction.Up or Direction.Down => '║',
                    Direction.Left or Direction.Right => '═',
                    _ => ' ',
                };
            }
            return direction switch
            {
                Direction.Up or Direction.Down => '─',
                Direction.Left or Direction.Right => '│',
                _ => ' ',
            };
        }

        private static char GiveInnerTube(Pipe? pipe, Direction direction)
        {
            if (pipe != null && pipe.HasDirection(direction))
            {
                return direction switch
                {
                    Direction.Up or Direction.Down => '║',
                    Direction.Left or Direction.Right => '═',
                    _ => ' ',
                };
            }
            else
            {
                return ' ';
            }
        }
    }
}