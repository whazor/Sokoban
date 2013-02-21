using System;
using System.Xml;

namespace Sokoban.Domain.Domain.Highscores
{
    public class Level
    {
        public string Name { get; set; }
        public string Highscore { get; private set; }

        public Level(String name)
        {
            Name = name;
            Highscore = "Huidige highscore: 100!!";
        }

        public bool ReadElement(XmlReader xmlReader)
        {
            throw new NotImplementedException();
        }
    }
}
