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
        //Новый коментарий
        private const int SnakeSquareSize = 20;
        private const int SnakeStartLength = 5;
        private const int SnakeStartSpeed = 40; // in milliseconds
        private const int SnakeSpeedThreshold = 100;

        private enum SnakeDirection { Left, Right, Up, Down };
        private SnakeDirection _snakeDirection1 = SnakeDirection.Right;
        private SnakeDirection _snakeDirection2 = SnakeDirection.Right;
        private List<Point> _snakeParts1 = new List<Point>();
        private List<Point> _snakeParts2 = new List<Point>();
        private List<Point> _foodItems = new List<Point>();
        private DispatcherTimer _gameTickTimer = new DispatcherTimer();
        private MediaPlayer _mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            _gameTickTimer.Tick += GameTickTimer_Tick;
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(SnakeStartSpeed);
            StartNewGame();
        }

        private void StartNewGame()
        {
            _snakeParts1.Clear();
            _snakeParts2.Clear();
            _foodItems.Clear();
            _snakeDirection1 = SnakeDirection.Right;
            _snakeDirection2 = SnakeDirection.Right;
            for (int i = 0; i < SnakeStartLength; i++)
            {
                _snakeParts1.Add(new Point(SnakeSquareSize * (SnakeStartLength - i - 1), 0));
                _snakeParts2.Add(new Point(SnakeSquareSize * (SnakeStartLength - i - 1), SnakeSquareSize * 2));
            }
            DrawSnakes();
            GenerateFoodItems();
            _gameTickTimer.IsEnabled = true;
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake(_snakeParts1, ref _snakeDirection1);
            MoveSnake(_snakeParts2, ref _snakeDirection2);
            DrawSnakes();
            DrawFoodItems();
        }

        private void MoveSnake(List<Point> snakeParts, ref SnakeDirection snakeDirection)
        {
            for (int i = snakeParts.Count - 1; i > 0; i--)
            {
                snakeParts[i] = snakeParts[i - 1];
            }

            Point head = snakeParts[0];
            switch (snakeDirection)
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
            snakeParts[0] = head;

            for (int i = 0; i < _foodItems.Count; i++)
            {
                if (head == _foodItems[i])
                {
                    snakeParts.Add(snakeParts[snakeParts.Count - 1]);
                    _foodItems.RemoveAt(i);
                    GenerateFoodItems();
                    break;
                }
            }
        }

        private void DrawSnakes()
        {
            GameCanvas.Children.Clear();
            DrawSnake(_snakeParts1, Brushes.Green);
            DrawSnake(_snakeParts2, Brushes.Blue);
        }

        private void DrawSnake(List<Point> snakeParts, Brush color)
        {
            foreach (Point part in snakeParts)
            {
                Rectangle rect = new Rectangle
                {
                    Width = SnakeSquareSize,
                    Height = SnakeSquareSize,
                    Fill = color
                };
                Canvas.SetLeft(rect, part.X);
                Canvas.SetTop(rect, part.Y);
                GameCanvas.Children.Add(rect);
            }
        }

        private void GenerateFoodItems()
        {
            Random rand = new Random();
            for (int i = 0; i < 3; i++)
            {
                Point food = new Point(rand.Next(0, (int)GameCanvas.ActualWidth / SnakeSquareSize) * SnakeSquareSize,
                                       rand.Next(0, (int)GameCanvas.ActualHeight / SnakeSquareSize) * SnakeSquareSize);
                _foodItems.Add(food);
            }
            DrawFoodItems();
        }

        private void DrawFoodItems()
        {
            foreach (Point food in _foodItems)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = SnakeSquareSize,
                    Height = SnakeSquareSize,
                    Fill = Brushes.Red
                };
                Canvas.SetLeft(ellipse, food.X);
                Canvas.SetTop(ellipse, food.Y);
                GameCanvas.Children.Add(ellipse);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            PlayKeyPressSound();
            switch (e.Key)
            {
                // Управление первой змейкой (стрелки)
                case Key.Left:
                    if (_snakeDirection1 != SnakeDirection.Right)
                        _snakeDirection1 = SnakeDirection.Left;
                    break;
                case Key.Right:
                    if (_snakeDirection1 != SnakeDirection.Left)
                        _snakeDirection1 = SnakeDirection.Right;
                    break;
                case Key.Up:
                    if (_snakeDirection1 != SnakeDirection.Down)
                        _snakeDirection1 = SnakeDirection.Up;
                    break;
                case Key.Down:
                    if (_snakeDirection1 != SnakeDirection.Up)
                        _snakeDirection1 = SnakeDirection.Down;
                    break;

                // Управление второй змейкой (WASD)
                case Key.A:
                    if (_snakeDirection2 != SnakeDirection.Right)
                        _snakeDirection2 = SnakeDirection.Left;
                    break;
                case Key.D:
                    if (_snakeDirection2 != SnakeDirection.Left)
                        _snakeDirection2 = SnakeDirection.Right;
                    break;
                case Key.W:
                    if (_snakeDirection2 != SnakeDirection.Down)
                        _snakeDirection2 = SnakeDirection.Up;
                    break;
                case Key.S:
                    if (_snakeDirection2 != SnakeDirection.Up)
                        _snakeDirection2 = SnakeDirection.Down;
                    break;
            }
        }

        private void PlayKeyPressSound()
        {
            _mediaPlayer.Open(new Uri("C:\\Users\\VEPS\\Desktop\\test git\\GitDemo\\GitDemo\\applepay.wav"));
            _mediaPlayer.Play();
        }
    }
}
