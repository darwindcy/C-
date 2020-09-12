// Submitted By: Darwin Charles Yadav
// Small Computer Software
// Submitted To: Dr. Dick Blandford
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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

namespace project2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        public class caveRectangle{
            public int startFreeSpacePosition { get; set; }
            public int endFreeSpacePosition { get; set; }
            public caveRectangle()
            {

            }
        }

        System.Windows.Threading.DispatcherTimer tmr1 = new System.Windows.Threading.DispatcherTimer();
        
        int GameSpeed = 0;
        int playerScore = 0;
        private double winWidth, winHeight;
        const int characterHeight = 20;
        const int characterWidth = 40;

        int characterXCoord;
        int characterYCoord;

        int startFreeSpaceYCoord = 0;
        
        const int freeSpaceHeight = 200;           // Free space is about 200px long
        caveRectangle[] caveRunner;

        Boolean gameOver = false;

        public MainWindow()
        {
            InitializeComponent();
            
            lblScore.Foreground = new SolidColorBrush(Colors.Blue);           // Score Label color   

            winWidth = imgPlot.Width; winHeight = imgPlot.Height;

            // Starting character coordinates (5, center of screen)
            characterXCoord = 5;
            characterYCoord = (int)winHeight / 2;

            DrawInitialScreen();
            changeGameSpeed();
            tmr1.Tick += new EventHandler(Tmr_Tick);
            tmr1.Start();
        }
        private void changeGameSpeed()
        {
            GameSpeed++;
            // Change gamespeed as the time
            tmr1.Interval = TimeSpan.FromMilliseconds(1000 / GameSpeed);
        }
        
        private void DrawFramesToScreen()
        {
            if (gameOver)                     // Check for crash on start of every frame change
                CallGameOver();
            Pen blkPen = new Pen(Brushes.Black, 1);
            Brush blueBrush = Brushes.AliceBlue;
            Brush brownBrush = Brushes.Brown;

            // Creating a drawing visual to draw on
            DrawingVisual vis = new DrawingVisual();
            // Create a drawing context for this visual
            DrawingContext dc = vis.RenderOpen();

            Random rnd = new Random();
            Rect upperRect;
            Rect lowerRect;

            Rect CharacterRect = new Rect(characterXCoord, characterYCoord, characterWidth, characterHeight);

            for (int a = 0; a < winWidth - 5; a = a + 5)
            {
                
                // Fill the array with the next frame of the cave
                if(a/5 + 1 <= caveRunner.Length)
                    startFreeSpaceYCoord = caveRunner[a/5 + 1].startFreeSpacePosition;

                upperRect = new Rect(a, 0, 5, startFreeSpaceYCoord);

                dc.DrawRectangle(brownBrush, blkPen, upperRect);

                caveRunner[a / 5].startFreeSpacePosition = startFreeSpaceYCoord;

                caveRunner[a / 5].endFreeSpacePosition = startFreeSpaceYCoord + freeSpaceHeight;

                lowerRect = new Rect(a, startFreeSpaceYCoord + freeSpaceHeight, 5, winHeight - startFreeSpaceYCoord - freeSpaceHeight);

                dc.DrawRectangle(brownBrush, blkPen, lowerRect);

                // Check the rectangle for intersect
                if (CheckForRectIntersect(upperRect, CharacterRect) || CheckForRectIntersect(lowerRect, CharacterRect))
                {
                    dc.DrawRectangle(Brushes.Red, blkPen, CharacterRect);
                    gameOver = true; 
                }
            }

            // change the free space to an additional + 30 or -30
            int freeSpaceChange = rnd.Next(-30, 30);

            dc.DrawRectangle(brownBrush, blkPen, new Rect(winWidth - 5, 0, 5, startFreeSpaceYCoord));

            startFreeSpaceYCoord = startFreeSpaceYCoord + freeSpaceChange;

            while (startFreeSpaceYCoord < 0 || startFreeSpaceYCoord > winHeight - freeSpaceHeight)
            {
                freeSpaceChange = rnd.Next(-30, 30);
                startFreeSpaceYCoord = startFreeSpaceYCoord + freeSpaceChange;
            }

            caveRunner[((int)winWidth - 5) / 5].startFreeSpacePosition = startFreeSpaceYCoord;
            caveRunner[((int)winWidth - 5) / 5].endFreeSpacePosition = startFreeSpaceYCoord + freeSpaceHeight;

            dc.DrawRectangle(brownBrush, blkPen, new Rect(winWidth - 5, startFreeSpaceYCoord + freeSpaceHeight, 5, winHeight - startFreeSpaceYCoord - freeSpaceHeight));

            // Drawing the CaverRunner Character
            
            dc.DrawRectangle(Brushes.Red, blkPen, CharacterRect);

            dc.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)winWidth, (int)winHeight, 96, 96, PixelFormats.Pbgra32);
            //Render the visual to the bitmap
            bmp.Render(vis);
            //Make the image source the bitmap
            imgPlot.Source = bmp;
            lblScore.Content = ("Score: " + (playerScore++).ToString());
            if(playerScore % 5 == 0)
            {
                changeGameSpeed();
            }
        }
        
        private void CallGameOver()
        {
            tmr1.Stop();

            if (MessageBox.Show("Game Over \n Total Score: " + playerScore, "Do you want to Play again", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
            {
                System.Environment.Exit(0);
            } else
            {
                characterYCoord = caveRunner[0].startFreeSpacePosition + 75;
                gameOver = false;
                DrawFramesToScreen();
                tmr1.Start();
            }
        }
        private bool CheckForRectIntersect(Rect R1, Rect R2)
        {

            Rect resultRect = Rect.Intersect(R1, R2);
            if (resultRect.IsEmpty)
                return false;
            else
                return true;
                           
        }
        private void DrawInitialScreen()
        {
            Pen blkPen = new Pen(Brushes.Black, 1);
            Brush blueBrush = Brushes.AliceBlue;
            Brush brownBrush = Brushes.Brown;
            // Creating a drawing visual to draw on
            DrawingVisual vis = new DrawingVisual();
            // Create a drawing context for this visual
            DrawingContext dc = vis.RenderOpen();
            Random rnd = new Random();

            caveRunner = new caveRectangle[(int)winWidth / 5];

            startFreeSpaceYCoord = (int)winHeight / 3;
            int freeSpaceChange = rnd.Next(-30, 30);

            for (int a = 0; a < winWidth; a = a + 5)
            {
                caveRunner[a / 5] = new caveRectangle();

                dc.DrawRectangle(brownBrush, blkPen, new Rect(a, 0, 5, startFreeSpaceYCoord));

                freeSpaceChange = rnd.Next(-30, 30);
                startFreeSpaceYCoord = startFreeSpaceYCoord + freeSpaceChange;

                while (startFreeSpaceYCoord < 0 || startFreeSpaceYCoord > winHeight - freeSpaceHeight)
                {
                    freeSpaceChange = rnd.Next(-30, 30);
                    startFreeSpaceYCoord = startFreeSpaceYCoord + freeSpaceChange;
                }

                caveRunner[a / 5].startFreeSpacePosition = startFreeSpaceYCoord;
                caveRunner[a / 5].endFreeSpacePosition = startFreeSpaceYCoord + freeSpaceHeight;

                dc.DrawRectangle(brownBrush, blkPen, new Rect(a, startFreeSpaceYCoord + freeSpaceHeight, 5, winHeight - startFreeSpaceYCoord - freeSpaceHeight));
            }

            dc.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)winWidth, (int)winHeight, 96, 96, PixelFormats.Pbgra32);
            //Render the visual to the bitmap
            bmp.Render(vis);
            //Make the image source the bitmap
            imgPlot.Source = bmp;
        }

        private void Tmr_Tick(object sender, EventArgs e)
        {
            DrawFramesToScreen();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Down)
            {
                characterYCoord = characterYCoord + 5;
                DrawFramesToScreen();
            } else if(e.Key == Key.Up)
            {
                characterYCoord = characterYCoord - 5;
                DrawFramesToScreen();
            } else if(e.Key == Key.Right)
            {
                DrawFramesToScreen();
            }
        }
    }
}
