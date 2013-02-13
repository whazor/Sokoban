using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sokoban.Domain.Things;

namespace Sokoban.Domain
{
    class Level
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        private readonly IThing[,] _staticMap;
        private readonly IThing[,] _dynamicMap;

        public Level(String file)
        {
            var lines = System.IO.File.ReadAllLines(file);
            Width = 0;
            foreach (var line in lines)
            {
                Width = Math.Max(Width, line.Length);
            }
            Height = lines.Length;

            _staticMap = new IThing[Width, Height];
            _dynamicMap = new IThing[Width, Height];

            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                {
                    IThing thing = null;
                    switch (lines[i][j])
                    {
                        case '#':
                            thing = new Wall();
                            break;
                        case 'o':
                            thing = new Coffin();
                            break;
                        case 'x':
                            thing = new Destination();
                            break;
                        case '@':
                            thing = new Forklift();
                            break;
                        case ' ':
                            break;
                    }
                    if (thing == null) continue;
                    if (thing.Dynamic)
                    {
                        _dynamicMap[i, j] = thing;
                    }
                    else
                    {
                        _staticMap[i, j] = thing;
                    }
                }
            }
            
        }

        public virtual IThing Neighbours(int x, int y, Direction direction)
        {
            var newx = x;
            var newy = y;
            switch (direction)
            {
                case Direction.Top:
                    newy--;
                    break;
                case Direction.Left:
                    newx--;
                    break;
                case Direction.Right:
                    newx++;
                    break;
                case Direction.Bottom:
                    newy++;
                    break;
            }
            
            return Get(newx, newy);
        }


        public virtual IThing Get(int x, int y)
        {
            // check for out of bounds
            if (x < 0 || y < 0 || y > Height || x > Width)
                return null;
            
            return _dynamicMap[x,y] ?? _staticMap[x, y];
        }
    }
}
