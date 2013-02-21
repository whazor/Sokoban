using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sokoban.Domain.Domain.Highscores;
using Sokoban.Frontend.Views;
using List = Sokoban.Domain.Domain.Highscores.List;

namespace Sokoban.Frontend
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Dictionary<String, Level> _levels = List.Get();
        private List<Level> _list; 
        private Game _game;

        public MainWindow()
        {
            InitializeComponent();
//            grid.DataContext = Sokoban.Domain.Highscores.List.Get();
            
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            _list = _levels.Values.ToList();
            _list.Sort((level, level1) => System.String.Compare(level.Name, level1.Name, System.StringComparison.Ordinal));
            Levels.ItemsSource = _list;
        }

        private void Level_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if(_game != null)
                _game.Close();
            _game = new Game(new Domain.Domain.Game(_levels[(string)btn.Tag]));
            _game.Show();
        }

        private void Levels_OnKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
                _list[Levels.SelectedIndex].RemoveHighscores();
        }
    }
}
