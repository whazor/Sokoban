using System;

namespace Sokoban.Domain.Domain.Events
{
    public class ScoreChangeEvent : EventArgs
    {
        public int PlayTime {get; private set; }
        public int Moves {get; private set; }

        public ScoreChangeEvent(int moves, int playTime)
        {
            this.Moves = moves;
            this.PlayTime = playTime;
        }
    }
}
