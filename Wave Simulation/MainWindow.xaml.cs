// Submitted By: Darwin Charles Yadav
// Project 2: Caverunner: Small Computer Software
// Submitted To: Dr. Dick Blandford

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        WriteableBitmap wbmap = new WriteableBitmap(128, 128, 300, 300, PixelFormats.Bgra32, null);

        Particle[,] initialParticles = new Particle[128, 128];
        Particle[,] finalParticles = new Particle[128, 128];

        Point thePoint;
        ColorDialog MyDialog = new ColorDialog();
        int[,] colorArrray = new int[32, 3];
        int initialRed = 86, initialGreen = 169, initialBlue = 165;
        int finalRed = 2, finalGreen = 21, finalBlue = 253;
        int colorRed = 0, colorGreen = 0, colorBlue = 255; 

        System.Windows.Threading.DispatcherTimer tmr1 = new System.Windows.Threading.DispatcherTimer();

        Boolean waveCreation = false;
        Boolean pointObstacleCreation = false;
        Boolean lineCreation = false;
        Boolean rectangleCreation = false;
        int iterations = 40;

        double angleValue = 1;

        int startPointX = 0;
        int startPointY = 0;


        public MainWindow()
        {
            InitializeComponent();
            
            imgTank.Source = new BitmapImage(new Uri("C:/Users/Darwin/Pictures/background.jpg"));
            imgTank.IsHitTestVisible = false;
            
            // Create the Particle array
            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    initialParticles[i, j] = new Particle();
                    finalParticles[i, j] = new Particle();
                }
            }

            // Set the values to be zero 
            for (int x = 0; x < 40; x++)
            {
                initialParticles[x, x].acceleration = 0;
                initialParticles[x, x].velocity = 0;
                initialParticles[x, x].position = 0;
            }
            createColorArray(initialRed, initialGreen, initialBlue,
                             finalRed, finalGreen, finalBlue);

            //angleValue = Math.Sin(2 * Math.PI * 1 / iterations);

            tmr1.Interval = TimeSpan.FromMilliseconds(1000 / iterations);
            tmr1.Tick += new EventHandler(Tmr_Tick);
            tmr1.Start();

        }

        private void createColorArray(int minRed, int minGreen, int minBlue,
                                      int maxRed, int maxGreen, int maxBlue)
        {
            for (int x = 0; x < 32; x++)
            {
                colorArrray[x, 0] = minRed;
                minRed = minRed + ((minRed - maxRed) / 32);
            }

            for (int x = 0; x < 32; x++)
            {
                colorArrray[x, 1] = minGreen;
                minGreen = minGreen + ((minGreen - maxGreen) / 32);
            }

            for (int x = 0; x < 32; x++)
            {
                colorArrray[x, 2] = minBlue;
                minBlue = minBlue + ((minBlue - maxBlue) / 32);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //wbmap = new WriteableBitmap(128, 128, 300, 300, PixelFormats.Bgra32, null);

            double force;

            angleValue = Math.Sin(2 * Math.PI * 1 / iterations);

            if (startPointX != 0 && startPointY != 0)
            {
                initialParticles[startPointX, startPointY].position = angleValue;
            }

            for (int x = 1; x < 127; x++)
            {
                for (int y = 1; y < 127; y++)
                {
                    if (initialParticles[x, y].isObstacle == false)
                    {
                        force = findNeighborSum(x, y) / 8;
                        force = 2 * force;
                        finalParticles[x, y].acceleration = -force / 4;
                        finalParticles[x, y].velocity = initialParticles[x, y].velocity + initialParticles[x, y].acceleration;
                        finalParticles[x, y].position = initialParticles[x, y].position + initialParticles[x, y].velocity;
                    }
                    else
                    {
                        finalParticles[x, y].acceleration = 0;
                        finalParticles[x, y].velocity = 0;
                        finalParticles[x, y].velocity = 0;
                    }
                }
            }

            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    if (initialParticles[i, j].isObstacle == false)
                    {

                        initialParticles[i, j].acceleration = finalParticles[i, j].acceleration;
                        initialParticles[i, j].velocity = finalParticles[i, j].velocity;
                        initialParticles[i, j].position = finalParticles[i, j].position;
                        initialParticles[i, j].isObstacle = finalParticles[i, j].isObstacle;
                    }

                    /*
                    int x = (int)((initialParticles[i, j].position + 1) * 15);

                    if (x < 32 && x >= 0)
                    {
                        SetPixel(wbmap, i, j, Color.FromArgb(255,
                            (byte)colorArrray[x, 0], (byte)colorArrray[x, 1], (byte)colorArrray[x, 2]));
                    }
                    */

                    initialParticles[startPointX, startPointY].position = angleValue;

                    int x = (int)((initialParticles[i, j].position + 1) * 128);
                    /*
                    if (x < 255 && x >= 0)
                    {
                        SetPixel(wbmap, i, j, Color.FromArgb((byte)x, (byte)colorRed, (byte)colorGreen, (byte)colorBlue));
                    }*/
                    SetPixel(wbmap, i, j, Color.FromArgb((byte)x, (byte)colorRed, (byte)colorGreen, (byte)colorBlue));
                }
            }


            imgTank.Source = wbmap;
        }

        private double findNeighborSum(int x, int y)
        {
            double sum;
            sum = topRightDifference(x, y) +
                    topCenterDifference(x, y) +
                    topLeftDifference(x, y) +
                    middleRightDifference(x, y) +
                    middleLeftDifference(x, y) +
                    bottomRightDifference(x, y) +
                    bottomCenterDifference(x, y) +
                   bottomLeftDifference(x, y);

            return sum;
        }

        private double topRightDifference(int x, int y)
        {
            return (initialParticles[x, y].position - initialParticles[x + 1, y - 1].position);
        }
        private double topCenterDifference(int x, int y)
        {
            return (initialParticles[x, y].position - initialParticles[x, y - 1].position);
        }
        private double topLeftDifference(int x, int y)
        {
                return (initialParticles[x, y].position - initialParticles[x - 1, y - 1].position);
        }
        private double middleRightDifference(int x, int y)
        {
               return (initialParticles[x, y].position - initialParticles[x + 1, y].position);
        }
        private double middleLeftDifference(int x, int y)
        {
               return (initialParticles[x, y].position - initialParticles[x - 1, y].position);
        }
        private double bottomRightDifference(int x, int y)
        {
            return (initialParticles[x, y].position - initialParticles[x + 1, y + 1].position);
        }
        private double bottomCenterDifference(int x, int y)
        {
                return (initialParticles[x, y].position - initialParticles[x, y + 1].position);
        }
        private double bottomLeftDifference(int x, int y)
        {
            double diff = 0;
            if ((x - 1) > 0 && (y + 1) < 128)
                diff = (initialParticles[x, y].position - initialParticles[x - 1, y + 1].position);

            return diff;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        Point startPoint;
        Rectangle rectSelectArea;

        private void ImgTank_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rectangleCreation == true)
            {
                startPoint = e.GetPosition(canImage);

                if (rectSelectArea != null)
                    canImage.Children.Remove(rectSelectArea);

                rectSelectArea = new Rectangle
                {
                    Stroke = Brushes.LightBlue,
                    StrokeThickness = 2
                };

                Canvas.SetLeft(rectSelectArea, startPoint.X);
                Canvas.SetTop(rectSelectArea, startPoint.X);
                canImage.Children.Add(rectSelectArea);
            }

            if (lineCreation == true)
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    startPoint = e.GetPosition(canImage);
            }

            if (pointObstacleCreation == true)
            {
                startPoint = e.GetPosition(canImage);
                initialParticles[(int)startPoint.X / 2, (int)startPoint.Y / 2].isObstacle = true;
            }

        }

        private void ImgTank_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (rectangleCreation == true)
            {
                if (e.LeftButton == MouseButtonState.Released || rectSelectArea == null)
                    return;

                var pos = e.GetPosition(canImage);

                // Set the position of rectangle
                var x = Math.Min(pos.X, startPoint.X);
                var y = Math.Min(pos.Y, startPoint.Y);

                // Set the dimenssion of the rectangle
                var w = Math.Max(pos.X, startPoint.X) - x;
                var h = Math.Max(pos.Y, startPoint.Y) - y;

                rectSelectArea.Width = w;
                rectSelectArea.Height = h;

                Canvas.SetLeft(rectSelectArea, x);
                Canvas.SetTop(rectSelectArea, y);
                //System.Windows.MessageBox.Show(x.ToString() + " , " + y.ToString() + " TO " + (x+w).ToString() + " , " + (y+h).ToString());
                
                for(int a = (int)x; a < x + w; a++)
                {
                    for(int b = (int)y; b < y + h; b++)
                    {
                        initialParticles[a/2, b/2].isObstacle = true; 
                    }
                }
            }

            if (lineCreation == true)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Line line = new Line();

                    line.Stroke = SystemColors.WindowFrameBrush;
                    line.X1 = startPoint.X;
                    line.Y1 = startPoint.Y;
                    line.X2 = e.GetPosition(canImage).X;
                    line.Y2 = e.GetPosition(canImage).Y;

                    startPoint = e.GetPosition(canImage);

                    canImage.Children.Add(line);
                }
            }

        }

        private void ObsLine_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lineCreation = true;
            imgTank.IsHitTestVisible = true;
        }

        private void ObsRect_MouseUp(object sender, MouseButtonEventArgs e)
        {
            rectangleCreation = true;
            imgTank.IsHitTestVisible = true;
        }

        private void ObsPoint_MouseUp(object sender, MouseButtonEventArgs e)
        {
            pointObstacleCreation = true;
            imgTank.IsHitTestVisible = true;
        }

        private void MnuCreateWave_Click(object sender, RoutedEventArgs e)
        {
            waveCreation = true;
            imgTank.IsHitTestVisible = true;
        }

        private void Iter_40_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            tmr1.Stop();
            iterations = 40;
            tmr1.Interval = TimeSpan.FromMilliseconds(1000 / iterations);
            tmr1.Tick += new EventHandler(Tmr_Tick);
            tmr1.Start();
        }

        private void Iter_80_MouseUp(object sender, MouseButtonEventArgs e)
        {
            tmr1.Stop();
            iterations = 80;
            tmr1.Interval = TimeSpan.FromMilliseconds(1000 / iterations);
            tmr1.Tick += new EventHandler(Tmr_Tick);
            tmr1.Start();
        }

        private void Iter_120_MouseUp(object sender, MouseButtonEventArgs e)
        {
            tmr1.Stop();
            iterations = 120;
            tmr1.Interval = TimeSpan.FromMilliseconds(1000 / iterations);
            tmr1.Tick += new EventHandler(Tmr_Tick);
            tmr1.Start();
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            txtAbout.Text = " EE 356 \n Project 1 \n Wave Simulation \n Submitted By: Darwin Yadav";
            if (txtAbout.Visibility == Visibility.Hidden)
                txtAbout.Visibility = Visibility.Visible;
            else
                txtAbout.Visibility = Visibility.Hidden;
        }

        private void WaveSin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //angleValue = 128 * Math.Sin(2 * Math.PI * 1 / iterations);
            angleValue = Math.Sin(2 * Math.PI * 1 / iterations);
        }

        private void WaveCos_MouseUp(object sender, MouseButtonEventArgs e)
        {
            angleValue = Math.Cos(2 * Math.PI * 1 / iterations);
        }

        public void SetPixel(WriteableBitmap wbm, int x, int y, Color c)
        {
            if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1) return;
            if (y < 0 || x < 0) return;
            if (!wbm.Format.Equals(PixelFormats.Bgra32)) return;
            wbm.Lock();
            IntPtr buff = wbm.BackBuffer;
            int Stride = wbm.BackBufferStride;
            unsafe
            {
                byte* pbuff = (byte*)buff.ToPointer();
                int loc = y * Stride + x * 4;
                pbuff[loc] = c.B;
                pbuff[loc + 1] = c.G;
                pbuff[loc + 2] = c.R;
                pbuff[loc + 3] = c.A;
            }
            wbm.AddDirtyRect(new Int32Rect(x, y, 1, 1));
            wbm.Unlock();
        }
        private void Tmr_Tick(object sender, EventArgs e)
        {
            Button_Click(null, null);
        }

        private void ImgTank_MouseUp(object sender, MouseButtonEventArgs e)
        {
            thePoint = e.GetPosition(imgTank);
            //System.Windows.MessageBox.Show(thePoint.X.ToString() + ", " + thePoint.Y.ToString());

            if (waveCreation == true)
            {
                //System.Windows.MessageBox.Show(angleValue.ToString());
                startPointX = (int)thePoint.X / 2;
                startPointY = (int)thePoint.Y / 2;
                initialParticles[(int)thePoint.X / 2, (int)thePoint.Y / 2].position = angleValue;
                if (!tmr1.IsEnabled)
                    tmr1.Start();
            }

            if (pointObstacleCreation == true)
            {
                initialParticles[(int)thePoint.X / 2, (int)thePoint.Y / 2].isObstacle = true;
            }

            // Stop creating wave
            imgTank.IsHitTestVisible = false;
            waveCreation = false;

            rectangleCreation = false;
            lineCreation = false;
            pointObstacleCreation = false;

        }

        private void BtnColor_Click(object sender, RoutedEventArgs e)
        {
            if (MyDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Windows.MessageBox.Show("Color Changed");
                /*
                // minRed = 0; minGreen = 0; minBlue = 0;
                int maxRed = (int)MyDialog.Color.R;
                int maxGreen = (int)MyDialog.Color.G;
                int maxBlue = (int)MyDialog.Color.B;
                createColorArray(0, 0, 0, maxRed, maxGreen, maxBlue);
                */
                colorRed = (int)MyDialog.Color.R;
                colorGreen = (int)MyDialog.Color.G;
                colorBlue = (int)MyDialog.Color.B;
            }
        }
    }
}

