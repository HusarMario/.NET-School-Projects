namespace PipeGame.Library
{
    public enum Direction
    {
        Up, Right, Down, Left
    }

    public static class DirectionCommands
    {
        public static void Rotate(ref Direction direction)
        {
            direction = direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => direction
            };
        }

        public static Direction Opposite(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                _ => direction
            };
        }
    }
    
}
