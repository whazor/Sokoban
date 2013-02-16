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
using Microsoft.Win32;
using Sokoban.Domain;
using Sokoban.Views;

namespace Sokoban
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".soko",
                Filter = "Sokubanlevels (.soko)|*.soko"
            };
            

            var result = dlg.ShowDialog();

            if (!((bool) result)) return;

            var level = new Level(dlg.FileName);
            var window = new Game(level);

            Close();
            window.ShowDialog();
        }
    }
}
