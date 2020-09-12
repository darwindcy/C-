using Othello;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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


namespace server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Serializable]
        struct Data
        {
            public int xpos;
            public int ypos;
            public char C;
            public char playerTrack;

            public Data(int x, int y, char c, char p)
            {
                xpos = x;
                ypos = y;
                C = c;
                playerTrack = p;
            }
            public String DataToStr()
            {
                return xpos.ToString() + " " + ypos.ToString() + " " + C.ToString() + " " + playerTrack.ToString();
            }
        }

        delegate void SetTextCallback(String text);
        delegate void SetIntCallbCk(int theadnum);

        TcpListener listener = new TcpListener(IPAddress.Any, 9801);

        Board gameBoard = new Board(8, 8);
        Player player1 = new Player(Colors.Black);
        Player player2 = new Player(Colors.White);
        Player currPlayer;
        Char playerAlphabet; 
        HashSet<Move> Moves = new HashSet<Move>();
        bool validMove = false;
        int p1Count = 0;
        int p2Count = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeOthello();
            updateScreen();
            //MessageBox.Show(gameBoard.squares[4, 4].getPiece().colorChar().ToString());
        }

        Socket currentClient;

        NetworkStream[] ns = new NetworkStream[100];
        StreamReader[] sr = new StreamReader[100];
        StreamWriter[] sw = new StreamWriter[100];
        List<int> AvailableClients = new List<int>(100);
        List<int> UsedClientNumbers = new List<int>(100);
        int clientcount = 0;
        BackgroundWorker bwMain = new BackgroundWorker();
        BackgroundWorker[] bw1 = new BackgroundWorker[100];

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

        private void bwMain_IDontWantToWorkk(object sender, DoWorkEventArgs e)
        {
            string toPrint;
            TcpListener newSocket = new TcpListener(IPAddress.Any, 9801); // Create a TCP listencer

            newSocket.Start();

            for (int i = 0; i < 100; i++)
            {
                AvailableClients.Add(i);
            }

            while (AvailableClients.Count > 0)
            {
                if (UsedClientNumbers.Count <= 5)
                {
                    WriteToScreen("Waiting For Client");  // waiting for connection
                    toPrint = "Available Clients = " + AvailableClients.Count;
                    WriteToScreen(toPrint);             // waiting for connection

                    currentClient = newSocket.AcceptSocket();          //Accept connection

                    clientcount = AvailableClients.First();
                    AvailableClients.Remove(clientcount);
                    ns[clientcount] = new NetworkStream(currentClient);
                    sr[clientcount] = new StreamReader(ns[clientcount]);
                    sw[clientcount] = new StreamWriter(ns[clientcount]);

                    string welcome = "Welcome " + clientcount.ToString();
                    WriteToScreen("Client Connected");

                    addActiveConnections("Client Number " + clientcount.ToString());

                    sw[clientcount].WriteLine(welcome);
                    sw[clientcount].Flush();

                    bw1[clientcount] = new BackgroundWorker();
                    bw1[clientcount].DoWork += new DoWorkEventHandler(client_DoesntWantToWork);

                    bw1[clientcount].RunWorkerAsync(clientcount);
                    UsedClientNumbers.Add(clientcount);

                    
                }
                else
                {
                    WriteToScreen("Maximum number of connections Reached");
                }
            }
        }

        private void client_DoesntWantToWork(object sender, DoWorkEventArgs e)
        {

            int clientNum = (int)e.Argument;
            //MessageBox.Show("Client Connected to and Doing work " + clientNum);
            bw1[clientNum].WorkerSupportsCancellation = true;

            while (true)
            {
                string inputStream;
                try
                {
                    inputStream = sr[clientNum].ReadLine();
                    WriteToScreen(inputStream);

                    //MessageBox.Show((inputStream[0] - '0') + " " + (inputStream[2] - '0'));
                    
                    playerMove(new Move(currPlayer, gameBoard.squares[(inputStream[0] - '0'), (inputStream[2] - '0')]));

                    if (inputStream == "disconnect")
                    {
                        sr[clientNum].Close();
                        sw[clientNum].Close();
                        ns[clientNum].Close();
                        WriteToScreen("Client " + clientNum + " has disconnected");
                        KillMe(clientNum);
                        break;
                    }
                }
                catch
                {
                    sr[clientNum].Close();
                    sw[clientNum].Close();
                    ns[clientNum].Close();
                    WriteToScreen("Client " + clientNum + " has disconnected");
                    KillMe(clientNum);
                }
            }
        }

        private void addActiveConnections(string clientName)
        {
            if (this.txtScreen.Dispatcher.CheckAccess())
            {
                activeConnections.Items.Insert(0, clientName);
            }
            else
            {
                txtScreen.Dispatcher.BeginInvoke(new SetTextCallback(addActiveConnections), clientName);
            }

            if(activeConnections.Items.Count == 2)
                btnSend_Click(null, null);

        }
        private void KillMe(int threadnum)
        {
            UsedClientNumbers.Remove(threadnum);
            AvailableClients.Add(threadnum);
            if (bw1[threadnum] != null && bw1[threadnum].IsBusy)
            {
                bw1[threadnum].CancelAsync();
                bw1[threadnum].Dispose();
            }
            bw1[threadnum] = null;
            GC.Collect();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            Data data = new Data(1, 2, 'B', 'W');
             
            foreach (int t in UsedClientNumbers)
            {
                //sw[t].WriteLine(txtData.Text);
                foreach (Square sq in gameBoard.squares) {
                    if (sq.isOccupied())
                    {
                        data = new Data(sq.getX(), sq.getY(), sq.getPiece().colorChar(), currPlayer.getAlphabet());
                        sw[t].WriteLine(data.DataToStr());
                        WriteToScreen("You Said:    " + data.DataToStr());
                    }
                }

                sw[t].Flush();
            }

            //txtData.Text = "";
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            bwMain.DoWork += new DoWorkEventHandler(bwMain_IDontWantToWorkk);
            if (!bwMain.IsBusy)
                bwMain.RunWorkerAsync("Hello Worker");
            else
                WriteToScreen("The Dude's already Started, How much do you want it to Work !!!");
        }

        public void InitializeOthello()
        {
            gameBoard.setPiece(4, 5, new Piece(player1.getPlayerColor()));
            gameBoard.setPiece(5, 4, new Piece(player1.getPlayerColor()));
            gameBoard.setPiece(4, 4, new Piece(player2.getPlayerColor()));
            gameBoard.setPiece(5, 5, new Piece(player2.getPlayerColor()));
            currPlayer = player1;
        }

        public void PrintBoard()
        {
            this.Dispatcher.Invoke(() =>
            {
                imgBoard.Source = gameBoard.BoardImage();
            });
            
        }

        private void updateScreen()
        {
            if (currPlayer.getPlayerColor() == Colors.White)
                lblPlayer.Content = "Player Turn: White";
            else if (currPlayer.getPlayerColor() == Colors.Black)
                lblPlayer.Content = "Player Turn: Black";
            Moves = gameBoard.getMoves(currPlayer);

            PrintBoard();

            btnSend_Click(null, null);
        }
        private void playerMove(Move move)
        {
            if (Moves.Count == 0)
            {
                MessageBox.Show("No Valid moves for current player; Turn Forfeited");

                if (!CheckForWin())
                    ChangeTurn();
                else
                    WinnerFound();

                updateScreen();
            }
            else
            {
                foreach (Move mv in Moves)
                {
                    if (mv.GetMovingSquare().getX() == move.GetMovingSquare().getX() &&
                        mv.GetMovingSquare().getY() == move.GetMovingSquare().getY())
                    {
                        validMove = true;
                        this.Dispatcher.Invoke(() =>
                        {
                        move.GetMovingSquare().setPiece(new Piece(move.GetMovingPlayer().getPlayerColor()));
                        ExecuteMove(mv);

                        if (!CheckForWin())
                            ChangeTurn();
                        else
                            WinnerFound();

                        updateScreen();
                        });

                        break;
                    }
                }
                if (!validMove)
                    MessageBox.Show("Invalid Move");

                validMove = false;
            }
        }

        private void ExecuteMove(Move aMove)
        {
            int startX = aMove.GetMovingSquare().getX();
            int startY = aMove.GetMovingSquare().getY();

            ChangeLeft(startX, startY);
            ChangeRight(startX, startY);
            ChangeUp(startX, startY);
            ChangeDown(startX, startY);
            ChangeUpLeft(startX, startY);
            ChangeUpRight(startX, startY);
            ChangeDownLeft(startX, startY);
            ChangeDownRight(startX, startY);

        }
        private void ChangeUpRight(int x, int y)
        {
            int i = x + 1; int j = y - 1;

            while (j >= 1 && i <= 7)
            {
                Square sq = gameBoard.squares[i, j];
                if (sq.isOccupied() && Math.Abs(sq.getY() - y) == 1 && Math.Abs(sq.getX() - x) == 1)
                {
                    if (sq.getPiece().getColor() == currPlayer.getPlayerColor())
                        break;
                    else
                        i++; j--; continue;
                }
                else
                {
                    if (sq.isOccupied() && sq.getPiece().getColor() == currPlayer.getPlayerColor())
                    {
                        changeBetween(i, j, x, y);
                        break;
                    }
                    else
                        i++; j--; continue;
                }
            }
        }
        private void ChangeDownLeft(int x, int y)
        {
            int i = x - 1; int j = y + 1;

            while (j <= 7 && i >= 1)
            {
                Square sq = gameBoard.squares[i, j];
                if (sq.isOccupied() && Math.Abs(sq.getY() - y) == 1 && Math.Abs(sq.getX() - x) == 1)
                {
                    if (sq.getPiece().getColor() == currPlayer.getPlayerColor())
                        break;
                    else
                        i--; j++; continue;
                }
                else
                {
                    if (sq.isOccupied() && sq.getPiece().getColor() == currPlayer.getPlayerColor())
                    {
                        changeBetween(i, j, x, y);
                        break;
                    }
                    else
                        i--; j++; continue;
                }
            }
        }
        private void ChangeUpLeft(int x, int y)
        {
            int i = x - 1; int j = y - 1;

            while (j >= 1 && i >= 1)
            {
                Square sq = gameBoard.squares[i, j];
                if (sq.isOccupied() && Math.Abs(sq.getY() - y) == 1 && Math.Abs(sq.getX() - x) == 1)
                {
                    if (sq.getPiece().getColor() == currPlayer.getPlayerColor())
                        break;
                    else
                        i--; j--; continue;
                }
                else
                {
                    if (sq.isOccupied() && sq.getPiece().getColor() == currPlayer.getPlayerColor())
                    {
                        changeBetween(i, j, x, y);
                        break;
                    }
                    else
                        i--; j--; continue;
                }
            }
        }

        private void ChangeDownRight(int x, int y)
        {
            int i = x + 1; int j = y + 1;

            while (j <= 7 && i <= 7)
            {
                Square sq = gameBoard.squares[i, j];
                if (Math.Abs(sq.getY() - y) == 1 && Math.Abs(sq.getX() - x) == 1)
                {
                    if (sq.isOccupied() && sq.getPiece().getColor() == currPlayer.getPlayerColor())
                        break;
                    else
                        i++; j++; continue;
                }
                else
                {
                    if (sq.isOccupied() && sq.getPiece().getColor() == currPlayer.getPlayerColor())
                    {
                        changeBetween(i, j, x, y);
                        break;
                    }
                    else
                        i++; j++; continue;
                }
            }
        }
        private void ChangeUp(int x, int y)
        {
            int i = x; int j = y - 1;

            while (j >= 1)
            {
                Square sq = gameBoard.squares[i, j];
                if (sq.isOccupied() && Math.Abs(sq.getY() - y) == 1)
                {
                    if (sq.getPiece().getColor() == currPlayer.getPlayerColor())
                        break;
                    else
                        j--; continue;
                }
                else
                {
                    if (sq.isOccupied() && sq.getPiece().getColor() == currPlayer.getPlayerColor())
                    {
                        changeBetween(i, j, x, y);
                        break;
                    }
                    else
                        j--; continue;
                }
            }
        }
        private void ChangeDown(int x, int y)
        {
            int i = x; int j = y + 1;

            while (j <= 7)
            {
                Square sq = gameBoard.squares[i, j];
                if (sq.isOccupied() && Math.Abs(sq.getY() - y) == 1)
                {
                    if (sq.getPiece().getColor() == currPlayer.getPlayerColor())
                        break;
                    else
                        j++; continue;
                }
                else
                {
                    if (sq.isOccupied() && sq.getPiece().getColor() == currPlayer.getPlayerColor())
                    {
                        changeBetween(i, j, x, y);
                        break;
                    }
                    else
                        j++; continue;
                }
            }
        }
        private void ChangeRight(int x, int y)
        {
            int i = x + 1; int j = y;

            while (i <= 7)
            {
                Square sq = gameBoard.squares[i, j];
                if (sq.isOccupied() && Math.Abs(sq.getX() - x) == 1)
                {
                    if (sq.getPiece().getColor() == currPlayer.getPlayerColor())
                        break;
                    else
                        i++; continue;
                }
                else
                {
                    if (sq.isOccupied() && sq.getPiece().getColor() == currPlayer.getPlayerColor())
                    {
                        changeBetween(i, j, x, y);
                        break;
                    }
                    else
                        i++; continue;
                }
            }
        }
        private void ChangeLeft(int x, int y)
        {
            int i = x - 1; int j = y;

            while (i >= 1)
            {
                Square sq = gameBoard.squares[i, j];
                if (sq.isOccupied() && Math.Abs(sq.getX() - x) == 1)
                {
                    if (sq.getPiece().getColor() == currPlayer.getPlayerColor())
                        break;
                    else
                        i--; continue;
                }
                else
                {
                    if (sq.isOccupied() && sq.getPiece().getColor() == currPlayer.getPlayerColor())
                    {

                        changeBetween(i, j, x, y);
                        break;
                    }
                    else
                        i--; continue;
                }
            }
        }
        private void changeBetween(int a, int b, int c, int d)
        {
            int startX, startY, endX, endY;

            if (a < c)
            {
                startX = a; endX = c;
            }
            else
            {
                startX = c; endX = a;
            }
            if (b < d)
            {
                startY = b; endY = d;
            }
            else
            {
                startY = d; endY = b;
            }
            //MessageBox.Show("Changing between " + startX + "," + startY + " and " + endX + "," + endY);
            while (startX != endX || startY != endY)
            {
                gameBoard.squares[startX, startY].setPiece(new Piece(currPlayer.getPlayerColor()));
                //MessageBox.Show("Squares Changing " + startX + " " + startY);

                if (startX != endX)
                    startX++;
                if (startY != endY)
                    startY++;
            }
        }

        private void WinnerFound()
        {
            string winnerMessage;
            if (p1Count > p2Count)
            {
                winnerMessage = "Player 1 Wins";
            }
            else
            {
                winnerMessage = "Player 2 Wins";
            }

            MessageBox.Show(winnerMessage);

            foreach (int t in UsedClientNumbers)
            {
                //sw[t].WriteLine(txtData.Text);
                foreach (Square sq in gameBoard.squares)
                {
                    if (sq.isOccupied())
                    {
                        sw[t].WriteLine(winnerMessage);
                        WriteToScreen("You Said:    " + winnerMessage);
                    }
                }

                sw[t].Flush();
            }
        }

        private void ChangeTurn()
        {
            if (currPlayer == player1)
                currPlayer = player2;
            else
                currPlayer = player1;
        }
        private bool CheckForWin()
        {

            for (int i = 0; i < gameBoard.getNumRows(); i++)
            {
                for (int j = 0; j < gameBoard.getNumCols(); j++)
                {
                    if (!gameBoard.squares[i, j].isOccupied())
                    {
                        return false;
                    }
                    if (gameBoard.squares[i, j].getPiece() != null && gameBoard.squares[i, j].getPiece().getColor() == player1.getPlayerColor())
                    {
                        p1Count += 1;
                    }
                    else if (gameBoard.squares[i, j].getPiece() != null && gameBoard.squares[i, j].getPiece().getColor() == player2.getPlayerColor())
                    {
                        p2Count += 1;
                    }
                }
            }

            return true;
        }

        private void imgBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point ClickedPosition;
            ClickedPosition = e.GetPosition(imgBoard);

            int x = (int)ClickedPosition.X / 50;
            int y = (int)ClickedPosition.Y / 50;

            //MessageBox.Show(x + " " + y);
            playerMove(new Move(currPlayer, gameBoard.squares[x, y]));

        }
    }
}
