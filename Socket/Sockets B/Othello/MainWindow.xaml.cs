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

namespace Othello
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board gameBoard = new Board(8, 8);
        Player player1 = new Player(Colors.Black);
        Player player2 = new Player(Colors.White);
        Player currPlayer;
        HashSet<Move> Moves = new HashSet<Move>();
        bool validMove = false;
        int p1Count = 0;
        int p2Count = 0; 

        public MainWindow()
        {
            InitializeComponent();

            InitializeOthello();

            //gameBoard.squares[6, 4].setPiece(new Piece(Colors.White));
            //gameBoard.squares[6, 5].setPiece(new Piece(Colors.White));
            //gameBoard.squares[3, 6].setPiece(new Piece(Colors.Black));

            updateScreen();
/*
            foreach (Move mv in Moves)
            {
                Console.WriteLine(mv.toString());
            }*/
          
            //MessageBox.Show("Reached Here");
            //Console.WriteLine("Reached Here");

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
            imgBoard.Source = gameBoard.BoardImage();
        }

        private void imgBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point ClickedPosition;
            ClickedPosition = e.GetPosition(imgBoard);

            int x = (int)ClickedPosition.X / 50;
            int y = (int)ClickedPosition.Y / 50;

            //MessageBox.Show(x + " " + y);
            playerMove(new Move(currPlayer, gameBoard.squares[x, y]));

        }

        private void updateScreen()
        {
            
            if (currPlayer.getPlayerColor() == Colors.White)
                lblPlayer.Content = "Player Turn: White";
            else if(currPlayer.getPlayerColor() == Colors.Black)
                lblPlayer.Content = "Player Turn: Black";
            Moves = gameBoard.getMoves(currPlayer);

            PrintBoard();
        }

        private void playerMove(Move move)
        {
            if(Moves.Count == 0)
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

                        move.GetMovingSquare().setPiece(new Piece(move.GetMovingPlayer().getPlayerColor()));
                        ExecuteMove(mv);

                        if (!CheckForWin())
                            ChangeTurn();
                        else
                            WinnerFound();

                        updateScreen();

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
            int i = x; int j = y-1;

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
            int i = x-1; int j = y;

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
                    if (sq.isOccupied() && sq.getPiece().getColor() == currPlayer.getPlayerColor()) {
                        
                        changeBetween(i, j, x, y);
                        break;
                    } else
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
            if (p1Count > p2Count)
                MessageBox.Show("Player 1 Wins");
            else
                MessageBox.Show("Player 2 Wins");
        }

        private void ChangeTurn()
        {
            if (currPlayer == player1)
                currPlayer = player2;
            else
                currPlayer = player1;
        }
        private bool CheckForWin() {

            for(int i = 0; i < gameBoard.getNumRows(); i++)
            {
                for(int j = 0; j < gameBoard.getNumCols(); j++)
                {
                    if (!gameBoard.squares[i, j].isOccupied())
                    {
                        return false;
                    }
                    if (gameBoard.squares[i, j].getPiece() != null && gameBoard.squares[i, j].getPiece().getColor() == player1.getPlayerColor())
                    {
                        p1Count += 1;
                    } else if(gameBoard.squares[i, j].getPiece() != null && gameBoard.squares[i, j].getPiece().getColor() == player2.getPlayerColor())
                    {
                        p2Count += 1; 
                    }
                }
            }

            return true;
        }
    }

}
