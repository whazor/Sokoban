using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sokoban.Domain.Highscores
{
    public class List
    {
        //private string _filename;
        private static List _current;  
        private System.Collections.Generic.List<Level> _levels;

        public List() //string name
        {
            var resourceManager = Levels.ResourceManager;
            var levels = resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            _levels = (from DictionaryEntry level in levels select new Level((string)level.Key)).ToList();
//            var f = XmlReader.Create(_filename);
//            while(f.Read())
//                if (f.NodeType == XmlNodeType.Element)
//                {
//                    var level = new Level();
//                    if (level.ReadElement(f))
//                    {
//                        _levels.Add(level);
//                    }
//                }
//            f.Close();
        }


     

        public static System.Collections.Generic.List<Level> Get()
        {
            if(_current == null) _current = new List();
            return _current._levels;
        }

//        ~List()
//        {
//            var f = new StreamWriter(_filename);
//            f.WriteLine("<level>");
//            f.WriteLine("</level>");
//        }
    }
}
