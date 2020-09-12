using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Othello
{
    class Piece
    {
        Color pieceColor;

        public Piece(Color C)
        {
            pieceColor = C;
        }

        public Color getColor()
        {
            return pieceColor; 
        }
        public void DrawPiece(DrawingContext dc, int x, int y)
        {
            SolidColorBrush brush = new SolidColorBrush(pieceColor);
            Pen pen = new Pen(brush, 1);

            dc.DrawEllipse(brush, pen, new System.Windows.Point(x*50 + 25, y*50 + 25), 20, 20);
        }

    }
}
