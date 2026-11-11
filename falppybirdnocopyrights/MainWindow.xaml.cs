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
        private double score = 0;
        private DispatcherTimer timer;
        private int maxhegit = 0;
        private int minhegit = 330;
        private bool rain = false;
        private double birdVelocity = 0;       
        private double gravity = 0.5;          
        private double normalJumpStrength = -7; 
        private double rainJumpStrength = -4;   
        private bool kod = false;

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

            
            birdVelocity += gravity;                    // pici ai ez miatt
            double birdNewTop = birdTop + birdVelocity; 
            Canvas.SetTop(bird, birdNewTop);

            if (birdNewTop <= maxhegit || birdNewTop >= minhegit)
            {
                timer.Stop();
                MessageBox.Show($"Vesztettél, eredmény: {score}");
                ResetGame(bird, pipeTop, pipeBottom);
                
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
            if (Canvas.GetLeft(Bird) == Canvas.GetLeft(PipeTop) + PipeTop.Width)
            {
                score ++;
                ScoreText.Text = $"Pontszám: {score}";
            }

            //---------------------------eso
            if (rain == true)
            {
                GameCanvas.Background = new SolidColorBrush(Colors.DarkCyan);
                if (new Random().Next(0, 100) < 50)
                {
                    
                    Rectangle raindrop = new Rectangle
                    {
                        Width = 5,
                        Height = 15,
                        Fill = Brushes.Blue
                    };
                    Canvas.SetLeft(raindrop, new Random().Next(0, (int)GameCanvas.ActualWidth));
                    Canvas.SetTop(raindrop, 0);
                    GameCanvas.Children.Add(raindrop);
                    
                    DispatcherTimer rainTimer = new DispatcherTimer();
                    rainTimer.Interval = TimeSpan.FromMilliseconds(20);
                    rainTimer.Tick += (s, args) =>
                    {
                        double rainTop = Canvas.GetTop(raindrop);
                        Canvas.SetTop(raindrop, rainTop + 5);
                        if (rainTop > GameCanvas.ActualHeight)
                        {
                            GameCanvas.Children.Remove(raindrop);
                            rainTimer.Stop();
                        }
                    };
                    rainTimer.Start();
                }
            }
            if (kod == true)
            {
                if (new Random().Next(0, 100) < 1)
                {
                    Rectangle fog = new Rectangle
                    {
                        Width = 800,
                        Height = 600,
                        Fill = new SolidColorBrush(Color.FromArgb(128, 200, 200, 200))
                    };
                    Canvas.SetLeft(fog, 0);
                    Canvas.SetTop(fog, 0);
                    GameCanvas.Children.Add(fog);
                    DispatcherTimer fogTimer = new DispatcherTimer();
                    fogTimer.Interval = TimeSpan.FromMilliseconds(1000);
                    fogTimer.Tick += (s, args) =>
                    {
                        GameCanvas.Children.Remove(fog);
                        fogTimer.Stop();
                    };
                    fogTimer.Start();
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
            birdVelocity = 0; 
        }

        private void Sima(object sender, RoutedEventArgs e)
        {
            rain = false;
            esos.IsEnabled = true;
            GameCanvas.Background = new SolidColorBrush(Colors.LightBlue);

            kod = false;
            kodos.IsEnabled = true;
        }

        private void Esos(object sender, RoutedEventArgs e)
        {
            rain = true;
            esos.IsEnabled = false;
        }

        private void Kodos(object sender, RoutedEventArgs e)
        {
            kod = true;
            kodos.IsEnabled = false;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void SpaceJump(object sender, RoutedEventArgs e)
        {
            
            if (rain == true) 
            {
                birdVelocity = rainJumpStrength;
            }
            else
            {
                birdVelocity = normalJumpStrength;
            }
        }
    }
}