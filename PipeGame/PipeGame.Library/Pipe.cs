namespace PipeGame.Library
{
    public abstract class Pipe
    {
        protected readonly List<Direction> Outputs;

        protected Pipe() 
        {
            this.Outputs = new List<Direction>();
        }

        internal virtual void Rotate()
        {
            for (var i = 0; i < this.Outputs.Count; i++)    
            {
                var direction = this.Outputs[i];
                DirectionCommands.Rotate(ref direction);
                this.Outputs[i] = direction;
            }
        }

        public IEnumerable<Direction> GetDirections()
        {
            return this.Outputs;
        }

        public bool HasDirection(Direction direction)
        {
            return Outputs.Contains(direction);
        }
    }

    public abstract class TankPipe : Pipe
    {
        protected TankPipe(Direction direction) 
        {
            Outputs.Add(direction);
        }

        internal override void Rotate()
        {
            
        }
    }

    public class WaterFeed : TankPipe
    {
        public WaterFeed(Direction direction) : base(direction)
        {

        }
    }

    public class WaterDrain : TankPipe
    {
        public WaterDrain(Direction direction) : base(direction)
        {

        }
    }


    public class NarrowPipe : Pipe
    {
        public NarrowPipe()
        {
            Outputs.Add(Direction.Up);
            Outputs.Add(Direction.Down);
        }
    }

    public class CornerPipe : Pipe
    {
        public CornerPipe()
        {
            Outputs.Add(Direction.Up);
            Outputs.Add(Direction.Right);
        }
    }

    public class TriplePipe : Pipe
    {
        public TriplePipe()
        {
            Outputs.Add(Direction.Up);
            Outputs.Add(Direction.Right);
            Outputs.Add(Direction.Down);
        }
    }

    public class QuadPipe : Pipe
    {
        public QuadPipe()
        {
            Outputs.Add(Direction.Up);
            Outputs.Add(Direction.Right);
            Outputs.Add(Direction.Down);
            Outputs.Add(Direction.Left);
        }
    }
}