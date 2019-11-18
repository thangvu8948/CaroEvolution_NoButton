using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CaroEvolution_NoButton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int[,] _a;

        const int Rows = 6;
        const int Cols = 6;
        const int startX = 60;
        const int startY = 60;
        const int width = 50;
        const int height = 50;
        bool gameOver = false;
        const int WinCondition = 5;

        int couldPlace = Rows * Cols;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _a = new int[Rows, Cols];

            for (int i = 1; i < Cols; i++)
            {
                var line1 = new Line();
                line1.StrokeThickness = 1;
                line1.Stroke = new SolidColorBrush(Colors.Red);
                canvas.Children.Add(line1);

                line1.X1 = startX + width * i;
                line1.Y1 = startY;

                line1.X2 = startX + width * i;
                line1.Y2 = startY + Cols * height;
            }


            //var line2 = new Line();
            //line2.StrokeThickness = 1;
            //line2.Stroke = new SolidColorBrush(Colors.Red);
            //canvas.Children.Add(line2);

            //line2.X1 = startX + 2 * width;
            //line2.Y1 = startY;

            //line2.X2 = startX + 2 * width;
            //line2.Y2 = startY + 3 * height;

            for (int i = 1; i < Rows; i++)
            {
                var line3 = new Line();
                line3.StrokeThickness = 1;
                line3.Stroke = new SolidColorBrush(Colors.Red);
                canvas.Children.Add(line3);

                line3.X1 = startX;
                line3.Y1 = startY + height * i;

                line3.X2 = startX + Rows * width;
                line3.Y2 = startY + i * height;

            }


            //var line4 = new Line();
            //line4.StrokeThickness = 1;
            //line4.Stroke = new SolidColorBrush(Colors.Red);
            //canvas.Children.Add(line4);

            //line4.X1 = startX;
            //line4.Y1 = startY + 2 * height;

            //line4.X2 = startX + 3 * width;
            //line4.Y2 = startY + 2 * height;
        }




        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);

            // Cot 
            int i = ((int)position.X - startX) / height;
            //Dong
            int j = ((int)position.Y - startY) / width;

            if (i >= Rows || j >= Cols) return;
            if (_a[i, j] == 0 && !gameOver)
            {
                if (isXTurn)
                {
                    DrawImage(i, j);
                    _a[i, j] = 1;
                }
                else
                {
                    DrawImage(i, j);
                    _a[i, j] = 2;
                }
                isXTurn = !isXTurn;

                couldPlace -= 1;

                var (gameOver, xWin) = checkWin(_a, i, j);
                if (gameOver)
                {
                    string result = (xWin == 1) ? "X Win" : (xWin == 2) ? "O Win" : "Draw";

                    MessageBoxResult msbResult = MessageBox.Show($"{result}\nNew Game?", "Caro", MessageBoxButton.YesNo);
                    switch (msbResult)
                    {
                        case MessageBoxResult.Yes:
                            NewGame();
                            break;
                        case MessageBoxResult.No:
                            break;
                    }

                }
            }
        }

        bool isXTurn = true;

        void DrawImage(int i, int j)
        {
            Uri uri;

            if (isXTurn)
            {
                uri = new Uri("./assets/cross.png", UriKind.Relative);
            }
            else
            {
                uri = new Uri("./assets/circle.png", UriKind.Relative);
            }

            Image image = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.EndInit();
            image.Source = bitmap;
            canvas.Children.Add(image);
            image.Width = width;
            image.Height = height;

            Canvas.SetTop(image, j * height + startX);
            Canvas.SetLeft(image, i * width + startY);
        }

        private (bool, int) checkWin(int[,] a, int i, int j)
        {
            var xWin = 1;

            if (checkWinHorizontal(_a, i, j) || checkWinVertical(_a, i, j)
                || checkWinDiagonal(_a, i, j))
            {
                gameOver = true;
                if (_a[i, j] == 1) xWin = 1;
                if (_a[i, j] == 2) xWin = 2;
                return (gameOver, xWin);
            }

            if (couldPlace == 0)
            {
                gameOver = true;
                xWin = 0;
            }
            return (gameOver, xWin);


        }

        /* Xet chieu ngang*/
        private bool checkWinHorizontal(int[,] a, int i, int j)
        {
            int di = 0;
            int dj = -1;
            int startI = i;
            int startJ = j;
            int count = 1;

            while (-1 != startJ + dj)
            {
                startJ += dj;
                if (_a[i, j] == a[i, startJ])
                {
                    count++;
                }
                else break;
            }

            //Xet theo ben phai
            startJ = j;
            dj = 1;
            while (startJ + dj != Cols)
            {
                startJ += dj;
                if (_a[i, j] == a[i, startJ])
                {
                    count++;
                }
                else break;
            }
            if (count >= WinCondition) return true;
            return false;
        }

        /* Xet chieu doc*/
        private bool checkWinVertical(int[,] a, int i, int j)
        {
            int di = -1;
            int dj = 0;
            int startI = i;
            int startJ = j;
            int count = 1;

            while (-1 != startI + di)
            {
                startI += di;
                if (_a[i, j] == a[startI, j])
                {
                    count++;
                }
                else break;
            }

            startI = i;
            di = 1;
            while (startI + di != Rows)
            {
                startI += di;
                if (_a[i, j] == a[startI, j])
                {
                    count++;
                }
                else break;
            }

            if (count >= WinCondition) return true;
            return false;
        }
        /* Xet duong cheo */
        private bool checkWinDiagonalToRight(int[,] a, int i, int j)
        {
            int di = -1;
            int dj = -1;
            int startI = i;
            int startJ = j;
            int count = 1;

            //xet cheo tren ben trai
            while (-1 != startI + di && -1 != startJ + dj)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])
                {
                    count++;
                }
                else break;
            }


            //xet cheo duoi ben phai
            startI = i;
            startJ = j;
            di = 1;
            dj = 1;
            while (startI + di != Rows && startJ + dj != Cols)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])
                {
                    count++;
                }
                else break;
            }

            if (count >= WinCondition) return true;
            return false;
        }
        private bool checkWinDiagonalToLeft(int[,] a, int i, int j)
        {
            int di = 1;
            int dj = -1;
            int startI = i;
            int startJ = j;
            int count = 1;

            //xet cheo duoi ben trai
            while (Rows != startI + di && -1 != startJ + dj)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])
                {
                    count++;
                }
                else break;
            }


            //xet tren duoi ben phai
            startI = i;
            startJ = j;
            di = -1;
            dj = 1;
            while (startI + di != -1 && startJ + dj != Cols)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])
                {
                    count++;
                }
                else break;
            }

            if (count >= WinCondition) return true;
            return false;
        }

        private bool checkWinDiagonal(int[,] a, int i, int j)
        {
            return checkWinDiagonalToLeft(a, i, j) || checkWinDiagonalToRight(a, i, j);
        }

        private void NewGame()
        {
            //Model And UI
            gameOver = false;
            isXTurn = true;
            couldPlace = Rows * Cols;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    _a[i, j] = 0;

                }
            }
            deleteAll();
        }

        private void deleteAll()
        {
            var images = canvas.Children.OfType<Image>().ToList();
            foreach (var image in images)
            {
                canvas.Children.Remove(image);
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("New Game?", "Caro", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    NewGame();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveGame();
            MessageBox.Show("Game saved");
        }
        private void SaveGame()
        {
            const string filename = "save.txt";

            var writer = new StreamWriter(filename);
            //Dong dau tien la luot di hien tai
            writer.WriteLine(isXTurn ? "X" : "O");

            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    writer.Write($"{_a[i, j]}");
                    if (j != Cols)
                    {
                        writer.Write(" ");
                    }
                }
                writer.WriteLine("");
            }
            writer.Close();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {

                var filename = screen.FileName;

                StreamReader reader = new StreamReader(filename);
                var firstLine = reader.ReadLine();
                
                deleteAll();
                for (int i = 0; i < Rows; i++)
                {
                    var tokens = reader.ReadLine().Split(
                        new string[] { " " }, StringSplitOptions.None);
                    //Model

                    for (int j = 0; j < Cols; j++)
                    {
                        _a[i, j] = int.Parse(tokens[j]);
                        //UI
                        if (_a[i, j] == 1)
                        {
                            isXTurn = true;
                            DrawImage(i, j);
                            
                        }
                        if (_a[i, j] == 2)
                        {
                            isXTurn = false;
                            DrawImage(i, j);
                            
                        }
                    }
                }
                isXTurn = firstLine == "X";
                MessageBox.Show("Game is Loaded");
                reader.Close();
            }

            
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //do my stuff before closing
            e.Cancel = true;
            MessageBoxResult result = MessageBox.Show("Do you want save before exit?", "Caro", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SaveGame();
                    e.Cancel = false;
                    break;
                case MessageBoxResult.No:
                    e.Cancel = false;
                    base.OnClosing(e);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
            base.OnClosing(e);
        }
    }
}
