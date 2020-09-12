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
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Othello;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board gameBoard = new Board(8, 8);

        public MainWindow()
        {
            InitializeComponent();

            //gameBoard.squares[2, 2].setPiece(new Piece(Colors.White));
            
            displayBoard();
            bw1.DoWork += new DoWorkEventHandler(bw1_I_DONT_WANT_TO_WORK);
        }
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;
        //MemoryStream ms; 
        string currData;
        string dataToSend;
        int playerNumber;
        char playerTurn;

        delegate void SetTextCallback(String text);
        BackgroundWorker bw1 = new BackgroundWorker();

        private void bw1_I_DONT_WANT_TO_WORK(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
               try
                {
                    string inputStream = sr.ReadLine();       // Read onyl reads into byte array

                    if (inputStream.Contains("Wins"))
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(inputStream);
                            inputStream = "disconnect";
                        });
                    }

                    if (inputStream.Contains("Welcome"))
                    {
                        //lblPlayer.Content = "Player " + inputStream.Split(' ')[1];
                        try
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                playerNumber = Convert.ToInt32(inputStream.Split(' ')[1]);
                                if (playerNumber == 0)
                                    lblPlayerPiece.Content = "Piece Color: Black";
                                else if (playerNumber == 1)
                                    lblPlayerPiece.Content = "Piece Color: White";
                                else
                                    lblPlayerPiece.Content = "Piece Color: NONE";

                                lblPlayer.Content = "Player " + (playerNumber+1).ToString();
                            });

                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Cannot set label " + ex.ToString());
                        }
                    }


                    //lblPlayer.Content = inputStream;
                    
                    currData = inputStream;
                    putDataToBoard(currData);
                        /* if(inputStream != "")
                             putDataToBoard(inputStream);*/

                     WriteToScreen("Server said:    " + inputStream);
                    
                    if (inputStream == "disconnect")
                    {
                        sw.WriteLine("disconnect");
                        sw.Flush();
                        sr.Close();
                        sw.Close();
                        if (ns != null)
                            ns.Close();

                        System.Environment.Exit(System.Environment.ExitCode); //close all 

                        break;
                    }
                }
                catch
                {
                    ns.Close();
                    System.Environment.Exit(System.Environment.ExitCode); //close all 
                }
            }
        }
        private void putDataToBoard(String s)
        {
            if (s == "" || s == null || s.Contains("Welcome"))
            {

            }
                //MessageBox.Show("Empty Data");
            else
            {
                try
                {
                    string[] words = s.Split(' ');
                    int x = Convert.ToInt32(words[0]);
                    int y = Convert.ToInt32(words[1]);
                    char c = Convert.ToChar(words[2]);
                    playerTurn = Convert.ToChar(words[3]);



                    this.Dispatcher.Invoke(() =>
                    {
                        if (playerTurn == 'B')
                            lblPlayerTurn.Content = "Player 1's Turn - Piece Turn: Black";
                        else
                            lblPlayerTurn.Content = "Player 2's Turn - Piece Turn: White";

                        if (c == 'B')
                            gameBoard.squares[x, y].setPiece(new Piece(Colors.Black));
                        else
                            gameBoard.squares[x, y].setPiece(new Piece(Colors.White));
                    
                        displayBoard();
                    });
                    //MessageBox.Show(playerTurn.ToString());
                } catch(Exception e)
                {
                    MessageBox.Show("Received " + s + " Some Error occured" + e.ToString());
                }
            }
        }

        private void WriteToScreen(string message)
        {
            if (this.txtScreen.Dispatcher.CheckAccess())
            {
                if (txtScreen.Text.Length != 0)
                    txtScreen.Text += "\n";
                txtScreen.Text += message;
            }
            else
            {
                txtScreen.Dispatcher.BeginInvoke(new SetTextCallback(WriteToScreen), message);
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            TcpClient newConnection = new TcpClient();
            try
            {
                newConnection.Connect("127.0.0.1", 9801);

                ns = newConnection.GetStream();
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);

                bw1.RunWorkerAsync("Message to Worker");
            }
            catch
            {
                WriteToScreen("Cannot connect to the Specified address; Connection cannot be made Sorry");
            }
        }
        private void displayBoard()
        {
            imgBoard.Source = gameBoard.BoardImage();
        }
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            //sw.WriteLine(txtSend.Text);
            sw.WriteLine(dataToSend);

            WriteToScreen("You Said:    " + dataToSend);

            sw.Flush();
            if (txtSend.Text == "disconnect")
            {
                sw.Close();
                sr.Close();
                ns.Close();
                System.Environment.Exit(System.Environment.ExitCode);
            }
            txtSend.Text = "";
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            if (currData == "" || currData == null || currData == "Welcome")
                MessageBox.Show("Empty Data"); 
            else
                putDataToBoard(currData);
        }

        private void imgBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((playerTurn == 'B' && playerNumber == 0) || (playerTurn == 'W' && playerNumber == 1))
            {
                System.Windows.Point ClickedPosition;
                ClickedPosition = e.GetPosition(imgBoard);

                int x = (int)ClickedPosition.X / 50;
                int y = (int)ClickedPosition.Y / 50;

                dataToSend = x.ToString() + " " + y.ToString();

                sw.WriteLine(dataToSend);

                WriteToScreen("You Said:    " + dataToSend);

                sw.Flush();
            }
        }
    }
}
