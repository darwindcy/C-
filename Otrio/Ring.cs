using System.Windows;
using System.Windows.Media;

namespace Otrio
{
    class Ring
    {
        private char size;
        private char color;
        private bool isVisible;

        public Ring() { size = ' '; color = ' '; }
        public char getSize() { return size;  }
        public char getColor() { return color; }
        public void setSize(char size) { this.size = size; }
        public void setColor(char color) { this.color = color; }
        public bool getVisibility() { return isVisible; }
        public void setVisibility(bool value) { isVisible = value; }
        
        public Ring(char Size, char Color)
        {
            this.size = Size;
            this.color = Color;
        }
        
        public void printRing(DrawingContext dc, int X, int Y)
        {
            Pen pen = new Pen(Brushes.Black, 1);   // Default
            int radiusX = 45, radiusY = 45;        // Default ring size large    

            if (this.color == 'B')
                pen = new Pen(Brushes.Blue, 2);
            else if (this.color == 'R')
                pen = new Pen(Brushes.Red, 2);
            else if (this.color == 'G')
                pen = new Pen(Brushes.Green, 2);
            else if (this.color == 'Y')
                pen = new Pen(Brushes.Yellow, 2);
            else 
                pen = new Pen(Brushes.Gray, 2);

            if (this.size == 'S')
            {
                radiusX = 15; radiusY = 15;
            } else if (this.size == 'M')
            {
                radiusX = 30; radiusY = 30;
            } 

            dc.DrawEllipse(Brushes.Transparent, pen, new Point(X + 50, Y + 50), radiusX, radiusY);
        }
        public void highlightRing(DrawingContext dc, int X, int Y)
        {

            int radiusX = 45, radiusY = 45;        // Default ring size large    

            Pen pen = new Pen(Brushes.Gray, 1);

            dc.DrawEllipse(Brushes.Transparent, pen, new Point(X + 50, Y + 50), radiusX, radiusY);
        }
    }
}
