namespace PipeGame.Library
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public class DifficultyParameters
    {
        public int Rows { get; init; }
        public int Columns { get; init; }
        public int Tubes { get; init; }

        public DifficultyParameters(int rows, int columns, int tubes)
        {
            Rows = rows;
            Columns = columns;
            Tubes = tubes;
        }
    }

    public static class DifficultyCommands
    {
        private static readonly Dictionary<Difficulty, DifficultyParameters> Parameters =
            new()
            {
                { Difficulty.Easy, new DifficultyParameters(4, 4, 1) },
                { Difficulty.Medium, new DifficultyParameters(6, 8, 2) },
                { Difficulty.Hard, new DifficultyParameters(8, 12, 3) }
            };

        public static DifficultyParameters GetParameters(Difficulty difficulty)
        {
            return Parameters[difficulty];
        }
    }
}
