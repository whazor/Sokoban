using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using Sokoban.Domain.Annotations;

namespace Sokoban.Domain.Domain.Highscores
{
    public class Level : INotifyPropertyChanged
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
            Directory.CreateDirectory("highscores/");
            var path = "highscores/" + Name;

            var writer = !File.Exists(path) ? File.CreateText(path) : File.AppendText(path);
            writer.WriteLine(time+","+moves);
            writer.Close();

            LoadHighscores();
            OnPropertyChanged("Highscore");
        }

        public void LoadHighscores()
        {
            if (File.Exists("highscores/" + Name))
            {
                var text = File.ReadAllLines("highscores/" + Name);
                var highscores = (
                    from line in text
                    select line.Split(',') into split
                    where split.Length == 2
                    orderby split[1], split[0]
                    select new Tuple<int, int>(int.Parse(split[0]), int.Parse(split[1]))
                    ).ToList();

                if (highscores.Count > 0)
                {
                    var min = Math.Floor((decimal)highscores.First().Item1 / 60);
                    var sec = (highscores.First().Item1 % 60).ToString("00");
                    Highscore = String.Format("Tijd: {0}:{1} - Stappen: {2}", min, sec, highscores.First().Item2);
                    return;
                }
            }

            Highscore = "Nog geen highscore!";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
