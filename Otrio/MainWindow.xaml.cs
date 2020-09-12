// submitted By: Darwin Charles Yadav
// Project 3: Otrio
// CS 376- Small Computer Software
// Submitted To: Dr. Blandford and Mr. Randall

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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Otrio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Square[,] gameBoard = new Square[3, 3];

        DrawingVisual vis = new DrawingVisual();

        Ring ringClicked;
        Square chosenSquare;
        char currentPlayer = 'B';
        String gameMode;
        bool moveOccured = false;

        char previousPlayer = 'R';
        Ring computerMoveRing;
        Square computerMoveSquare;

        public MainWindow()
        {
            InitializeComponent();
            //printBoard();
            btnBlueSmall.Visibility = Visibility.Hidden;
            btnBlueMedium.Visibility = Visibility.Hidden;
            btnBlueLarge.Visibility = Visibility.Hidden;

            btnRedSmall.Visibility = Visibility.Hidden;
            btnRedMedium.Visibility = Visibility.Hidden;
            btnRedLarge.Visibility = Visibility.Hidden;

            btnGreenLarge.Visibility = Visibility.Hidden;
            btnGreenMedium.Visibility = Visibility.Hidden;
            btnGreenSmall.Visibility = Visibility.Hidden;

            btnYellowLarge.Visibility = Visibility.Hidden;
            btnYellowMedium.Visibility = Visibility.Hidden;
            btnYellowSmall.Visibility = Visibility.Hidden;

            lblMain.Content = "                Otrio \n Select the Game Mode";
        }
        private void startGame()
        {
            printBoard();
            setImagesofButtons();
        }

        private void move(Square currSquare, Ring currRing)
        {
            //MessageBox.Show("Entereed Move; ");
            if (currRing != null && currSquare != null)
            {
                if (currRing.getSize() == 'S')
                {
                    if (currSquare.GetSmallRing() == null)
                    {
                        currSquare.SetSmallRing(currRing.getColor());
                        changePlayer();
                        moveOccured = true;
                    }
                    else
                        MessageBox.Show("That position is already filled for that size of Ring");
                }
                else if (currRing.getSize() == 'M')
                {
                    if (currSquare.GetMediumRing() == null)
                    {
                        currSquare.SetMediumRing(currRing.getColor());
                        changePlayer();
                        moveOccured = true; 
                        //MessageBox.Show("Properly executed Med ");
                    }
                    else
                        MessageBox.Show("That position is already filled for that size of Ring");
                }
                else if (currRing.getSize() == 'L')
                {
                    if (currSquare.GetLargeRing() == null)
                    {
                        currSquare.SetLargeRing(currRing.getColor());
                        changePlayer();
                        moveOccured = true;
                        //MessageBox.Show("Properly executed large ");
                    }
                    else
                        MessageBox.Show("That position is already filled for that size of Ring");
                }
            }
            else
            {
                MessageBox.Show("Select a Square :P ");
            }
        }

        private void printBoard()
        {
            initializeBoard();
            printGameBoard();
        }

        private void initializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gameBoard[i, j] = new Square(i, j);
                }
            }

            DrawingVisual aVis = new DrawingVisual();
            DrawingContext aDc = aVis.RenderOpen();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    aDc.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Green, 1), new Rect(i * 100, j * 100, 100, 100));
                }
            }
            aDc.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(300, 300, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(aVis);
            imgBackBoard.Source = bmp;
            imgBoard.Source = null;
        }

        private void printGameBoard()
        {
            setBackgroundSquare();

            DrawingContext dc = vis.RenderOpen();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gameBoard[i, j].printSquare(dc);
                }
            }
            dc.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(300, 300, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(vis);

            imgBoard.Source = bmp;

            DoubleAnimation myDoubleAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));

            imgBoard.BeginAnimation(OpacityProperty, myDoubleAnimation);
        }

        private void setImagesofButtons()
        {
            DrawingVisual vis1 = new DrawingVisual();
            DrawingVisual vis2 = new DrawingVisual();
            DrawingVisual vis3 = new DrawingVisual();
            DrawingVisual vis4 = new DrawingVisual();
            DrawingVisual vis5 = new DrawingVisual();
            DrawingVisual vis6 = new DrawingVisual();

            DrawingContext dcSmallBlue = vis1.RenderOpen();
            DrawingContext dcMediumBlue = vis2.RenderOpen();
            DrawingContext dcLargeBlue = vis3.RenderOpen();
            DrawingContext dcSmallRed = vis4.RenderOpen();
            DrawingContext dcMediumRed = vis5.RenderOpen();
            DrawingContext dcLargeRed = vis6.RenderOpen();



            dcSmallBlue.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Blue, 2), new Point(50, 50), 15, 15);
            dcMediumBlue.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Blue, 2), new Point(50, 50), 30, 30);
            dcLargeBlue.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Blue, 2), new Point(50, 50), 45, 45);
            dcSmallRed.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Red, 2), new Point(50, 50), 15, 15);
            dcMediumRed.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Red, 2), new Point(50, 50), 30, 30);
            dcLargeRed.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Red, 2), new Point(50, 50), 45, 45);

            dcSmallBlue.Close();
            dcMediumBlue.Close();
            dcLargeBlue.Close();
            dcSmallRed.Close();
            dcMediumRed.Close();
            dcLargeRed.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            RenderTargetBitmap bmp2 = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            RenderTargetBitmap bmp3 = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            RenderTargetBitmap bmp4 = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            RenderTargetBitmap bmp5 = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            RenderTargetBitmap bmp6 = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);

            ////////////////////////////////////////
            bmp.Render(vis1);
            btnBlueSmall.Background = new ImageBrush(bmp);
            Image x = new Image
            {
                Source = bmp
            };
            btnBlueSmall.Content = x;
            /////////////////////////////////////////

            bmp2.Render(vis2);
            btnBlueMedium.Background = new ImageBrush(bmp2);
            Image x1 = new Image
            {
                Source = bmp2
            };
            btnBlueMedium.Content = x1;

            bmp3.Render(vis3);
            btnBlueLarge.Background = new ImageBrush(bmp3);
            Image x2 = new Image
            {
                Source = bmp3
            };
            btnBlueLarge.Content = x2;

            bmp4.Render(vis4);
            btnRedSmall.Background = new ImageBrush(bmp4);
            Image x3 = new Image
            {
                Source = bmp4
            };
            btnRedSmall.Content = x3;

            bmp5.Render(vis5);
            btnRedMedium.Background = new ImageBrush(bmp5);
            Image x4 = new Image
            {
                Source = bmp5
            };
            btnRedMedium.Content = x4;

            bmp6.Render(vis6);
            btnRedLarge.Background = new ImageBrush(bmp6);
            Image x5 = new Image
            {
                Source = bmp6
            };
            btnRedLarge.Content = x5;

            btnBlueSmall.Visibility = Visibility.Visible;
            btnBlueMedium.Visibility = Visibility.Visible;
            btnBlueLarge.Visibility = Visibility.Visible;

            btnRedSmall.Visibility = Visibility.Visible;
            btnRedMedium.Visibility = Visibility.Visible;
            btnRedLarge.Visibility = Visibility.Visible;
        }
        private void setYellowGreen()
        {
            DrawingVisual vis1 = new DrawingVisual();
            DrawingVisual vis2 = new DrawingVisual();
            DrawingVisual vis3 = new DrawingVisual();
            DrawingVisual vis4 = new DrawingVisual();
            DrawingVisual vis5 = new DrawingVisual();
            DrawingVisual vis6 = new DrawingVisual();


            DrawingContext dcSmallBlue = vis1.RenderOpen();
            DrawingContext dcMediumBlue = vis2.RenderOpen();
            DrawingContext dcLargeBlue = vis3.RenderOpen();
            DrawingContext dcSmallRed = vis4.RenderOpen();
            DrawingContext dcMediumRed = vis5.RenderOpen();
            DrawingContext dcLargeRed = vis6.RenderOpen();



            dcSmallBlue.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Green, 2), new Point(50, 50), 15, 15);
            dcMediumBlue.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Green, 2), new Point(50, 50), 30, 30);
            dcLargeBlue.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Green, 2), new Point(50, 50), 45, 45);
            dcSmallRed.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Yellow, 2), new Point(50, 50), 15, 15);
            dcMediumRed.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Yellow, 2), new Point(50, 50), 30, 30);
            dcLargeRed.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Yellow, 2), new Point(50, 50), 45, 45);

            dcSmallBlue.Close();
            dcMediumBlue.Close();
            dcLargeBlue.Close();
            dcSmallRed.Close();
            dcMediumRed.Close();
            dcLargeRed.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            RenderTargetBitmap bmp2 = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            RenderTargetBitmap bmp3 = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            RenderTargetBitmap bmp4 = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            RenderTargetBitmap bmp5 = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            RenderTargetBitmap bmp6 = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);

            ////////////////////////////////////////
            bmp.Render(vis1);
            btnGreenSmall.Background = new ImageBrush(bmp);
            Image x = new Image
            {
                Source = bmp
            };
            btnGreenSmall.Content = x;
            /////////////////////////////////////////

            bmp2.Render(vis2);
            btnGreenMedium.Background = new ImageBrush(bmp2);
            Image x1 = new Image
            {
                Source = bmp2
            };
            btnGreenMedium.Content = x1;

            bmp3.Render(vis3);
            btnGreenLarge.Background = new ImageBrush(bmp3);
            Image x2 = new Image
            {
                Source = bmp3
            };
            btnGreenLarge.Content = x2;

            bmp4.Render(vis4);
            btnYellowSmall.Background = new ImageBrush(bmp4);
            Image x3 = new Image
            {
                Source = bmp4
            };
            btnYellowSmall.Content = x3;

            bmp5.Render(vis5);
            btnYellowMedium.Background = new ImageBrush(bmp5);
            Image x4 = new Image
            {
                Source = bmp5
            };
            btnYellowMedium.Content = x4;

            bmp6.Render(vis6);
            btnYellowLarge.Background = new ImageBrush(bmp6);
            Image x5 = new Image
            {
                Source = bmp6
            };
            btnYellowLarge.Content = x5;

            btnGreenSmall.Visibility = Visibility.Visible;
            btnGreenMedium.Visibility = Visibility.Visible;
            btnGreenLarge.Visibility = Visibility.Visible;

            btnYellowSmall.Visibility = Visibility.Visible;
            btnYellowMedium.Visibility = Visibility.Visible;
            btnYellowLarge.Visibility = Visibility.Visible;
        }
        private void imgBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point ClickedPosition;
            ClickedPosition = e.GetPosition(imgBoard);

            int xPos = (int)ClickedPosition.X;
            int yPos = (int)ClickedPosition.Y;

            if (xPos < 100)
            {
                if (yPos < 100)
                {
                    chosenSquare = gameBoard[0, 0];
                }
                else if (yPos < 200)
                {
                    chosenSquare = gameBoard[0, 1];
                }
                else if (yPos < 300)
                {
                    chosenSquare = gameBoard[0, 2];
                }
            }
            else if (xPos < 200)
            {
                if (yPos < 100)
                {
                    chosenSquare = gameBoard[1, 0];
                }
                else if (yPos < 200)
                {
                    chosenSquare = gameBoard[1, 1];
                }
                else if (yPos < 300)
                {
                    chosenSquare = gameBoard[1, 2];
                }
            }
            else if (xPos < 300)
            {
                if (yPos < 100)
                {
                    chosenSquare = gameBoard[2, 0];
                }
                else if (yPos < 200)
                {
                    chosenSquare = gameBoard[2, 1];
                }
                else if (yPos < 300)
                {
                    chosenSquare = gameBoard[2, 2];
                }
            }
            else
            {
                MessageBox.Show("Invalid location on Image");
            }

            move(chosenSquare, ringClicked);

            printGameBoard();

            if (moveOccured)
            {
                moveOccured = false;
                ringClicked = null;
                chosenSquare = null;

                if(gameMode != "quadplayer")
                    changePlayer();

                //MessageBox.Show("The Current Player is " + currentPlayer);

                if (checkForWin())
                {
                    if (gameMode == "quadplayer")
                    {
                        if (currentPlayer == 'R')
                            MessageBox.Show("Game Over, Blue Wins");
                        else if (currentPlayer == 'B')
                            MessageBox.Show("Game Over, Yellow Wins");
                        else if (currentPlayer == 'G')
                            MessageBox.Show("Game Over, Red Wins");
                        else if (currentPlayer == 'Y')
                            MessageBox.Show("Game Over, Green Wins");
                    }
                    else
                    {
                        if (currentPlayer == 'R')
                            MessageBox.Show("Game Over, Red Wins");
                        else
                            MessageBox.Show("Game Over, Blue Wins");
                    }
                    if (MessageBox.Show("Game Over\n", "Do you want to Play again ?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                    {
                        System.Environment.Exit(0);
                    }
                    else
                    {
                        initializeBoard();
                        printBoard();
                        changePlayer();
                    }

                }

                if(gameMode != "quadplayer")
                    changePlayer();

                if (gameMode == "singleplayer")
                    getMove();
            }
            
        }

        private void getMove()
        {       
            computerMove();

            chosenSquare = computerMoveSquare;
            ringClicked = computerMoveRing;

            //MessageBox.Show("Computer chose " + chosenSquare.x + ", " + chosenSquare.y + "-" + ringClicked.getSize() + " - " + ringClicked.getColor());

            move(gameBoard[chosenSquare.x, chosenSquare.y], ringClicked);
            //gameBoard[0, 0].SetLargeRing('R');
            
            printGameBoard();
            //MessageBox.Show("Printed");

            if (moveOccured)
            {
                moveOccured = false;
                ringClicked = null;
                chosenSquare = null;

                changePlayer();

                if (checkForWin())
                {
                    if (currentPlayer == 'R')
                        MessageBox.Show("Game Over, Red Wins");
                    else
                        MessageBox.Show("Game Over, Blue Wins");

                    if (MessageBox.Show("Game Over\n", "Do you want to Play again ?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                    {
                        System.Environment.Exit(0);
                    }
                    else
                    {
                        initializeBoard();
                        printBoard();
                        changePlayer();
                    }

                }
                changePlayer();
            }

        }

        private void computerMove()
        {
            bool forceEndLoop = false;

            // Checking if Computer has a winning move

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameBoard[i, j].GetLargeRing() == null)
                    {
                        gameBoard[i, j].SetLargeRing(currentPlayer);
                        if (checkForWin())
                        {
                            computerMoveRing = new Ring('L', currentPlayer);
                            computerMoveSquare = new Square(gameBoard[i, j]);

                            gameBoard[i, j].RemoveLargeRing();

                            forceEndLoop = true;
                            break;
                        }
                        gameBoard[i, j].RemoveLargeRing();
                    }
                    if (gameBoard[i, j].GetMediumRing() == null)
                    {
                        gameBoard[i, j].SetMediumRing(currentPlayer);
                        if (checkForWin())
                        {
                            computerMoveRing = new Ring('M', currentPlayer);
                            computerMoveSquare = new Square(gameBoard[i, j]);

                            gameBoard[i, j].RemoveMediumRing();

                            forceEndLoop = true;
                            break;
                        }
                        gameBoard[i, j].RemoveMediumRing();
                    }

                    if (gameBoard[i, j].GetSmallRing() == null)
                    {
                        gameBoard[i, j].SetSmallRing(currentPlayer);
                        if (checkForWin())
                        {
                            computerMoveRing = new Ring('S', currentPlayer);
                            computerMoveSquare = new Square(gameBoard[i, j]);

                            gameBoard[i, j].RemoveSmallRing();

                            forceEndLoop = true;
                            break;
                        }
                        gameBoard[i, j].RemoveSmallRing();
                    }
                }
                if (forceEndLoop)
                    break;
            }

            // Checking if the computer Opponent has a winning move

            if (!forceEndLoop)
            {
                changePlayer();
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        //Console.WriteLine("Checking the square for opponent large ring ", i, ", ", j);
                        if (gameBoard[i, j].GetLargeRing() == null)
                        {
                            gameBoard[i, j].SetLargeRing(currentPlayer);
                            if (checkForWin())
                            {
                                changePlayer();
                                computerMoveRing = new Ring('L', currentPlayer);
                                changePlayer();
                                computerMoveSquare = new Square(gameBoard[i, j]);

                                gameBoard[i, j].RemoveLargeRing();

                                forceEndLoop = true;
                              
                                break;
                            }
                            gameBoard[i, j].RemoveLargeRing();
                        }
                        if (gameBoard[i, j].GetMediumRing() == null)
                        {
                            gameBoard[i, j].SetMediumRing(currentPlayer);
                            if (checkForWin())
                            {
                                changePlayer();
                                computerMoveRing = new Ring('M', currentPlayer);
                                changePlayer();
                                computerMoveSquare = new Square(gameBoard[i, j]);

                                gameBoard[i, j].RemoveMediumRing();

                                forceEndLoop = true;
                                //changePlayer();
                                break;
                            }
                            gameBoard[i, j].RemoveMediumRing();
                        }

                        if (gameBoard[i, j].GetSmallRing() == null)
                        {
                            gameBoard[i, j].SetSmallRing(currentPlayer);
                            if (checkForWin())
                            {
                                changePlayer();
                                computerMoveRing = new Ring('S', currentPlayer);
                                changePlayer();
                                computerMoveSquare = new Square(gameBoard[i, j]);

                                gameBoard[i, j].RemoveSmallRing();

                                forceEndLoop = true;

                                break;
                            }
                            gameBoard[i, j].RemoveSmallRing();
                        }

                    }
                    changePlayer();

                    if (forceEndLoop)
                        break;
                }
            }

            Random rand = new Random();
            int size = rand.Next(3);
            int a = rand.Next(3);
            int b = rand.Next(3);

            if (!forceEndLoop)
            {
                while (gameBoard[a, b].GetLargeRing() != null || gameBoard[a, b].GetMediumRing() != null || gameBoard[a, b].GetSmallRing() != null)
                {
                    a = rand.Next(3);
                    b = rand.Next(3);
                    size = rand.Next(3);
                }

                computerMoveSquare = new Square(gameBoard[a, b]);

                if (gameBoard[a, b].GetLargeRing() == null && size == 0)
                    computerMoveRing = new Ring('L', currentPlayer);
                else if (gameBoard[a, b].GetMediumRing() == null && size == 1)
                    computerMoveRing = new Ring('M', currentPlayer);
                else if (gameBoard[a, b].GetSmallRing() == null && size == 2)
                    computerMoveRing = new Ring('S', currentPlayer);
            }
            //computerTurn = false;
        }

        private bool checkForWin()
        {
            return (checkStraights() || checkDiagonals() || checkEachSquare());
        }
        private bool checkDiagonals()
        {
            return Check3Squares(gameBoard[0, 0], gameBoard[1, 1], gameBoard[2, 2]) || Check3Squares(gameBoard[2, 0], gameBoard[1, 1], gameBoard[0, 2]);
        }

        private bool checkStraights()
        {
            return (Check3Squares(gameBoard[0, 0], gameBoard[0, 1], gameBoard[0, 2]) ||
                    Check3Squares(gameBoard[1, 0], gameBoard[1, 1], gameBoard[1, 2]) ||
                    Check3Squares(gameBoard[2, 0], gameBoard[2, 1], gameBoard[2, 2]) ||
                    Check3Squares(gameBoard[0, 0], gameBoard[1, 0], gameBoard[2, 0]) ||
                    Check3Squares(gameBoard[0, 1], gameBoard[1, 1], gameBoard[2, 1]) ||
                    Check3Squares(gameBoard[0, 2], gameBoard[1, 2], gameBoard[2, 2]));

        }
        private bool Check3Squares(Square sq1, Square sq2, Square sq3)
        {
            if (sq1.GetLargeRing() != null && sq2.GetMediumRing() != null && sq3.GetSmallRing() != null)
            {
                if (sq1.GetLargeRing().getColor() == sq2.GetMediumRing().getColor() && sq2.GetMediumRing().getColor() == sq3.GetSmallRing().getColor() && sq1.GetLargeRing().getColor() == currentPlayer)
                    return true;
            }

            if (sq1.GetSmallRing() != null && sq2.GetMediumRing() != null && sq3.GetLargeRing() != null)
            {
                if (sq1.GetSmallRing().getColor() == sq2.GetMediumRing().getColor() && sq2.GetMediumRing().getColor() == sq3.GetLargeRing().getColor() && sq1.GetSmallRing().getColor() == currentPlayer)
                    return true;
            }

            if (sq1.GetSmallRing() != null && sq2.GetSmallRing() != null && sq3.GetSmallRing() != null)
            {
                if (sq1.GetSmallRing().getColor() == sq2.GetSmallRing().getColor() && sq2.GetSmallRing().getColor() == sq3.GetSmallRing().getColor() && sq1.GetSmallRing().getColor() == currentPlayer)
                    return true;
            }

            if (sq1.GetMediumRing() != null && sq2.GetMediumRing() != null && sq3.GetMediumRing() != null)
            {
                if (sq1.GetMediumRing().getColor() == sq2.GetMediumRing().getColor() && sq2.GetMediumRing().getColor() == sq3.GetMediumRing().getColor() && sq1.GetMediumRing().getColor() == currentPlayer)
                    return true;
            }

            if (sq1.GetLargeRing() != null && sq2.GetLargeRing() != null && sq3.GetLargeRing() != null)
            {
                if (sq1.GetLargeRing().getColor() == sq2.GetLargeRing().getColor() && sq2.GetLargeRing().getColor() == sq3.GetLargeRing().getColor() && sq1.GetLargeRing().getColor() == currentPlayer)
                    return true;
            }
            return false;
        }
        private bool checkEachSquare()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if ((gameBoard[i, j].GetSmallRing() != null && gameBoard[i, j].GetMediumRing() != null && gameBoard[i, j].GetLargeRing() != null))
                    {
                        if ((gameBoard[i, j].GetSmallRing().getColor() == gameBoard[i, j].GetMediumRing().getColor()) &&
                            (gameBoard[i, j].GetMediumRing().getColor() == gameBoard[i, j].GetLargeRing().getColor()) &&
                            gameBoard[i, j].GetSmallRing().getColor() == currentPlayer)
                            return true;
                    }
                }
            }
            return false;
        }
        private void btnBlueSmall_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'B')
            {
                Ring newRing = new Ring('S', 'B');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }

        private void btnBlueMedium_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'B')
            {
                Ring newRing = new Ring('M', 'B');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }

        private void btnBlueLarge_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'B')
            {
                Ring newRing = new Ring('L', 'B');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }

        private void btnRedSmall_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'R')
            {
                Ring newRing = new Ring('S', 'R');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }

        private void btnRedMedium_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'R')
            {
                Ring newRing = new Ring('M', 'R');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }

        private void btnRedLarge_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'R')
            {
                Ring newRing = new Ring('L', 'R');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }
        private void changePlayer()
        {
            if (gameMode == "quadplayer")
            {
                //MessageBox.Show("Changing player in quad player");
                if (currentPlayer == 'B')
                    currentPlayer = 'R';
                else if (currentPlayer == 'R')
                    currentPlayer = 'G';
                else if (currentPlayer == 'G')
                    currentPlayer = 'Y';
                else if (currentPlayer == 'Y')
                    currentPlayer = 'B';
                //MessageBox.Show("The player is " + currentPlayer);
            }
            else
            {
                if (currentPlayer == 'B')
                    currentPlayer = 'R';
                else if (currentPlayer == 'R')
                    currentPlayer = 'B';
                else
                    MessageBox.Show("Some error occured while switching players");
            }

        }

        private void btnSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            btnSinglePlayer.Visibility = Visibility.Hidden;
            btnMultiPlayer.Visibility = Visibility.Hidden;
            lblMain.Visibility = Visibility.Hidden;
            btnQuadPlayer.Visibility = Visibility.Hidden;

            gameMode = "singleplayer";
            startGame();
        }

        private void btnMultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            btnSinglePlayer.Visibility = Visibility.Hidden;
            btnMultiPlayer.Visibility = Visibility.Hidden;
            lblMain.Visibility = Visibility.Hidden;
            btnQuadPlayer.Visibility = Visibility.Hidden;

            gameMode = "multiplayer";
            startGame();

        }

        private void btnRedLarge_DragOver(object sender, DragEventArgs e)
        {
            MessageBox.Show("Drag over started");

            if (!e.Data.GetDataPresent(typeof(Ring)))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void btnRedLarge_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Image)))
            {
                // do whatever you want do with the dropped element
                Image droppedThingie = e.Data.GetData(typeof(Image)) as Image;

                if (currentPlayer == 'R')
                {
                    Ring newRing = new Ring('M', 'R');
                    ringClicked = newRing;
                }
                else
                    MessageBox.Show("It is not your turn");

            }
        }

        private void setBackgroundSquare()
        {
            imgBackBoard.Source = imgBoard.Source;
        }
        private void highlightWinningSquares()
        {
            setBackgroundSquare();

            DrawingContext dc = vis.RenderOpen();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gameBoard[i, j].highlightSquare(dc);
                }
            }
            dc.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(300, 300, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(vis);

            imgBoard.Source = bmp;

            DoubleAnimation myDoubleAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));

            imgBoard.BeginAnimation(OpacityProperty, myDoubleAnimation);
        }

        private void btnQuadPlayer_Click(object sender, RoutedEventArgs e)
        {
            btnSinglePlayer.Visibility = Visibility.Hidden;
            btnMultiPlayer.Visibility = Visibility.Hidden;
            lblMain.Visibility = Visibility.Hidden;
            btnQuadPlayer.Visibility = Visibility.Hidden;

            gameMode = "quadplayer";
            setYellowGreen();
            startGame();
        }

        private void btnGreenLarge_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'G')
            {
                Ring newRing = new Ring('L', 'G');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }

        private void btnGreenMedium_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'G')
            {
                Ring newRing = new Ring('M', 'G');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }

        private void btnGreenSmall_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'G')
            {
                Ring newRing = new Ring('S', 'G');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }

        private void btnYellowLarge_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'Y')
            {
                Ring newRing = new Ring('L', 'Y');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }

        private void btnYellowMedium_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'Y')
            {
                Ring newRing = new Ring('M', 'Y');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }

        private void btnYellowSmall_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == 'Y')
            {
                Ring newRing = new Ring('S', 'Y');
                ringClicked = newRing;
            }
            else
                MessageBox.Show("It is not your turn");
        }
    }
}
