namespace PipeGame.Library
{
    public class Board  
    {
        private readonly Segment[,] _layout;
        private readonly List<Segment> _startingPoints;
        private readonly List<Segment> _endPoints;
        public int Rows { get; init; }
        public int Columns { get; init; }
        public Difficulty Difficulty {get; init; }
        private readonly Random _random;
        private readonly List<Segment> _createdAnswers;



        public Board(Difficulty difficulty)
        {
            Difficulty = difficulty;
            Rows = DifficultyCommands.GetParameters(difficulty).Rows;
            Columns = DifficultyCommands.GetParameters(difficulty).Columns;
            _layout = new Segment[Rows, Columns];
            _startingPoints = new List<Segment>();
            _endPoints = new List<Segment>();
            _createdAnswers = new List<Segment>();
            _random = new Random();

            BuildLayout();
            BuildPath();
        }

        public Segment this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Rows)
                {
                    throw new ArgumentOutOfRangeException(nameof(row));
                }
                if (column < 0 || column >= Columns)  {
                    throw new ArgumentOutOfRangeException(nameof(column));
                }
                return _layout[row, column]; 
            }
        }

        public IEnumerable<Segment> GetCreatedAnswers()
        {
            return this._createdAnswers;
        }

        public bool CheckAnswers()
        {
            ClearVisited();
            var next = _startingPoints.ToList();

            while (next.Count > 0)
            {
                var current = next[0];
                _createdAnswers.Add(current);
                current.CorrectAnswer = true;

                foreach (var direction in current.Pipe!.GetDirections())
                {
                    if (current.AccessSurrounding(direction) == null)
                    {
                        current.CorrectAnswer = false;
                    }
                    else if (current.AccessSurrounding(direction)!.Pipe!.HasDirection(DirectionCommands.Opposite(direction)))
                    {
                        if (!_createdAnswers.Contains(current.AccessSurrounding(direction)!))
                        {
                            next.Add(current.AccessSurrounding(direction)!);
                        }
                    }
                    else
                    {
                        current.CorrectAnswer = false;
                    }
                }

                next.Remove(current);
            }

            return _createdAnswers.All(answer => answer.CorrectAnswer) && _endPoints.All(endPoint => _createdAnswers.Contains(endPoint));
        }

        private void BuildLayout()
        {
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    _layout[i, j] = new Segment();

                    if (i != 0)
                    {
                        _layout[i, j].AssignSegment(Direction.Up, _layout[i - 1, j]);
                        _layout[i-1, j].AssignSegment(Direction.Down, _layout[i, j]);
                    }

                    if (j == 0) continue;
                    _layout[i, j].AssignSegment(Direction.Left, _layout[i, j - 1]);
                    _layout[i, j - 1].AssignSegment(Direction.Right, _layout[i, j]);
                }
            }
        }

        private void BuildPath()
        {
            for (var i = 0; i < DifficultyCommands.GetParameters(Difficulty).Tubes; i++)
            {
                Segment? start;
                do
                {
                    start = PickTank(0, _random.Next(4), _random.Next(1, Rows - 1), _random.Next(1, Columns - 1));
                } while (start == null);

                Segment? end;
                do
                {
                    end = PickTank(1, _random.Next(4), _random.Next(1, Rows - 1), _random.Next(1, Columns - 1));
                } while (end == null);
                
                _startingPoints.Add(start);
                _endPoints.Add(end);
            }

            foreach (var start in _startingPoints)
            {
                ClearVisited();
                FindPath(start);
            }

            AssignPipes();
            RotatePipes();
        }

        private void RotatePipes()
        {
            foreach (var segment in _layout)
            {
                for (var i = 0; i < _random.Next(4); i++)
                {
                    segment.Rotate();
                }
            }
        }

        private bool FindPath(Segment segment)
        {
            segment.Visited = true;

            if ((segment.Pipe?.GetType() == typeof(WaterDrain)))
            {
                return segment.GetConnectedSize() == 0;
            }

            var segments = FillSegments(segment);

            while (segments.Count > 0)
            {
                var next = segments[_random.Next(segments.Count)];
                if (FindPath(next))
                {
                    segment.ConnectSegment(next);
                    next.ConnectSegment(segment);
                    return true;
                }
                segments.Remove(next);
                
            }
            return false;
        }

        private static List<Segment> FillSegments(Segment segment)
        {
            var segments = new List<Segment?>();
            if ((segment.Pipe?.GetType() == typeof(WaterFeed)))
            {
                segments.Add(segment.AccessSurrounding(segment.Pipe.GetDirections().First()));
            }
            else
            {
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    if (segment.AccessSurrounding(direction) == null ||
                        segment.AccessSurrounding(direction)!.Visited || 
                        segment.AccessSurrounding(direction)!.Pipe?.GetType() == typeof(WaterFeed)) continue;
                    if (segment.AccessSurrounding(direction)!.Pipe?.GetType() == typeof(WaterDrain))
                    {
                        if (segment.AccessSurrounding(direction)!.AccessSurrounding(segment.AccessSurrounding(direction)!.Pipe!.GetDirections().First()) == segment)
                        {
                            segments.Add(segment.AccessSurrounding(direction));
                        }
                    }
                    else
                    {
                        segments.Add(segment.AccessSurrounding(direction));
                    }
                }
            }

            return segments!;
        }

        private void ClearVisited()
        {
            foreach (var segment in _layout)
            {
                if (segment.Pipe?.GetType() != typeof(TankPipe))
                {
                    segment.Visited = false;
                }
            }
        }

        private void AssignPipes()
        {
            foreach (var segment in _layout)
            {
                segment.AssignCorrectPipe(Difficulty, _random);
            }
        }

        private Segment? PickTank(int type, int decision, int rowIndex, int colIndex)
        {
            return decision switch
            {
                0 => MakeTank(type, 0, colIndex, Direction.Down),
                1 => MakeTank(type, rowIndex, Columns - 1, Direction.Left),
                2 => MakeTank(type, Rows - 1, colIndex, Direction.Up),
                3 => MakeTank(type, rowIndex, 0, Direction.Right),
                _ => throw new ArgumentException("Invalid decision.")
            };
        }

        private Segment? MakeTank(int type, int rowIndex, int colIndex, Direction direction)
        {
            if (_layout[rowIndex, colIndex].Pipe != null) return null;
            _layout[rowIndex, colIndex].AssignTank(type, direction);
            return _layout[rowIndex, colIndex];
        }
    }
}
