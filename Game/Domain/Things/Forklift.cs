using System.Windows;
using System.Windows.Media;

namespace Sokoban.Domain.Things
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
