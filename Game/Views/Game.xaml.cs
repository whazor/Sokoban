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
using System.Windows.Shapes;
using Sokoban.Domain;
using Sokoban.Domain.Things;

namespace Sokoban.Views
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        private readonly Level _level;
        private readonly Image[,] _images;
        public Game(Level level)
        {
            _level = level;
            InitializeComponent();
            _images = new Image[_level.Width, _level.Height];
            Height = _level.Height * 60 + 100;
            Width = _level.Width * 60 + 100;
            Redraw();
            _level.Moved += Moved;
        }

        private void Moved(object sender, ThingChangeEvent thingChangeEvent)
        {
            var o = thingChangeEvent.Old;
            var n = thingChangeEvent.New;

            DrawImage(o.X, o.Y);
            DrawImage(n.X, n.Y);
        }

        public void Redraw()
        {
            ImageHolder.Children.Clear();
            ImageHolder.ColumnDefinitions.Clear();
            ImageHolder.RowDefinitions.Clear();
            for (var x = 0; x < _level.Width; x++)
            {
                var col = new ColumnDefinition { Width = new GridLength(60) };
                ImageHolder.ColumnDefinitions.Add(col);
            }
            for (var y = 0; y < _level.Height; y++)
            {
                var row = new RowDefinition { Height = new GridLength(60) };
                ImageHolder.RowDefinitions.Add(row);
            }
            for (var x = 0; x < _level.Width; x++)
            {
                for (var y = 0; y < _level.Height; y++)
                {                    
                    DrawImage(x, y);
                }
            }
        }

        private void DrawImage(int x, int y)
        {
            if (_images[x, y] != null)
                ImageHolder.Children.Remove(_images[x, y]);

            var thing = _level.Get(x, y);
            if (thing == null) return;
            _images[x, y] = new Image {Source = (ImageSource) FindResource(thing.ResourceName)};
            
            if (thing is Forklift)
            {
                _images[x, y].RenderTransformOrigin = new Point(0.5, 0.5);
                int rotate = 0;
                switch (((Forklift)thing).Direction)
                {
                    case Direction.Up: 
                        rotate = 270;
                        break;
                    case Direction.Left:
                        rotate = 180;
                        break;
                    case Direction.Right:
                        rotate = 0;
                        break;
                    case Direction.Down:
                        rotate = 90;
                        break;
                }
                var transform = new RotateTransform(rotate);
                _images[x, y].RenderTransform = transform;
            }
            _images[x, y].SetValue(Grid.RowProperty, y);
            _images[x, y].SetValue(Grid.ColumnProperty, x);
            ImageHolder.Children.Add(_images[x, y]);
        }

        private void Game_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                    _level.Move(Direction.Right);
                    break;
                case Key.Left:
                    _level.Move(Direction.Left);
                    break;
                case Key.Up:
                    _level.Move(Direction.Up);
                    break;
                case Key.Down:
                    _level.Move(Direction.Down);
                    break;
            }
            
        }

        private void Game_OnKeyUp(object sender, KeyEventArgs e)
        {
//            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _level.Reset();
            Redraw();
        }

    }
}
