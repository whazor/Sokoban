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
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Position Player { get; private set; }
        private int _destinations = 0;
        private int _coffinsOnDestinations = 0;

        public EventHandler<ThingChangeEvent> Moved;
        public EventHandler Won;

        private IThing[,] _staticMap;
        private IThing[,] _dynamicMap;

        public Level(String file)
        {
            _file = file;
            Load(file);
        }

        private void Load(string file)
        {
            _destinations = 0;
            _coffinsOnDestinations = 0;
            var lines = System.IO.File.ReadAllLines(file);

            var first = int.MaxValue;
            var last = int.MinValue;

            foreach (var line in lines)
            {
                for (var j = 0; j < line.Length; j++)
                {
                    if (line[j] == ' ') continue;
                    first = Math.Min(first, j);
                    last = Math.Max(last, j);
                }
            }
            Height = lines.Length;
            last -= first;
            Width = last + 1;

            _staticMap = new IThing[Width,Height];
            _dynamicMap = new IThing[Width,Height];

            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                {
                    IThing thing = null;
                    IThing thing2 = null;
                    switch (lines[i][j])
                    {
                        case '#':
                            thing = new Wall();
                            break;
                        case 'o':
                            thing = new Coffin();
                            break;
                        case 'O':
                            thing2 = new Coffin();
                            thing = new Destination();
                            _destinations++;
                            _coffinsOnDestinations++;
                            break;
                        case 'x':
                            thing = new Destination();
                            _destinations++;
                            break;
                        case '@':
                            thing = new Forklift();
                            Player = new Position(j - first, i);
                            break;
                        case ' ':
                            break;
                    }
                    if (thing == null) continue;

                    if (thing.Dynamic)
                        _dynamicMap[j - first, i] = thing;
                    else
                        _staticMap[j - first, i] = thing;

                    if (thing2 == null) continue;

                    if (thing2.Dynamic)
                        _dynamicMap[j - first, i] = thing2;
                    else
                        _staticMap[j - first, i] = thing2;
                }
            }
        }

        public virtual IThing Neighbours(Position pos, Direction direction)
        {
            return Get(pos.Move(direction));
        }

        public virtual IThing Get(Position pos)
        {
            // check for out of bounds
            if (!InBounds(pos)) return null;

            return _dynamicMap[pos.X, pos.Y] ?? _staticMap[pos.X, pos.Y];
        }

        private bool InBounds(Position pos)
        {
            return !(pos.X < 0 || pos.Y < 0 || pos.Y > Height || pos.Y > Width);
        }

        private void Move(Position o, Position n)
        {
            if (!InBounds(o)) return;
            if (!InBounds(n)) return;

            if (_dynamicMap[o.X, o.Y] == null) return;
            _dynamicMap[n.X, n.Y] = _dynamicMap[o.X, o.Y];
            _dynamicMap[o.X, o.Y] = null;

            if (Moved == null) return;

            Moved(this, new ThingChangeEvent(o, n));
        }

        public void Move(Direction direction)
        {
            // check for blocks
            var neighbour = Neighbours(Player, direction);
            if (neighbour is Coffin)
            {
                var pos = Player.Move(direction);
                if ((Neighbours(pos, direction) is Destination))
                {
                    ((Coffin)neighbour).OnDestination = true;
                    _coffinsOnDestinations++;
                    if (_coffinsOnDestinations == _destinations)
                    {
                        //Won(this, new EventArgs());
                    }
                    // doe iets
                }
                else if (Neighbours(pos, direction) != null) return;
                else if (((Coffin) neighbour).OnDestination)
                {
                    ((Coffin)neighbour).OnDestination = false;
                    _coffinsOnDestinations--;
                }
                Move(pos, pos.Move(direction));
            }
            else if (neighbour is Destination) { }
            else if (neighbour != null) return;

            ((Forklift)Get(Player)).Direction = direction;
            Move(Player, Player.Move(direction));
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
