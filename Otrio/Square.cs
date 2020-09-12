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

namespace Otrio
{
    class Square
    {
        public int x { get; set; }
        public int y { get; set; }
       
        private Ring SmallRing;
        private Ring MediumRing;
        private Ring LargeRing;

        public Square(int x, int y)
        {
            this.x = x;
            this.y = y; 
            SmallRing = null;
            MediumRing = null;
            LargeRing = null;
        }
        public Square(Square sampleSquare)
        {
            this.x = sampleSquare.x;
            this.y = sampleSquare.y;
            if(sampleSquare.GetSmallRing() != null)
                SmallRing = new Ring(sampleSquare.GetSmallRing().getSize(), sampleSquare.GetSmallRing().getColor());
            if (sampleSquare.GetMediumRing() != null)
                MediumRing = new Ring(sampleSquare.GetMediumRing().getSize(), sampleSquare.GetMediumRing().getColor());
            if (sampleSquare.GetLargeRing() != null)
                LargeRing = new Ring(sampleSquare.GetLargeRing().getSize(), sampleSquare.GetLargeRing().getColor());
        }
        public Square(Ring ring1, Ring ring2, Ring ring3)
        {
            SmallRing = ring1;
            MediumRing = ring2;
            LargeRing = ring3;
        }
        public void RemoveSmallRing()
        {
            if(this.SmallRing != null)
                this.SmallRing = null;
        }
        public void RemoveLargeRing()
        {
            if(this.LargeRing != null)
                this.LargeRing = null;
        }
        public void RemoveMediumRing()
        {
            if(this.MediumRing != null)
                this.MediumRing = null;
        }
        public Ring GetSmallRing()
        {
            return SmallRing; 
        }
        public Ring GetMediumRing()
        {
            return MediumRing;
        }
        public Ring GetLargeRing()
        {
            return LargeRing;
        }
        public void SetSmallRing(char ringColor)
        {
            if(SmallRing == null)
                this.SmallRing = new Ring('S', ringColor);
        }
        public void SetMediumRing(char ringColor)
        {
            if(MediumRing == null)
                this.MediumRing = new Ring('M', ringColor);
        }
        public void SetLargeRing(char ringColor)
        {
            if(LargeRing == null)
                this.LargeRing = new Ring('L', ringColor);
        }

        public void printSquare(DrawingContext dc)
        {
            int startPositionX = 0, startPositionY = 0;

            if (this.x == 0)
                startPositionX = 0;
            else if (this.x == 1)
                startPositionX = 100;
            else if (this.x == 2)
                startPositionX = 200;

            if (this.y == 0)
                startPositionY = 0;
            else if (this.y == 1)
                startPositionY = 100;
            else if (this.y == 2)
                startPositionY = 200;

            dc.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Green, 1), new Rect(startPositionX, startPositionY, 100, 100));
            
            if(SmallRing != null)
                SmallRing.printRing(dc, startPositionX, startPositionY);
            if (MediumRing != null)
                MediumRing.printRing(dc, startPositionX, startPositionY);
            if (LargeRing != null)
                LargeRing.printRing(dc, startPositionX, startPositionY);
        }
        public void highlightSquare(DrawingContext dc)
        {
            int startPositionX = 0, startPositionY = 0;

            if (this.x == 0)
                startPositionX = 0;
            else if (this.x == 1)
                startPositionX = 100;
            else if (this.x == 2)
                startPositionX = 200;

            if (this.y == 0)
                startPositionY = 0;
            else if (this.y == 1)
                startPositionY = 100;
            else if (this.y == 2)
                startPositionY = 200;

            dc.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Green, 1), new Rect(startPositionX, startPositionY, 100, 100));

            if (SmallRing != null)
                SmallRing.highlightRing(dc, startPositionX, startPositionY);
            if (MediumRing != null)
                MediumRing.highlightRing(dc, startPositionX, startPositionY);
            if (LargeRing != null)
                LargeRing.highlightRing(dc, startPositionX, startPositionY);
        }
        
    }
}
