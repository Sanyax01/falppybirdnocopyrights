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
            double sebesseg = 5;
            double ujPozicio = Pozicio + sebesseg;
            Canvas.SetLeft(bird, ujPozicio);
            double torony1Pozicio = Canvas.GetLeft(torony1);
            double torony2Pozicio = Canvas.GetLeft(torony2);
            double toronySebesseg = 5;
            double ujTorony1Pozicio = torony1Pozicio - toronySebesseg;
            double ujTorony2Pozicio = torony2Pozicio - toronySebesseg;
            Canvas.SetLeft(torony1, ujTorony1Pozicio);
            Canvas.SetLeft(torony2, ujTorony2Pozicio);


            if (Pozicio >= Canvas.GetBottom(torony1) || Pozicio >= Canvas.GetTop(torony2))
            {
                timer.Stop();
                MessageBox.Show($"Vesztettél, eredmény: {score}");
                Canvas.SetLeft(bird, 0);
                score = 0;
            }
            score += 1;
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
    }
}