using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            LoadHighscores();
        }

        public void SaveHighscore(int time, int moves)
        {
            var writer = File.AppendText("highscores/" + Name);
            writer.WriteLine(time+","+moves);
        }

        public void LoadHighscores()
        {
            Directory.CreateDirectory("highscores/");
            try
            {
                var text = File.ReadAllLines("highscores/" + Name);
                var highscores = (
                    from line in text select line.Split(',') into split 
                    where split.Length == 2
                    orderby split[1], split[0]
                    select new Tuple<int, int>(int.Parse(split[0]), int.Parse(split[1]))
                    ).ToList();

                if (highscores.Count > 0)
                {
                    var min = Math.Floor((decimal)highscores.First().Item1 / 60);
                    var sec = (highscores.First().Item1 % 60).ToString("00");
                    Highscore = String.Format("Tijd: {0}:{1} - Stappen: {2}", min, sec, highscores.First().Item2);   
                }
                return;
            }
            catch (FileNotFoundException){}

            Highscore = "Nog geen highscore!";
        }
    }
}
