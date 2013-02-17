using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sokoban.Domain.Things;

namespace Sokoban.Domain
{
    public class ThingChangeEvent : EventArgs
    {
        public ThingChangeEvent(Position pOld, Position pNew)
        {
            Old = pOld;
            New = pNew;
        }

        public Position Old { get; set; }
        public Position New { get; set; }
    }

    public class Level
    {
        private readonly string _file;
        private Map _map;

        public int Height { get { return _map.Height; } }
        public int Width { get { return _map.Width; } }

        public Position Player
        {
            get { return _map.Player; }
            set { _map.Player = value; }
        }
        //public int Width { get; private set; }
        //public int Height { get; private set; }
        //public Position Player { get; private set; }
        //private int _destinations = 0;
        //private int _coffinsOnDestinations = 0;

        public EventHandler<ThingChangeEvent> Moved;
        public EventHandler Won;

        //private IThing[,] _staticMap;
        //private IThing[,] _dynamicMap;

        public Level(String file)
        {
            _file = file;
            Load(file);
        }

        private void Load(string file)
        {
            var lines = System.IO.File.ReadAllLines(file);
            _map = new Map(lines);
        }

        public virtual IThing Neighbours(Position pos, Direction direction)
        {
            return Get(pos.Move(direction));
        }

        public virtual IThing Get(Position pos)
        {
            return _map.Get(pos);
        }

        private bool InBounds(Position pos)
        {
            return _map.InBounds(pos);
        }


        public void Move(Direction direction)
        {
            // check for blocks
            var neighbour = Neighbours(_map.Player, direction);
            if (neighbour is Coffin)
            {
                var pos = Player.Move(direction);
                if ((Neighbours(pos, direction) is Destination))
                {
                    ((Coffin)neighbour).OnDestination = true;
//                    _coffinsOnDestinations++;
//                    if (_coffinsOnDestinations == _destinations)
//                    {
                        //Won(this, new EventArgs());
//                    }
                    // doe iets
                }
                else if (Neighbours(pos, direction) != null) return;
                else if (((Coffin) neighbour).OnDestination)
                {
                    ((Coffin)neighbour).OnDestination = false;
//                    _coffinsOnDestinations--;
                }
                _map.Move(pos, pos.Move(direction), Moved);
            }
            else if (neighbour is Destination) { }
            else if (neighbour != null) return;

            ((Forklift)Get(Player)).Direction = direction;
            _map.Move(Player, Player.Move(direction), Moved);
            Player = Player.Move(direction);
        }

        public virtual IThing Get(int pos, int i)
        {
            return Get(new Position(pos, i));
        }

        public void Reset()
        {
            Load(_file);
        }
    }
}
