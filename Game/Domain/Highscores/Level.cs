using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sokoban.Domain.Highscores
{
    public class Level
    {
        public string Name { get; set; }
        public string Highscore { get; private set; }

        public Level(String name)
        {
            Name = name;
            Highscore = "100!!";
        }

        public bool ReadElement(XmlReader xmlReader)
        {
            throw new NotImplementedException();
        }
    }
}
