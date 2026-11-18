using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace falppybirdnocopyrights
{
    
    public class PipePair
    {
        public Rectangle TopPipe { get; set; }
        public Rectangle BottomPipe { get; set; }
        public bool Passed { get; set; } = false;
    }

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
        private bool Ho = false;

        
        private List<PipePair> pipePairs = new List<PipePair>();
        private double pipeSpeed = 10;

        private Rectangle fog1 = new Rectangle
        {
            Width = 550,
            Height = 600,
            Fill = new SolidColorBrush(Color.FromArgb(250, 200, 200, 200))
        };

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += Timer_Tick;

            // Generate initial pipes in code
            InitializePipes();
        }

        private void InitializePipes()
        {
            
            foreach (var pair in pipePairs)
            {
                GameCanvas.Children.Remove(pair.TopPipe);
                GameCanvas.Children.Remove(pair.BottomPipe);
            }
            pipePairs.Clear();

            
            double startX = 500;
            double spacing = 300; 

            for (int i = 0; i < 3; i++)
            {
                Rectangle topPipe = new Rectangle
                {
                    Width = 50,
                    Height = 200,
                    Fill = Brushes.Green,
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 4, 35, 3)),
                    StrokeThickness = 10
                };

                Rectangle bottomPipe = new Rectangle
                {
                    Width = 50,
                    Height = 200,
                    Fill = Brushes.Green,
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 4, 35, 3)),
                    StrokeThickness = 10
                };

                
                double topPipeBottomY = new Random().Next(-120, 0);
                Canvas.SetTop(topPipe, topPipeBottomY);
                Canvas.SetLeft(topPipe, startX);

                Canvas.SetTop(bottomPipe, topPipeBottomY + topPipe.Height + new Random().Next(140, 180));
                Canvas.SetLeft(bottomPipe, startX);

                
                GameCanvas.Children.Add(topPipe);
                GameCanvas.Children.Add(bottomPipe);

                
                pipePairs.Add(new PipePair { TopPipe = topPipe, BottomPipe = bottomPipe });

                startX += spacing; 
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Mozog(Bird);
        }

        private void Mozog(Rectangle bird)
        {
            double birdLeft = Canvas.GetLeft(bird);
            double birdTop = Canvas.GetTop(bird);

            
            birdVelocity += gravity;
            double birdNewTop = birdTop + birdVelocity;
            Canvas.SetTop(bird, birdNewTop);

            
            if (birdNewTop <= maxhegit || birdNewTop >= minhegit)
            {
                GameOver(bird);
                return;
            }

            //a csovek loop-olása a listában
            foreach (var pair in pipePairs)
            {
                double pipeLeft = Canvas.GetLeft(pair.TopPipe);
                double newPipeLeft = pipeLeft - pipeSpeed;

                Canvas.SetLeft(pair.TopPipe, newPipeLeft);
                Canvas.SetLeft(pair.BottomPipe, newPipeLeft);

               
                if (newPipeLeft < -50)
                {
                    
                    double maxLeft = 0;
                    foreach (var p in pipePairs)
                    {
                        double currentLeft = Canvas.GetLeft(p.TopPipe);
                        if (currentLeft > maxLeft) maxLeft = currentLeft;
                    }

                    Canvas.SetLeft(pair.TopPipe, maxLeft + 300);
                    Canvas.SetLeft(pair.BottomPipe, maxLeft + 300);

                    double topPipeBottomY = new Random().Next(-120, 0);
                    Canvas.SetTop(pair.TopPipe, topPipeBottomY);
                    Canvas.SetTop(pair.BottomPipe, topPipeBottomY + pair.TopPipe.Height + new Random().Next(140, 180));
                    pair.Passed = false; 
                }

               
                bool isInsideHorizontalBounds = (birdLeft + bird.Width > newPipeLeft) && (birdLeft < newPipeLeft + pair.TopPipe.Width);

                if (isInsideHorizontalBounds)
                {
                    bool hitsTopPipe = birdNewTop < Canvas.GetTop(pair.TopPipe) + pair.TopPipe.Height;
                    bool hitsBottomPipe = birdNewTop + bird.Height > Canvas.GetTop(pair.BottomPipe);

                    if (hitsTopPipe || hitsBottomPipe)
                    {
                        GameOver(bird);
                        return;
                    }
                }

                
                if (!pair.Passed && birdLeft > newPipeLeft + pair.TopPipe.Width)
                {
                    score++;
                    ScoreText.Text = $"Pontszám: {score}";
                    pair.Passed = true;
                }
            }

            
            Idojaras();
        }

        private void Idojaras()
        {
            if (rain == true)
            {
                
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
                Canvas.SetLeft(fog1, 250);
                Canvas.SetTop(fog1, 0);
                if (!GameCanvas.Children.Contains(fog1))
                {
                    GameCanvas.Children.Add(fog1);
                }
                

                if (new Random().Next(0, 100) < 2)
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
            else if (kod == false)
            {
                if (GameCanvas.Children.Contains(fog1))
                {
                    GameCanvas.Children.Remove(fog1);
                }
            }

            if (Ho == true)
            {
                pipeSpeed = 20;

                if (new Random().Next(0, 100) < 50)
                {
                    Rectangle snowdrop = new Rectangle
                    {
                        Width = 5,
                        Height = 5,
                        Fill = Brushes.White
                    };
                    Canvas.SetLeft(snowdrop, new Random().Next(0, (int)GameCanvas.ActualWidth));
                    Canvas.SetTop(snowdrop, 0);
                    GameCanvas.Children.Add(snowdrop);

                    DispatcherTimer rainTimer = new DispatcherTimer();
                    rainTimer.Interval = TimeSpan.FromMilliseconds(20);
                    rainTimer.Tick += (s, args) =>
                    {
                        double rainTop = Canvas.GetTop(snowdrop);
                        Canvas.SetTop(snowdrop, rainTop + 5);
                        if (rainTop > GameCanvas.ActualHeight)
                        {
                            GameCanvas.Children.Remove(snowdrop);
                            rainTimer.Stop();
                        }
                    };
                    rainTimer.Start();
                }
            }
        }

        private static ImageBrush Kepek(string szo)//AI tok bonyolult, szerinte majd megertewm
        {
            string fileName = "normal.png"; // default

            if (szo == "eso") fileName = "eso.png";
            else if (szo == "kod") fileName = "kod.png";
            else if (szo == "ho") fileName = "ho.png";

            try
            {
                ImageBrush brush = new ImageBrush();
                
                string uriString = $"pack://application:,,,/{fileName}";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(uriString, UriKind.Absolute); 
                bitmap.CacheOption = BitmapCacheOption.OnLoad; 
                bitmap.EndInit();

                brush.ImageSource = bitmap;
                return brush;
            }
            catch
            {
                // Fallback color if image is missing so the app doesn't crash
                return new ImageBrush(new BitmapImage());
            }
        }

        private void GameOver(Rectangle bird)
        {
            timer.Stop();
            MessageBox.Show($"Vesztettél, eredmény: {score}");
            ResetGame(bird);
        }

        private void ResetGame(Rectangle bird)
        {
            Canvas.SetLeft(bird, 100);
            Canvas.SetTop(bird, 150);
            score = 0;
            ScoreText.Text = $"Pontszám: {score}";
            rain = false;
            esos.IsEnabled = true;
            
            birdVelocity = 0;
            InitializePipes(); // Reset pipe positions
            pipeSpeed = 10;
            string szo = "normal";
            GameCanvas.Background = Kepek(szo);
        }

        private void Sima(object sender, RoutedEventArgs e)
        {
            rain = false;
            esos.IsEnabled = true;
            

            kod = false;
            kodos.IsEnabled = true;

            Ho = false;
            karacsony.IsEnabled = true;

            string szo = "normal";
            GameCanvas.Background = Kepek(szo);
        }

        private void Esos(object sender, RoutedEventArgs e)
        {
            rain = true;
            esos.IsEnabled = false;
            string szo = "eso";
            GameCanvas.Background = Kepek(szo);
        }

        private void Kodos(object sender, RoutedEventArgs e)
        {
            kod = true;
            kodos.IsEnabled = false;
            string szo = "kod";
            GameCanvas.Background = Kepek(szo);
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

        private void Karacsony(object sender, RoutedEventArgs e)
        {
            Ho = true;
            karacsony.IsEnabled = false;
            string szo = "ho";
            GameCanvas.Background = Kepek(szo);
        }
    }
}