using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DrawBoard();

            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += Timer_Tick;
        }

        const int columns = 100;
        const int rows = 100;

        bool rightIsClickecd = false;
        bool leftIsClicked = false;

        Rectangle[,] field = new Rectangle[rows, columns];
        private const double cellSiize = 7;
        private const double spaceBetween = 0.1;

        DispatcherTimer timer = new DispatcherTimer();

        #region Drawing board
        /// <summary>
        /// Draws the board filled with 100x100 cells(rectangles).
        /// </summary>
        public void DrawBoard()
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Rectangle rectangle = new Rectangle
                    {
                        Height = cellSiize,
                        Width = cellSiize
                    };
                    rectangle.Fill = Brushes.DarkGray;
                    Board.Children.Add(rectangle);

                    Canvas.SetLeft(rectangle, x * (cellSiize + spaceBetween));
                    Canvas.SetTop(rectangle, y * (cellSiize + spaceBetween));

                    rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
                    rectangle.MouseLeftButtonUp += Rectangle_MouseLeftButtonUp;

                    rectangle.MouseRightButtonDown += Rectangle_MouseRightButtonDown;
                    rectangle.MouseRightButtonUp += Rectangle_MouseRightButtonUp;

                    rectangle.MouseMove += Rectangle_MouseMove;

                    field[x, y] = rectangle;
                }
            }

        }
        #endregion

        /// <summary>
        /// Start/Stop button starts the timer or freezes the board.
        /// </summary>
        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                buttonStartStop.Content = "Start!";
            }
            else
            {
                timer.Start();
                buttonStartStop.Content = "Stop!";
            }
        }

        /// <summary>
        /// Clears the board and creates a new one.
        /// </summary>
        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            Board.Children.Clear();
            DrawBoard();
        }

        #region Timer and game logic
        /// <summary>
        /// Timer is holdiing all the logic and will start the whole game.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            //Using the position of specific cell we are going to look for the number of neighbours
            int[,] numberOfNeighbors = new int[columns, rows];

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighbour = 0;

                    int xAbove = x - 1;

                    if (xAbove < 0) { xAbove = columns - 1; }

                    int xUnder = x + 1;
                    if (xUnder >= columns) { xUnder = 0; }

                    int yLeft = y - 1;
                    if (yLeft < 0) { yLeft = rows - 1; }

                    int yRight = y + 1;
                    if (yRight >= rows) { yRight = 0; }

                    if (field[xAbove, yLeft].Fill == Brushes.Green) { neighbour++; }
                    if (field[xAbove, y].Fill == Brushes.Green) { neighbour++; }
                    if (field[xAbove, yRight].Fill == Brushes.Green) { neighbour++; }
                    if (field[x, yLeft].Fill == Brushes.Green) { neighbour++; }
                    if (field[x, yRight].Fill == Brushes.Green) { neighbour++; }
                    if (field[xUnder, yLeft].Fill == Brushes.Green) { neighbour++; }
                    if (field[xUnder, y].Fill == Brushes.Green) { neighbour++; }
                    if (field[xUnder, yRight].Fill == Brushes.Green) { neighbour++; }

                    numberOfNeighbors[x, y] = neighbour;
                }
            }

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (numberOfNeighbors[x, y] < 2 || numberOfNeighbors[x, y] > 3)
                    {
                        field[x, y].Fill = Brushes.DarkGray;
                    }

                    if (numberOfNeighbors[x, y] == 3)
                    {
                        field[x, y].Fill = Brushes.Green;
                    }
                }
            }
        }
        #endregion

        #region Mouse click and mouse hold events
        /// <summary>
        /// Fills the cell with green(alive) on left button click. 
        /// Sets leftIsClicked to true for Rectangle_MouseMove event.
        /// </summary>
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Fill = Brushes.Green;

            leftIsClicked = true;
        }

        /// <summary>
        /// Sets leftIsClicked to false if user doesn't hold left mouse button anymore for Rectangle_MouseMove event.
        /// </summary>
        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            leftIsClicked = false;
        }


        /// <summary>
        /// Fills the cell with grey(dead) on right button click. Sets rightIsClicked to true for Rectangle_MouseMove event.
        /// </summary>
        private void Rectangle_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Fill = Brushes.DarkGray;

            rightIsClickecd = true;
        }

        /// <summary>
        /// Sets rightIsClicked to false if user doesn't hold right mouse button anymore for Rectangle_MouseMove event.
        /// </summary>
        private void Rectangle_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            rightIsClickecd = false;
        }

        /// <summary>
        /// Fills the cell with green(alive) or gray(dead) if user is holding left or right mouse button. 
        /// </summary>
        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftIsClicked)
            {
                ((Rectangle)sender).Fill = Brushes.Green;
            }

            if (rightIsClickecd)
            {
                ((Rectangle)sender).Fill = Brushes.DarkGray;
            }
        }
        #endregion
      
    }
}

