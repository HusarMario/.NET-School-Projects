namespace PipeGame.Library
{
    public class Segment
    {
        private readonly Dictionary<Direction, Segment?> _surroundings; 
        private readonly List<Segment?> _connected;
        public Pipe? Pipe { get; private set; }
        internal bool Visited { get; set; }
        public bool CorrectAnswer { get; internal set; }

        public Segment()
        {
            _surroundings = new Dictionary<Direction, Segment?>
            {
                { Direction.Up, null },
                { Direction.Right, null },
                { Direction.Down, null },
                { Direction.Left, null }
            };
            _connected = new List<Segment?>();
            Pipe = null;
            Visited = false;
        }

        public void Rotate()
        {
            Pipe?.Rotate();
        }

        internal Segment? AccessSurrounding(Direction direction)
        {
            return _surroundings[direction];
        }

        internal void AssignSegment(Direction direction, Segment segment)
        {
            _surroundings[direction] = segment;
        }

        internal void ConnectSegment(Segment segment)
        {
            if (!_connected.Contains(segment))
            {
                _connected.Add(segment);
            }    
        }

        internal void AssignTank(int type, Direction direction)
        {
            Pipe = type switch
            {
                0 => new WaterFeed(direction),
                1 => new WaterDrain(direction),
                _ => throw new ArgumentException("Invalid type of tank"),
            };
        }

        internal int GetConnectedSize()
        {
            return _connected.Count;
        }

        internal void AssignCorrectPipe(Difficulty difficulty, Random random)
        {
            if (Pipe != null) return;
            switch (_connected.Count)
            {
                case 0:
                    if (difficulty == Difficulty.Easy)
                    {
                        Pipe = random.Next(2) switch
                        {
                            0 => new NarrowPipe(),
                            1 => new CornerPipe(),
                            _ => Pipe
                        };
                    }
                    else
                    {
                        Pipe = random.Next(4) switch
                        {
                            0 => new NarrowPipe(),
                            1 => new CornerPipe(),
                            2 => new TriplePipe(),
                            3 => new QuadPipe(),
                            _ => Pipe
                        };
                    }

                    break;
                case 2:
                    if (_connected.Contains(_surroundings[Direction.Up]) && _connected.Contains(_surroundings[Direction.Down])
                        || _connected.Contains(_surroundings[Direction.Left]) && _connected.Contains(_surroundings[Direction.Right]))
                    {
                        Pipe = new NarrowPipe();
                    } else
                    {
                        Pipe = new CornerPipe();
                    }
                    break;
                case 3: 
                    Pipe = new TriplePipe(); 
                    break;
                case 4: 
                    Pipe = new QuadPipe(); 
                    break;
            }
        }
    }
}
