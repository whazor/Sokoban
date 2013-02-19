using System;

namespace Sokoban.Domain.Events
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
}