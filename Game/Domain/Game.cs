﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Sokoban.Domain.Events;
using Sokoban.Domain.Highscores;
using Sokoban.Domain.Things;

namespace Sokoban.Domain
{
    public class Game
    {
        #region Properties
        private string[] _lines; 
        private Map _map;
        private int _moves;
        private int _playtime;
        

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
        public Game(String file)
        {
            _lines = File.ReadAllLines(file);
            _map = new Map(_lines);
            InitiateTimer();
        }

        public Game(Level game)
        {
            var bytes = Levels.ResourceManager.GetObject(game.Name) as Byte[];
            _lines = Encoding.UTF8.GetString(bytes).Split(new[] {"\n", "\r\n"}, StringSplitOptions.None);
            _map = new Map(_lines);
            InitiateTimer();
        }
        #endregion

        private void InitiateTimer() 
        {
            _playtime = 0;
            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += (sender, args) => _playtime++;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
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

                    if (_map.Coffins.Count(c => c.OnDestination) == _map.Destinations.Count)
                    {
                        Console.Out.Write("Gewonnen!"); //TODO: hier iets doen
                        //Won(this, new EventArgs());
                    }
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

            //Add to score
            _moves++;
            if(Score != null)
                Score(this, new ScoreChangeEvent(this._moves, this._playtime));
        }

        public virtual IThing Get(int pos, int i)
        {
            return Get(new Position(pos, i));
        }

        public void Reset()
        {
            _map = new Map(_lines);
            InitiateTimer();
        }
        #endregion
    }
}
