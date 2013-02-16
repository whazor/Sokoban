using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sokoban.Domain.Helpers;
using Sokoban.Domain.Things;

namespace Sokoban.Domain
{
    class Map
    {

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Position Player { get; private set; }

        private IThing[,] _staticMap;
        private IThing[,] _dynamicMap;

        private int _destinations = 0;
        private int _coffinsOnDestinations = 0;
       
        public Map(String[] lines)
        {
            var t = Trim(lines);
            var first = t.Item1;
            var last = t.Item2;
            Height = lines.Length;
            Width = last + 1;

            _staticMap = new IThing[Width, Height];
            _dynamicMap = new IThing[Width, Height];

            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                {
                    Recognize(lines[i][j], new Position(j - first, i));
                }
            }

            var isOpen = new Func<Position, bool>(thing => !(_staticMap[thing.X, thing.Y] is Wall));
            var getUnionId = new Func<Position, int>(thing => thing.Y % Width + thing.X * Height);
            var uf = new UnionFind(Width * Height); 
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    var pos = new Position(i, j);
                    if(!isOpen(pos)) return;
                    
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
            if (percolates)
            {
                throw new LevelException("Er zit een opening in het level.");
            }
        }

        private bool InBounds(Position pos)
        {
            return !(pos.X < 0 || pos.Y < 0 || pos.Y > Height || pos.Y > Width);
        }

        private delegate void SetMap(IThing[,] map, Position pos, IThing thing);
        private void Recognize(char letter, Position pos)
        {
            SetMap map = (target, cor, obj) => target[cor.X, cor.Y] = obj;
            switch (letter)
            {
                case '#':
                    map(_staticMap, pos, new Wall());
                    break;
                case 'o':
                    map(_dynamicMap, pos, new Coffin());
                    break;
                case 'O':
                    map(_dynamicMap, pos, new Coffin());
                    map(_staticMap, pos, new Destination());
                    _destinations++;
                    _coffinsOnDestinations++;
                    break;
                case 'x':
                    map(_staticMap, pos, new Destination());
                    _destinations++;
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

    internal class LevelException : Exception
    {
        public LevelException(string erZitEenOpeningInHetLevel)
        {
            throw new NotImplementedException();
        }
    }
}
