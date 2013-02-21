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
            _levels.Sort((level, level1) => level.Name.CompareTo(level1.Name));
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
