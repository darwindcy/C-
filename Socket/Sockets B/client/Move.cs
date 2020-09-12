using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Move
    {
        Player movingPlayer;
        Square movingSquare;

        public Move(Player P, Square s) 
        {
            movingPlayer = P;
            movingSquare = s;
        }

        public Player GetMovingPlayer()
        {
            return movingPlayer;
        }

        public Square GetMovingSquare()
        {
            return movingSquare;
        }
        
        public bool IsValid()
        {
            if (movingSquare.isOccupied())
                return false;
            return true; 
        }
        public String toString()
        {
            return "[" + movingSquare.getX() + ", " + movingSquare.getY() + "]";
        }
    }
}
