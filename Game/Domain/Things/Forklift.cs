using Sokoban.Domain.Domain.Floor;

namespace Sokoban.Domain.Domain.Things
{
    public class Forklift : IThing
    {
        private Direction _direction = Direction.Left;
        public string ResourceName { get { return "forklift"; } }

        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
    }
}
