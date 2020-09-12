using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractals
{
    class Coordinate
    {
        private int x;
        private int y; 

        public int GetX()
        {
            return x; 
        }
        public int GetY()
        {
            return y;
        }
        public void SetX(int X)
        {
            x = X;
        }
        public void SetY(int Y)
        {
            y = Y;
        }

        public Coordinate(int X, int Y)
        {
            x = X; y = Y;
        } 
        
        public Coordinate GetWorldCoordinate()
        {
            int newX = x;
            int newY = y;
            return new Coordinate(newX, newY);
        }
    }
}
