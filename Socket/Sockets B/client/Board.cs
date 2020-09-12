using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Othello
{
    class Board
    {
        int numRows;
        int numCols;
        public Square[,] squares;
        private HashSet<Move> Moves = new HashSet<Move>();

        public Board(int row, int col)
        {
            numRows = row + 2;
            numCols = col + 2;
            squares = new Square[numRows, numCols];
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    squares[i, j] = new Square(i, j);
                }
            }
        }
        public HashSet<Move> getMoves(Player currentPlayer)
        {
            Moves = new HashSet<Move>();

            FindMoves(currentPlayer);
            return Moves;
        }

        private void FindMoves(Player currentPlayer)
        {
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    if (!squares[i, j].isOccupied())
                    {
                        squares[i, j].setPiece(new Piece(currentPlayer.getPlayerColor()));

                        checkLeft(squares[i, j], currentPlayer);
                        checkRight(squares[i, j], currentPlayer);

                        checkUp(squares[i, j], currentPlayer);
                        checkDown(squares[i, j], currentPlayer);

                        checkUpLeft(squares[i, j], currentPlayer);
                        checkUpRight(squares[i, j], currentPlayer);
                        checkDownLeft(squares[i, j], currentPlayer);
                        checkDownRight(squares[i, j], currentPlayer);

                        squares[i, j].removePiece();
                    }
                }
            }
        }
        private void checkUpLeft(Square currentSquare, Player cPlayer)
        {
            int i = currentSquare.getX();
            int j = currentSquare.getY();

            while (i >= 2  && j >= 2)
            {
                Square sq = squares[i - 1, j - 1];
                if (currentSquare.getY() - sq.getY() >= 2 && currentSquare.getX() - sq.getX() >= 2 &&
                    sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    if (LRDiagonalCheck(sq, currentSquare))
                        Moves.Add(new Move(cPlayer, currentSquare));

                    break;
                }
                else if (currentSquare.getY() - sq.getY() <= 1 && currentSquare.getX() - sq.getX() <= 1 &&
                        sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    break;
                }
                i -= 1; j -= 1;
            }
        }

        private void checkDownRight(Square currentSquare, Player cPlayer)
        {
            int i = currentSquare.getX();
            int j = currentSquare.getY();

            while (j <= 7 && i <= 7)
            {
                Square sq = squares[i + 1, j + 1];
                if (sq.getY() - currentSquare.getY() >= 2 && sq.getX() - currentSquare.getX() >= 2 &&
                    sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    if (LRDiagonalCheck(currentSquare, sq))
                        Moves.Add(new Move(cPlayer, currentSquare));

                    break;
                }
                else if (currentSquare.getY() - sq.getY() <= 1 && currentSquare.getX() - sq.getX() <= 1 &&
                        sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    break;
                }
                i += 1; j += 1;
            }
        }
        private bool LRDiagonalCheck(Square s1, Square s2)
        {
            bool addition = false;

            int i = s1.getX();
            int j = s1.getY();
            if (s2.getX() - s1.getX() == 2 && s2.getY() - s1.getY() == 2)
            {
                if (squares[i + 1, j + 1].isOccupied() && s1.isOccupied() &&
                    squares[i + 1, j + 1].getPiece().getColor() != s1.getPiece().getColor())
                {
                    addition = true;
                }
            }
            else
            {
                Square required = squares[i + 1, j + 1];

                while (i < s2.getX() && j < s2.getY())
                {
                    if (squares[i, j].isOccupied() && required.isOccupied() &&
                        squares[i, j].getPiece().getColor() == required.getPiece().getColor())
                    {

                        addition = true;
                    }
                    else
                        addition = false;
                    i += 1; j += 1; 
                }

            }
            return addition;
        }
        private void checkDownLeft(Square currentSquare, Player cPlayer)
        {
            int i = currentSquare.getX();
            int j = currentSquare.getY();

            while (j <= 7 && i >= 2)
            {
                Square sq = squares[i - 1, j + 1];
                if (sq.getY() - currentSquare.getY() >= 2 && currentSquare.getX() - sq.getX() >= 2 &&
                    sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    if (RLDiagonalCheck(currentSquare, sq))
                    {
                        Moves.Add(new Move(cPlayer, currentSquare));
                    }
                    break;
                }
                else if (sq.getY() - currentSquare.getY() <= 1 && currentSquare.getX() - sq.getX() <= 1 &&
                        sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    break;
                }
                i -= 1; j += 1;
            }
        }
        private void checkUpRight(Square currentSquare, Player cPlayer)
        {
            int i = currentSquare.getX();
            int j = currentSquare.getY();

            while (j >= 2 && i <= 7)
            {
                Square sq = squares[i + 1, j - 1];
                if (currentSquare.getY() - sq.getY() >= 2 && sq.getX() - currentSquare.getX() >= 2 &&
                    sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    if (RLDiagonalCheck(sq, currentSquare))
                        Moves.Add(new Move(cPlayer, currentSquare));

                    break;
                }
                else if (currentSquare.getY() - sq.getY() <= 1 && sq.getX() - currentSquare.getX() <= 1 &&
                        sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    break;
                }
                i += 1; j -= 1;
            }

        }
        private bool RLDiagonalCheck(Square s1, Square s2)
        {
            bool addition = false;

            int i = s1.getX();
            int j = s1.getY();
            if (s1.getX() - s2.getX() == 2 && s2.getY() - s1.getY() == 2)
            {
                if (squares[i - 1, j + 1].isOccupied() && s1.isOccupied() &&
                    squares[i - 1, j + 1].getPiece().getColor() != s1.getPiece().getColor())
                {
                    addition = true;
                }
            }
            else
            {
                Square required = squares[i - 1, j + 1];

                while (i > s2.getX() && j < s2.getY())
                {
                    if (squares[i, j].isOccupied() && required.isOccupied() &&
                        squares[i, j].getPiece().getColor() == required.getPiece().getColor())
                    {

                        addition = true;
                    }
                    else
                        addition = false;
                    i -= 1; j += 1;
                }

            }
            return addition;
        }

        private void checkDown(Square currentSquare, Player cPlayer)
        {
            int i = currentSquare.getX();
            int j = currentSquare.getY();

            while (j <= 7)
            {
                //Console.WriteLine(i + " " + j);
                Square sq = squares[i, j + 1];
                if (sq.getY() - currentSquare.getY() >= 2 &&
                    sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    //Console.WriteLine("Same square found " + sq.getX() + " " + sq.getY());
                    Console.WriteLine("About to Check between " + currentSquare.getY() + " " + sq.getY());
                    if (CheckVerticalBetween(currentSquare, sq))
                        Moves.Add(new Move(cPlayer, currentSquare));

                    break;
                }
                else if (currentSquare.getY() - sq.getY() <= 1 &&
                        sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    break;
                }
                j += 1;
            }
        }
        private void checkUp(Square currentSquare, Player cPlayer)
        {
            int i = currentSquare.getX();
            int j = currentSquare.getY();

            while (j >= 2)
            {
                //Console.WriteLine(i + " " + j);
                Square sq = squares[i, j - 1];
                if (currentSquare.getY() - sq.getY() >= 2 &&
                    sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    //Console.WriteLine("Same square found " + sq.getX() + " " + sq.getY());

                    if (CheckVerticalBetween(sq, currentSquare))
                        Moves.Add(new Move(cPlayer, currentSquare));

                    break;
                }
                else if (currentSquare.getY() - sq.getY() <= 1 &&
                        sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    break;
                }
                j -= 1;
            }
        }

        private bool CheckVerticalBetween(Square s1, Square s2)
        {
            bool addition = false;

            int i = s1.getX();
            if (s2.getY() - s1.getY() == 2)
            {
                //Console.WriteLine("Checking " + (s1.getX() + 1) + " " + j);
                //Console.WriteLine(s1.getPiece().getColor().ToString());
                if (squares[i, s1.getY() + 1].isOccupied() && s1.isOccupied() &&
                    squares[i, s1.getY() + 1].getPiece().getColor() != s1.getPiece().getColor())
                {
                    //Console.WriteLine("Square Found");
                    addition = true;
                }
            }
            else
            {
                Square required = squares[i, s1.getY() + 1];
                Console.WriteLine("Checking from " + (s1.getY() + 1) + " to " + (s2.getY() - 1));
                for (int j = s1.getY() + 1; j < s2.getY(); j++)
                {
                    if (squares[i, j].isOccupied() && required.isOccupied() &&
                        squares[i, j].getPiece().getColor() == required.getPiece().getColor())
                    {
                        //Console.WriteLine("Same squares are " + i + " " + (i + 1));
                        addition = true;
                    }
                    else
                        addition = false;
                }
                /*                if (addition)
                                    Console.WriteLine("From " + (s1.getX()+1) + " to " + (s2.getX()-1) + " Are the Same");*/
            }
            return addition;
        }

        private void checkRight(Square currentSquare, Player cPlayer)
        {
            int i = currentSquare.getX();
            int j = currentSquare.getY();

            while (i <= 7)
            {
                //Console.WriteLine(i + " " + j);
                Square sq = squares[i + 1, j];
                if (sq.getX() - currentSquare.getX() >= 2 &&
                    sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    //Console.WriteLine("Same square found " + sq.getX() + " " + sq.getY());
                    Console.WriteLine("About to Check between " + currentSquare.getX() + " " + sq.getX());
                    if (checkHorizontalBetween(currentSquare, sq))
                        Moves.Add(new Move(cPlayer, currentSquare));

                    break;
                }
                else if (currentSquare.getX() - sq.getX() <= 1 &&
                        sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    break;
                }
                i += 1;
            }
        }

        private void checkLeft(Square currentSquare, Player cPlayer)
        {
            int i = currentSquare.getX();
            int j = currentSquare.getY();

            while (i >= 2)
            {
                //Console.WriteLine(i + " " + j);
                Square sq = squares[i - 1, j];
                if (currentSquare.getX() - sq.getX() >= 2 &&
                    sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    //Console.WriteLine("Same square found " + sq.getX() + " " + sq.getY());

                    if (checkHorizontalBetween(sq, currentSquare))
                        Moves.Add(new Move(cPlayer, currentSquare));

                    break;
                }
                else if (currentSquare.getX() - sq.getX() <= 1 &&
                        sq.isOccupied() && sq.getPiece().getColor() == cPlayer.getPlayerColor())
                {
                    break;
                }
                i -= 1;
            }
        }
        private bool checkHorizontalBetween(Square s1, Square s2)
        {

            //Console.WriteLine("From " + s1.getX() + " To " + s2.getX());
            bool addition = false;

            int j = s1.getY();
            if (s2.getX() - s1.getX() == 2)
            {
                //Console.WriteLine("Checking " + (s1.getX() + 1) + " " + j);
                //Console.WriteLine(s1.getPiece().getColor().ToString());
                if (squares[s1.getX() + 1, j].isOccupied() && s1.isOccupied() &&
                    squares[s1.getX() + 1, j].getPiece().getColor() != s1.getPiece().getColor())
                {
                    //Console.WriteLine("Square Found");
                    addition = true;
                }
            }
            else
            {
                Square required = squares[s1.getX() + 1, j];
                Console.WriteLine("Checking from " + (s1.getX() + 1) + " to " + (s2.getX() - 1));
                for (int i = s1.getX() + 1; i < s2.getX(); i++)
                {
                    if (squares[i, j].isOccupied() && required.isOccupied() &&
                        squares[i, j].getPiece().getColor() == required.getPiece().getColor())
                    {
                        //Console.WriteLine("Same squares are " + i + " " + (i + 1));
                        addition = true;
                    }
                    else
                        addition = false;
                }
                /*                if (addition)
                                    Console.WriteLine("From " + (s1.getX()+1) + " to " + (s2.getX()-1) + " Are the Same");*/
            }
            return addition;
        }

        private void setBorderAlphabets()
        {
            for (int i = 1; i < numRows - 1; i++)
            {
                squares[i, 0] = new Square(i, 0, (char)i);
                squares[i, numCols - 2] = new Square(i, 0, (char)i);
            }
        }
        public void setPiece(int x, int y, Piece P)
        {
            squares[x, y].setPiece(P);
        }
        public int getNumRows()
        {
            return numRows;
        }

        public int getNumCols()
        {
            return numCols;
        }

        public bool IsEmpty(int x, int y)
        {
            return (!squares[x, y].isOccupied());
        }

        public RenderTargetBitmap BoardImage()
        {
            
            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (i == 0 || i == numRows - 1 || j == 0 || j == numCols - 1)
                    {
                        dc.DrawRectangle(Brushes.White, new Pen(Brushes.Transparent, 1), new System.Windows.Rect(i * 50, j * 50, 50, 50));
                        FormattedText formattedText = new FormattedText(squares[i, j].getChar().ToString(),
                                                                        System.Globalization.CultureInfo.GetCultureInfo("en-us"),
                                                                        System.Windows.FlowDirection.LeftToRight,
                                                                        new Typeface("Verdana"), 15, Brushes.Black, VisualTreeHelper.GetDpi(vis).PixelsPerDip);
                        dc.DrawText(formattedText, new System.Windows.Point(i * 50 + 15, j * 50 + 15));


                        //dc.DrawText(new FormattedText(squares[i, j].getChar().ToString(), CultureInfo("en-us"), flowDirection.LeftToRight, new Typeface("Arial"), 15, Brushes.Black))), new System.Windows.Point(i * 50, j * 50));
                    }
                    else
                    {
                        dc.DrawRectangle(Brushes.Green, new Pen(Brushes.Black, 1), new System.Windows.Rect(i * 50, j * 50, 50, 50));
                        squares[i, j].DrawSquare(dc);

                        if (!squares[i, j].isOccupied())
                        {
                            foreach (Move move in Moves)
                            {
                                if (move.GetMovingSquare().getX() == i && move.GetMovingSquare().getY() == j)
                                    squares[i, j].HighLight(dc);
                            }
                        }
                    }
                }
            }

            dc.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(500, 500, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(vis);

            return bmp;

        }
    }
}
