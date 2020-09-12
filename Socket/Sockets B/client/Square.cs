using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Square
    {
        int x;
        int y;
        Piece piece;
        Char achar = ' ';

        public Square(int a, int b, char c)
        {
            x = a;  y = b; piece = null; achar = c;
        }
        public Square(int a, int b)
        {
            x = a; y = b; piece = null;
        }
        public char getChar()
        {
            return achar;
        }
        public bool isOccupied() 
        {
            if (piece != null)
                return true;
            else
                return false; 
        }
        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }
        public void setPiece(Piece aPiece)
        {
            piece = aPiece;
        }
        public void removePiece()
        {
            piece = null;
        }
        public Piece getPiece()
        {
             return piece;
        }

        public void DrawSquare(System.Windows.Media.DrawingContext dc)
        {
            if (piece != null)
                piece.DrawPiece(dc, x, y);
        }

        public void HighLight(System.Windows.Media.DrawingContext dc)
        {
            System.Windows.Media.SolidColorBrush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            System.Windows.Media.Pen pen = new System.Windows.Media.Pen(brush, 1);

            dc.DrawEllipse(brush, pen, new System.Windows.Point(x * 50 + 25, y * 50 + 25), 20, 20);
        }
    }
}
