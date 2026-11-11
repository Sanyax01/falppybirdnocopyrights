using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace falppybirdnocopyrights
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int score = 0;
        private DispatcherTimer timer;
        private int maxhegit = 0;
        private int minhegit = 330;
        private bool rain= false;
        private int ugras = 40;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Mozog(Bird, PipeTop, PipeBottom);
        }

        private void Mozog(Rectangle bird, Rectangle pipeTop, Rectangle pipeBottom)
        {
            double birdLeft = Canvas.GetLeft(bird);
            double birdTop = Canvas.GetTop(bird);
            double pipeLeft = Canvas.GetLeft(pipeTop);
            double pipeSpeed = 10;
            double newPipeLeft = pipeLeft - pipeSpeed;
            Canvas.SetLeft(pipeTop, newPipeLeft);
            Canvas.SetLeft(pipeBottom, newPipeLeft);


            if (newPipeLeft < -50)
            {
                Canvas.SetLeft(pipeTop, 800);
                Canvas.SetLeft(pipeBottom, 800);
                double topPipeBottomY = new Random().Next(-120, 0);
                Canvas.SetTop(pipeTop, topPipeBottomY);

                Canvas.SetTop(pipeBottom, topPipeBottomY + pipeTop.Height + new Random().Next(140, 180));
            }

            double birdNewTop = birdTop + 5;
            Canvas.SetTop(bird, birdNewTop);

            if (birdNewTop <= maxhegit || birdNewTop >= minhegit)
            {
                timer.Stop();
                MessageBox.Show($"Vesztettél, eredmény: {score}");
                ResetGame(bird, pipeTop, pipeBottom);
                return;
            }

            bool isInsideHorizontalBounds = (birdLeft + bird.Width > newPipeLeft) && (birdLeft < newPipeLeft + pipeTop.Width);

            if (isInsideHorizontalBounds)
            {
                bool hitsTopPipe = birdNewTop < Canvas.GetTop(pipeTop) + pipeTop.Height;
                bool hitsBottomPipe = birdNewTop + bird.Height > Canvas.GetTop(pipeBottom);

                if (hitsTopPipe || hitsBottomPipe)
                {
                    timer.Stop();
                    MessageBox.Show($"Vesztettél, eredmény: {score}");
                    ResetGame(bird, pipeTop, pipeBottom);
                }
            }
            score++;
            //---------------------------eso
            
            if (rain == true)
            {
                GameCanvas.Background = new SolidColorBrush(Colors.DarkCyan);
                if (new Random().Next(0, 100) < 2)
                {
                    Rectangle rain = new Rectangle
                    {
                        Width = 5,
                        Height = 15,
                        Fill = Brushes.Blue
                    };
                    Canvas.SetLeft(rain, new Random().Next(0, (int)GameCanvas.ActualWidth));
                    Canvas.SetTop(rain, 0);
                    GameCanvas.Children.Add(rain);
                    DispatcherTimer rainTimer = new DispatcherTimer();
                    rainTimer.Interval = TimeSpan.FromMilliseconds(20);
                    rainTimer.Tick += (s, args) =>
                    {
                        double rainTop = Canvas.GetTop(rain);
                        Canvas.SetTop(rain, rainTop + 5);
                        if (rainTop > GameCanvas.ActualHeight)
                        {
                            GameCanvas.Children.Remove(rain);
                            rainTimer.Stop();
                        }
                    };
                    rainTimer.Start();
                }
            }
            
        }

        private void ResetGame(Rectangle bird, Rectangle pipeTop, Rectangle pipeBottom)
        {
            Canvas.SetLeft(bird, 100);
            Canvas.SetTop(bird, 150);
            Canvas.SetLeft(pipeTop, 500);
            Canvas.SetLeft(pipeBottom, 500);
            score = 0;
            rain = false;
            esos.IsEnabled = true;
            GameCanvas.Background = new SolidColorBrush(Colors.LightBlue);
            ugras = 40;
        }


        //-----------------------------------------------------------------

        private void Sima(object sender, RoutedEventArgs e)
        {

        }

        private void Esos(object sender, RoutedEventArgs e)
        {
            rain = true;
            esos.IsEnabled = false;
        }

        private void Kodos(object sender, RoutedEventArgs e)
        {

        }

        private void Start(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void SpaceJump(object sender, RoutedEventArgs e)
        {
            double FentLentPozicio = Canvas.GetTop(Bird);
            if (rain == true) 
            {
                ugras = 20;
                FentLentPozicio -= ugras;
            }
            else
            {
                FentLentPozicio -= ugras;
            }

            Canvas.SetTop(Bird, FentLentPozicio);
            
        }
    }
}