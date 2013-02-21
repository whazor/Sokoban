using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.Domain.Events
{
    class ScoreChangeEvent : EventArgs
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
