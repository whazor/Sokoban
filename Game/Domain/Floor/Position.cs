namespace Sokoban.Domain.Domain.Floor
{
    public class Position
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position Move(Direction dir)
        {
            var newx = X;
            var newy = Y;
            switch (dir)
            {
                case Direction.Up:
                    newy--;
                    break;
                case Direction.Left:
                    newx--;
                    break;
                case Direction.Right:
                    newx++;
                    break;
                case Direction.Down:
                    newy++;
                    break;
            }
            return new Position(newx, newy);
        }
    }
}
