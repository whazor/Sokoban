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
using Sokoban.Domain.Events;
using Sokoban.Domain.Things;

namespace Sokoban.Views
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game
    {
        private readonly Domain.Game _game;
        private readonly Image[,] _images;
        public Game(Domain.Game game)
        {
            _game = game;
            InitializeComponent();
            _images = new Image[_game.Width, _game.Height];
            Height = _game.Height * 60 + 100;
            Width = _game.Width * 60 + 100;
            Redraw();
            _game.Moved += Moved;
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
            for (var x = 0; x < _game.Width; x++)
            {
                var col = new ColumnDefinition { Width = new GridLength(60) };
                ImageHolder.ColumnDefinitions.Add(col);
            }
            for (var y = 0; y < _game.Height; y++)
            {
                var row = new RowDefinition { Height = new GridLength(60) };
                ImageHolder.RowDefinitions.Add(row);
            }
            for (var x = 0; x < _game.Width; x++)
            {
                for (var y = 0; y < _game.Height; y++)
                {                    
                    DrawImage(x, y);
                }
            }
        }

        private void DrawImage(int x, int y)
        {
            if (_images[x, y] != null)
                ImageHolder.Children.Remove(_images[x, y]);

            var thing = _game.Get(x, y);
            if (thing == null) return;
            _images[x, y] = new Image {Source = (ImageSource) FindResource(thing.ResourceName)};

            var forklift = thing as Forklift;
            if (forklift != null)
            {
                _images[x, y].RenderTransformOrigin = new Point(0.5, 0.5);
                var rotate = 0;
                switch (forklift.Direction)
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
                    _game.Move(Direction.Right);
                    break;
                case Key.Left:
                    _game.Move(Direction.Left);
                    break;
                case Key.Up:
                    _game.Move(Direction.Up);
                    break;
                case Key.Down:
                    _game.Move(Direction.Down);
                    break;
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _game.Reset();
            Redraw();
        }

    }
}
