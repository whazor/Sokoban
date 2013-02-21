using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Sokoban.Domain.Domain.Events;
using Sokoban.Domain.Domain.Floor;
using Sokoban.Domain.Domain.Highscores;
using Sokoban.Domain.Domain.Things;

namespace Sokoban.Domain.Domain
{
    public class Game
    {
        #region Properties
        private readonly string[] _lines; 
        private Map _map;
        private int _moves;
        private int _playtime;
        private Level _level;
        private DispatcherTimer _dispatcherTimer;

        public int Height { get { return _map.Height; } }
        public int Width { get { return _map.Width; } }

        public Position Player
        {
            get { return _map.Player; }
            set { _map.Player = value; }
        }

        public EventHandler<ThingChangeEvent> Moved;
        public EventHandler Won;
        public EventHandler<ScoreChangeEvent> Score;
        #endregion
        
        #region Contructors
        public Game(Level game)
        {
            _level = game;
            var bytes = Levels.ResourceManager.GetObject(game.Name) as Byte[];
            if (bytes == null) return;
            _lines = Encoding.UTF8.GetString(bytes).Split(new[] {"\n", "\r\n"}, StringSplitOptions.None);
            _map = new Map(_lines);
            InitiateTimer();
        }
        #endregion

        private void InitiateTimer() 
        {
            _playtime = 0;
            if (_dispatcherTimer != null) _dispatcherTimer.Stop();
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += (sender, args) => {
                _playtime++;
                if (Score != null)
                    Score(this, new ScoreChangeEvent(_moves, _playtime));
            };
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
        }

        #region Methods
        public virtual IThing Neighbour(Position pos, Direction direction)
        {
            return Get(pos.Move(direction));
        }

        public virtual IThing Get(Position pos)
        {
            return _map.Get(pos);
        }

        public void Move(Direction direction)
        {
            bool has_won = false;

            // check for blocks
            var neighbour = Neighbour(_map.Player, direction);

            // is the player pushing a coffin?
            var coffin = neighbour as Coffin;
            if (coffin != null)
            {
                var pos = Player.Move(direction);
                if ((Neighbour(pos, direction) is Destination))
                {
                    coffin.OnDestination = true;

                    has_won = (_map.Coffins.Count(c => c.OnDestination) == _map.Destinations.Count);
                }
                else if (Neighbour(pos, direction) != null) return;
                else if (coffin.OnDestination)
                {
                    coffin.OnDestination = false;
                }
                _map.Move(pos, pos.Move(direction), Moved);
            }
            else if (neighbour is Destination) { }
            else if (neighbour != null) return;

            ((Forklift)Get(Player)).Direction = direction;
            _map.Move(Player, Player.Move(direction), Moved);
            Player = Player.Move(direction);
            _moves++;

            if (Score != null)
                Score(this, new ScoreChangeEvent(_moves, _playtime));

            if (!has_won) return;

            _level.SaveHighscore(_playtime, _moves);
            _dispatcherTimer.Stop();
            MessageBox.Show("Je hebt gewonnen!");
        }

        public virtual IThing Get(int pos, int i)
        {
            return Get(new Position(pos, i));
        }

        public void Reset()
        {
            _map = new Map(_lines);
            _moves = 0;
            _playtime = 0;
            InitiateTimer();
        }
        #endregion
    }
}
