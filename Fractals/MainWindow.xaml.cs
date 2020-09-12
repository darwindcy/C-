using System;
using System.Collections.Generic;
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
using System.ComponentModel;

namespace Fractals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Color[] ColorArray = new Color[64];

        WriteableBitmap wbmap = new WriteableBitmap(1000, 750, 300, 300, PixelFormats.Bgra32, null);

        System.Windows.Threading.DispatcherTimer tmr1 = new System.Windows.Threading.DispatcherTimer();
        
        string fractal = "mandelbrot";
        double WorldXmin = -4, WorldXmax = 4;
        double WorldYmin = -3, WorldYmax = 3;
        double centerX = 0, centerY = 0;
        int iteration = 5;

        MediaPlayer mp1 = new MediaPlayer();

        static BackgroundWorker bw1 = new BackgroundWorker();
        static BackgroundWorker bw2 = new BackgroundWorker();
        static BackgroundWorker bw3 = new BackgroundWorker();
        static BackgroundWorker bw4 = new BackgroundWorker();
        static BackgroundWorker bw5 = new BackgroundWorker();
        static BackgroundWorker bw6 = new BackgroundWorker();
        static BackgroundWorker bw7 = new BackgroundWorker();
        static BackgroundWorker bw8 = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            initializeColorArray();

            setBW();

            tmr1.Tick += new EventHandler(Tmr1_tick);
            tmr1.Interval = TimeSpan.FromMilliseconds(100);
            //Display();

        }

        private void Tmr1_tick(object sender, EventArgs e)
        {
            btnZoom_Click(null, null);
        }
        private void setBW()
        {
            bw1.DoWork += new DoWorkEventHandler(Screen1);
            //bw1.ProgressChanged += new ProgressChangedEventHandler(bw1_ProgressChanged);
            //bw1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw1_RunWorkerCompleted);
            bw1.WorkerReportsProgress = true;
            bw1.WorkerSupportsCancellation = true;

            bw2.DoWork += new DoWorkEventHandler(Screen2);
            bw2.WorkerReportsProgress = true;
            bw2.WorkerSupportsCancellation = true;

            bw3.DoWork += new DoWorkEventHandler(Screen3);
            bw3.WorkerReportsProgress = true;
            bw3.WorkerSupportsCancellation = true;

            bw4.DoWork += new DoWorkEventHandler(Screen4);
            bw4.WorkerReportsProgress = true;
            bw4.WorkerSupportsCancellation = true;

            bw5.DoWork += new DoWorkEventHandler(Screen5);
            bw5.WorkerReportsProgress = true;
            bw5.WorkerSupportsCancellation = true;

            bw6.DoWork += new DoWorkEventHandler(Screen6);
            bw6.WorkerReportsProgress = true;
            bw6.WorkerSupportsCancellation = true;
            
            bw7.DoWork += new DoWorkEventHandler(Screen7);
            bw7.WorkerReportsProgress = true;
            bw7.WorkerSupportsCancellation = true;
            
            bw8.DoWork += new DoWorkEventHandler(Screen8);
            bw8.WorkerReportsProgress = true;
            bw8.WorkerSupportsCancellation = true;

        }

        private void Display()
        {

            if (!bw1.IsBusy)
                bw1.RunWorkerAsync();
            if (!bw2.IsBusy)
                bw2.RunWorkerAsync();
            if (!bw3.IsBusy)
                bw3.RunWorkerAsync();
            if (!bw4.IsBusy)
                bw4.RunWorkerAsync();
            if (!bw5.IsBusy)
                bw5.RunWorkerAsync();
            if (!bw6.IsBusy)
                bw6.RunWorkerAsync();
            if (!bw7.IsBusy)
                bw7.RunWorkerAsync();
            if (!bw8.IsBusy)
                bw8.RunWorkerAsync();

            /*                bw1.RunWorkerAsync();

                            bw2.RunWorkerAsync();

                            bw3.RunWorkerAsync();

                            bw4.RunWorkerAsync();

                            bw5.RunWorkerAsync();

                            bw6.RunWorkerAsync();

                            bw7.RunWorkerAsync();

                            bw8.RunWorkerAsync();*/

            imgMain.Source = wbmap;
        }
        public void initializeColorArray()
        {
            //ColorArray[0] = Color.FromArgb(255, 255, 0, 0);
            ColorArray[0] = Colors.Black;
            ColorArray[1] = Colors.Red;
            ColorArray[2] = Colors.Green;
            ColorArray[3] = Colors.Blue;
            ColorArray[4] = Colors.CadetBlue;
            ColorArray[5] = Colors.Yellow;
            ColorArray[6] = Colors.Orange;
            ColorArray[7] = Colors.LightYellow;
            ColorArray[8] = Colors.Violet;
            ColorArray[9] = Colors.Brown;
            ColorArray[10] = Colors.Gray;
            ColorArray[11] = Colors.Pink;
            ColorArray[12] = Colors.Purple;
            ColorArray[13] = Colors.AliceBlue;
            ColorArray[14] = Colors.Aqua;
            ColorArray[15] = Colors.Aquamarine;
            ColorArray[16] = Colors.BlanchedAlmond;

            ColorArray[17] = Colors.BlueViolet;
            ColorArray[18] = Colors.Chocolate;
            ColorArray[19] = Colors.Coral;
            ColorArray[20] = Colors.Cyan;
            ColorArray[21] = Colors.DarkBlue;
            ColorArray[22] = Colors.DarkCyan;
            ColorArray[23] = Colors.LightGreen;
            ColorArray[24] = Colors.CornflowerBlue;
            ColorArray[25] = Colors.DarkGoldenrod;
            ColorArray[26] = Colors.DarkKhaki;
            ColorArray[27] = Colors.DarkOliveGreen;
            ColorArray[28] = Colors.DarkOrange;
            ColorArray[29] = Colors.DarkOrchid;
            ColorArray[30] = Colors.DarkRed;
            ColorArray[31] = Colors.DarkSeaGreen;
            ColorArray[32] = Colors.DarkSlateGray;
            ColorArray[33] = Colors.DarkViolet;

            ColorArray[34] = Colors.LightBlue;
            ColorArray[35] = Colors.LightCoral;
            ColorArray[36] = Colors.LightCyan;
            ColorArray[37] = Colors.LightGoldenrodYellow;
            ColorArray[38] = Colors.LightGray;
            ColorArray[39] = Colors.LightGreen;
            ColorArray[40] = Colors.LightPink;
            ColorArray[41] = Colors.LightSalmon;
            ColorArray[42] = Colors.LightSeaGreen;
            ColorArray[43] = Colors.LightSkyBlue;
            ColorArray[44] = Colors.LightSlateGray;
            ColorArray[45] = Colors.LightSteelBlue;
            ColorArray[46] = Colors.IndianRed;
            ColorArray[47] = Colors.OrangeRed;
            ColorArray[48] = Colors.PaleVioletRed;
            ColorArray[49] = Colors.DodgerBlue;
            ColorArray[50] = Colors.HotPink;

            ColorArray[51] = Colors.Indigo;
            ColorArray[52] = Colors.Khaki;
            ColorArray[53] = Colors.Ivory;
            ColorArray[54] = Colors.Gold;
            ColorArray[55] = Colors.Silver;
            ColorArray[56] = Colors.Turquoise;
            ColorArray[57] = Colors.PaleTurquoise;
            ColorArray[58] = Colors.Lavender;
            ColorArray[59] = Colors.MediumAquamarine;
            ColorArray[60] = Colors.MediumSlateBlue;
            ColorArray[61] = Colors.MediumSpringGreen;
            ColorArray[62] = Colors.MediumVioletRed;
            ColorArray[63] = Colors.Teal;

        }

        private void performWork(int startX, int startY)
        {
            int endX = startX + 1000/4;
            int endY = startY + 750/2;

            Complex[] compArray = new Complex[64];
            compArray[0] = new Complex();
/*            if (fractal == "julia")
                compArray[0] = new Complex(-0.75, 0);*/

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    Complex c = new Complex(XWorld(x), YWorld(y));

                    Complex juliaA = new Complex(XWorld(y), YWorld(x));

                    Complex juliaC = new Complex(XWorld(-0.75), YWorld(50));

                    compArray[0] = new Complex();
                    int i = 1;

                    while (i < 64 && compArray[i - 1].Mag() < iteration)
                    {
                        if (fractal == "mandelbrot")
                            compArray[i] = compArray[i - 1] * compArray[i - 1] + c;
                        else if (fractal == "julia") {
                            //compArray[i - 1] = new Complex(compArray[i-1].Imag, compArray[i -1].Real);
                            compArray[i] = (compArray[i - 1] * compArray[i - 1]) + juliaC;
                        } else if (fractal == "nova")
                            compArray[i] = compArray[i - 1] * compArray[i - 1] * compArray[i - 1] + c;
                        else if (fractal == "magnet")
                            compArray[i] = ((compArray[i - 1] * compArray[i - 1]) + c + (compArray[i - 1].multWhole(2)) + c);
                        else if (fractal == "phoenix")
                        {
                            compArray[i] = compArray[i - 1] + c;
                        }
                        i++;
                    }
                    int a = i % 64;
                    DrawBitmap(x, y, a);
                }
            }
        } 
/*        private void work()
        {
            Complex c;

            for (int y = 0; y < 750; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    c = new Complex(XWorld(x, 0), YWorld(y, 0));

                    int count = 0;
                    Complex z = new Complex();
                    Complex z1;

                    while (count < 64 && z.Mag() < 5)
                    {
                        z1 = z + c;
                        z = z1;
                        count++;
                    }
                    int colNum = count % 64;

                    DrawBitmap(x, y, colNum);
                }
            }
            imgMain.Source = wbmap;
        }*/

        private void Screen1(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                //System.Threading.Thread.Sleep(200);
                performWork(0, 0);
                
            });
            
        }

        /*------------------------------------------------------------------------------------------------*/
        private void Screen2(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                performWork(250, 0);
            });     
        }
        private void Screen3(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                performWork(500, 0);
            });
        }
        private void Screen4(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                performWork(750, 0);
            });
        }
        private void Screen5(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                performWork(0, 375);
            });
            
        }
        private void Screen6(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                performWork(250, 375);
            });
            
        }
        private void Screen7(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                performWork(500, 375);
            });
            
        }
        private void Screen8(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                performWork(750, 375);
            });
            
        }

        private void DrawBitmap(int x, int y, int colNum)
        {
            SetPixel(wbmap, x, y, ColorArray[colNum]);
        }
        
        private double PixelToWorld(double Screen, double Wmax, double Wmin, double Smax, double Smin)
        {
            return Screen * ((Wmax - Wmin)/(Smax - Smin));
        }
        
        private double XWorld(double pixelX)
        {
            return PixelToWorld(pixelX, WorldXmax, WorldXmin, imgMain.Width, 0) + (centerX - WorldXmax); 
        }

        private double YWorld(double pixelY)
        {
            return (PixelToWorld(pixelY, WorldYmax, WorldYmin, imgMain.Height, 0) + (centerY - WorldYmax));
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



        private void imgMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(imgMain);
            double x = p.X;
            double y = p.Y;
            centerX = XWorld(x);
            centerY = YWorld(y);
            Display();
/*            
            if(x < 250)
            {

            } else if()*/
        }

        private void btnZoom_Click(object sender, RoutedEventArgs e)
        {
            WorldXmin = WorldXmin + 0.1;
            WorldXmax = WorldXmax - 0.1;
            WorldYmin = WorldYmin + (0.1* 3/4);
            WorldYmax = WorldYmax - (0.1 * 3/4);
            if (WorldXmin == 0 && WorldXmax == 0)
                MessageBox.Show("Fractals Finished");
            Display();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            tmr1.Start();
            mp1.Open(new Uri(@"D:/College Files/Semester 7/CS 376/Project 4/Fractals\Music/Fractal1.mp3"));
            //mp1.Play();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            tmr1.Stop();
            
            mp1.Stop();
        }

        private void cmbChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WorldXmin = -4;
            WorldXmax = 4;
            WorldYmin = -3;
            WorldYmax = 3;

            DisplaySelection();
        }
        private void DisplaySelection()
        {
            if (cmbChoice.SelectedIndex == 0)
            {
                fractal = "mandelbrot";
            }
            else if (cmbChoice.SelectedIndex == 1)
            {
                fractal = "julia";
            }
            else if (cmbChoice.SelectedIndex == 2)
            {
                fractal = "nova";
            }
            else if (cmbChoice.SelectedIndex == 3)
            {
                fractal = "phoenix";
            }
            

            Display();
 
        }
    }
}
