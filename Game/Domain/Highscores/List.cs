using System.Collections;
using System.Globalization;
using System.Linq;

namespace Sokoban.Domain.Domain.Highscores
{
    public class List
    {
        //private string _filename;
        private static List _current;  
        private readonly System.Collections.Generic.List<Level> _levels;

        public List() //string name
        {
            var resourceManager = Levels.ResourceManager;
            var levels = resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            _levels = (from DictionaryEntry level in levels select new Level((string)level.Key)).ToList();
            _levels.Sort((level, level1) => System.String.Compare(level.Name, level1.Name, System.StringComparison.Ordinal));
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
