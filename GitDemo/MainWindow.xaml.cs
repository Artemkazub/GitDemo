using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GitDemo
{
    public partial class MainWindow : Window
    {
        private const int SnakeSquareSize = 20;
        private const int SnakeStartLength = 5;
        private const int SnakeStartSpeed = 40; // in milliseconds
        private const int SnakeSpeedThreshold = 100;

        private enum SnakeDirection { Left, Right, Up, Down };
        private SnakeDirection _snakeDirection = SnakeDirection.Right;
        private List<Point> _snakeParts = new List<Point>();
        private Point _snakeFood = new Point();
        private DispatcherTimer _gameTickTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            _gameTickTimer.Tick += GameTickTimer_Tick;
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(SnakeStartSpeed);
            StartNewGame();
        }

        private void StartNewGame()
        {
            _snakeParts.Clear();
            _snakeDirection = SnakeDirection.Right;
            for (int i = 0; i < SnakeStartLength; i++)
            {
                _snakeParts.Add(new Point(SnakeSquareSize * (SnakeStartLength - i - 1), 0));
            }
            DrawSnake();
            DrawSnakeFood();
            _gameTickTimer.IsEnabled = true;
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        private void MoveSnake()
        {
            for (int i = _snakeParts.Count - 1; i > 0; i--)
            {
                _snakeParts[i] = _snakeParts[i - 1];
            }

            Point head = _snakeParts[0];
            switch (_snakeDirection)
            {
                case SnakeDirection.Left:
                    head.X -= SnakeSquareSize;
                    break;
                case SnakeDirection.Right:
                    head.X += SnakeSquareSize;
                    break;
                case SnakeDirection.Up:
                    head.Y -= SnakeSquareSize;
                    break;
                case SnakeDirection.Down:
                    head.Y += SnakeSquareSize;
                    break;
            }
            _snakeParts[0] = head;

            if (head == _snakeFood)
            {
                _snakeParts.Add(_snakeParts[_snakeParts.Count - 1]);
                DrawSnakeFood();
            }

            DrawSnake();
        }

        private void DrawSnake()
        {
            GameCanvas.Children.Clear();
            foreach (Point part in _snakeParts)
            {
                Rectangle rect = new Rectangle
                {
                    Width = SnakeSquareSize,
                    Height = SnakeSquareSize,
                    Fill = Brushes.Green
                };
                Canvas.SetLeft(rect, part.X);
                Canvas.SetTop(rect, part.Y);
                GameCanvas.Children.Add(rect);
            }
        }

        private void DrawSnakeFood()
        {
            Random rand = new Random();
            _snakeFood = new Point(rand.Next(0, (int)GameCanvas.ActualWidth / SnakeSquareSize) * SnakeSquareSize,
                                   rand.Next(0, (int)GameCanvas.ActualHeight / SnakeSquareSize) * SnakeSquareSize);

            Ellipse ellipse = new Ellipse
            {
                Width = SnakeSquareSize,
                Height = SnakeSquareSize,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(ellipse, _snakeFood.X);
            Canvas.SetTop(ellipse, _snakeFood.Y);
            GameCanvas.Children.Add(ellipse);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (_snakeDirection != SnakeDirection.Right)
                        _snakeDirection = SnakeDirection.Left;
                    break;
                case Key.Right:
                    if (_snakeDirection != SnakeDirection.Left)
                        _snakeDirection = SnakeDirection.Right;
                    break;
                case Key.Up:
                    if (_snakeDirection != SnakeDirection.Down)
                        _snakeDirection = SnakeDirection.Up;
                    break;
                case Key.Down:
                    if (_snakeDirection != SnakeDirection.Up)
                        _snakeDirection = SnakeDirection.Down;
                    break;
            }
        }
    }
}
