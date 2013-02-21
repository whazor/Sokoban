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
        List<Level> _levels = List.Get();  
        public MainWindow()
        {
            InitializeComponent();
//            grid.DataContext = Sokoban.Domain.Highscores.List.Get();
            
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Levels.ItemsSource = _levels;
            //Levels.ItemsSource = Sokoban.Domain.Highscores.List.Get();
        }

        private void Level_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var game = new Game(new Domain.Domain.Game(new Level((string) btn.Tag)));
            game.Show();
        }
    }
}
