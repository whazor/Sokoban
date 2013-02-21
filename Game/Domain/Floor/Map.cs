using System;
using System.Collections.Generic;
using System.Linq;
using Sokoban.Domain.Domain.Events;
using Sokoban.Domain.Domain.Things;
using Sokoban.Domain.Events;
using Sokoban.Domain.Helpers;
using Sokoban.Domain.Things;

namespace Sokoban.Domain.Domain.Floor
{
    class Map
    {

        public int Width { get; private set; }
        public int Height { get; private set; }

        // Map
        public Position Player { get; set; }

        public List<Coffin> Coffins { get; private set; }
        public List<Destination> Destinations { get; private set; }

        private readonly IThing[,] _staticMap;
        private readonly IThing[,] _dynamicMap;
      
        public Map(IList<string> lines)
        {
            if(lines.Count == 0)
                throw new LevelException("Dit level is leeg.");

            var t = Trim(lines);
            var first = t.Item1;
            var last = t.Item2;
            Height = lines.Count;
            Width = last + 1;

            Coffins = new List<Coffin>();
            Destinations = new List<Destination>();

            _staticMap = new IThing[Width, Height];
            _dynamicMap = new IThing[Width, Height];

            for (var i = 0; i < lines.Count; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                {
                    Recognize(lines[i][j], new Position(j - first, i));
                }
            }

            if (CheckForPercolation())
            {
                throw new LevelException("Er zit een opening in het level, dus het level is kapot.");
            } 

            if (Destinations.Count == 0 || Coffins.Count < Destinations.Count)
            {
                throw new LevelException("Dit level is onspeelbaar, er is namelijk geen manier om dit te winnen.");
            }
        }

        private bool CheckForPercolation()
        {
            var isOpen = new Func<Position, bool>(thing => !(_staticMap[thing.X, thing.Y] is Wall));
            var getUnionId = new Func<Position, int>(thing =>
                {
                    if (Width >= Height)
                    {
                        return thing.Y % Height + thing.X * Height;
                    }
                    else
                    {
                        return thing.X % Width + thing.Y * Width;
                    }
                });
            var uf = new UnionFind(Width * Height);
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    var pos = new Position(i, j);
                    if (!isOpen(pos)) continue;

                    Direction[] directions = {Direction.Down, Direction.Up, Direction.Left, Direction.Right};
                    foreach (var tmp in directions
                        .Select(pos.Move)
                        .Where(tmp => InBounds(tmp) && isOpen(tmp)))
                    {
                        uf.Union(getUnionId(pos), getUnionId(tmp));
                    }
                }
            }
            var percolates = false;
            var playerId = getUnionId(Player);
            for (var i = 0; i < Width; i++)
            {
                var top = new Position(i, 0);
                var bottom = new Position(i, Height - 1);
                if (uf.IsConnected(playerId, getUnionId(top)))
                    percolates = true;
                if (uf.IsConnected(playerId, getUnionId(bottom)))
                    percolates = true;
            }
            for (var j = 0; j < Height; j++)
            {
                var left = new Position(0, j);
                var right = new Position(Width - 1, j);
                if (uf.IsConnected(playerId, getUnionId(left)))
                    percolates = true;

                if (uf.IsConnected(playerId, getUnionId(right)))
                    percolates = true;
            }
            return percolates;
        }

        public bool InBounds(Position pos)
        {
            return (pos.X >= 0 && pos.Y >= 0 && pos.Y < Height && pos.X < Width);
        }

        public void Move(Position o, Position n, EventHandler<ThingChangeEvent> moved)
        {
            if (!InBounds(o)) return;
            if (!InBounds(n)) return;

            if (_dynamicMap[o.X, o.Y] == null) return;
            _dynamicMap[n.X, n.Y] = _dynamicMap[o.X, o.Y];
            _dynamicMap[o.X, o.Y] = null;

            if (moved == null) return;
            moved(this, new ThingChangeEvent(o, n));
        }

        public virtual IThing Get(Position pos)
        {
            // check for out of bounds
            if (!InBounds(pos)) return null;

            return _dynamicMap[pos.X, pos.Y] ?? _staticMap[pos.X, pos.Y];
        }

        private delegate void SetMap(IThing[,] map, Position pos, IThing thing);
        private void Recognize(char letter, Position pos)
        {
            SetMap map = (target, cor, obj) => target[cor.X, cor.Y] = obj;
            Destination destination;
            Coffin coffin;
            switch (letter)
            {
                case '#':
                    map(_staticMap, pos, new Wall());
                    break;
                case 'o':
                    coffin = new Coffin();
                    Coffins.Add(coffin);
                    map(_dynamicMap, pos, coffin);
                    break;
                case 'O':
                    coffin = new Coffin {OnDestination = true};
                    Coffins.Add(coffin);
                    map(_dynamicMap, pos, new Coffin());
                    destination = new Destination();
                    Destinations.Add(destination);
                    map(_staticMap, pos, destination);
                    break;
                case 'x':
                    destination = new Destination();
                    Destinations.Add(destination);
                    map(_staticMap, pos, destination);
                    break;
                case '@':
                    map(_dynamicMap, pos, new Forklift());
                    Player = pos;
                    break;
            }
        }

        private static Tuple<int, int> Trim(IEnumerable<string> lines)
        {
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
            
            last -= first;
            return new Tuple<int, int>(first, last);
        }
    }

    public class LevelException : Exception
    {
        public string Level { get; set; }
        public override string Message { 
            get
            {
                if (Level != null)
                    return Level + " - " + base.Message;
                return base.Message;
            }
        }
        public LevelException(string message)
            : base(message)
        {
        }
    }
}
