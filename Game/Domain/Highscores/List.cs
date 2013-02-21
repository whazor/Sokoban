using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sokoban.Domain.Domain.Highscores
{
    public class List
    {
        private static List _current;
        private readonly Dictionary<string, Level> _levels = new Dictionary<string, Level>();

        public List()
        {
            var resourceManager = Levels.ResourceManager;
            var levels = resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            foreach (var name in from DictionaryEntry level in levels select (string) level.Key)
            {
                _levels.Add(name, new Level(name));
            }            
            //_levels.Sort((level, level1) => System.String.Compare(level.Name, level1.Name, System.StringComparison.Ordinal));
        }

        public static Dictionary<String, Level> Get()
        {
            if(_current == null) _current = new List();
            return _current._levels;
        }
    }
}
