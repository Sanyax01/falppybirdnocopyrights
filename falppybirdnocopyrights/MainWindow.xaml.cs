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

        private void Mozog(Rectangle bird, Rectangle torony1, Rectangle torony2)
        {
            double Pozicio = Canvas.GetLeft(bird);
            double torony1Pozicio = Canvas.GetLeft(torony1);
            double torony2Pozicio = Canvas.GetLeft(torony2);
            double toronySebesseg = 10;
            double ujTorony1Pozicio = torony1Pozicio - toronySebesseg;
            double ujTorony2Pozicio = torony2Pozicio - toronySebesseg;
            Canvas.SetLeft(torony1, ujTorony1Pozicio);
            Canvas.SetLeft(torony2, ujTorony2Pozicio);
            if (ujTorony1Pozicio < -50)
            {
                Canvas.SetLeft(torony1, 800);
                Canvas.SetLeft(torony2, 800);
                Canvas.SetTop(torony2, new Random().Next(-120, 0));
                Canvas.SetTop(torony1, Canvas.GetTop(torony2) + new Random().Next(280, 300));
            }

            double FentLentPozicio = Canvas.GetTop(bird);
            FentLentPozicio += 5;
            Canvas.SetTop(bird, FentLentPozicio);
            if (FentLentPozicio <= maxhegit ) 
            {
                timer.Stop();
                MessageBox.Show($"Vesztettél, eredmény: {score}");
            }
            if (FentLentPozicio >= minhegit)
            {
                timer.Stop();
                MessageBox.Show($"Vesztettél, eredmény: {score}");
            }
            /*if 
                (
                    (Pozicio + bird.Width >= torony1Pozicio && Pozicio <= torony1Pozicio + torony1.Width && FentLentPozicio <= Canvas.GetTop(torony2) + torony2.Height) ||
                    (Pozicio + bird.Width >= torony2Pozicio && Pozicio <= torony2Pozicio + torony2.Width && FentLentPozicio + bird.Height >= Canvas.GetTop(torony1))
                )
            {
                timer.Stop();
                MessageBox.Show($"Vesztettél, eredmény: {score}");
                Canvas.SetLeft(bird, 0);
                Canvas.SetLeft(torony1, 500);
                Canvas.SetLeft(torony2, 500);
                score = 0;
            }*/

            if (Canvas.GetLeft(torony1) < Canvas.GetLeft(bird) + bird.Width &&
                Canvas.GetLeft(torony1) + torony1.Width > Canvas.GetLeft(bird) &&
                (Canvas.GetTop(bird) < Canvas.GetTop(torony1) + torony1.Height ||
                 Canvas.GetTop(bird) + bird.Height > Canvas.GetTop(torony2)))
            {
                timer.Stop();
                MessageBox.Show($"Vesztettél, eredmény: {score}");
                Canvas.SetLeft(bird, 0);
                Canvas.SetLeft(torony1, 500);
                Canvas.SetLeft(torony2, 500);
                score = 0;
            }
           

            score ++;
        }


        //-----------------------------------------------------------------

        private void Sima(object sender, RoutedEventArgs e)
        {

        }

        private void Esos(object sender, RoutedEventArgs e)
        {

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
            FentLentPozicio -= 40;
            Canvas.SetTop(Bird, FentLentPozicio);
            
        }
    }
}