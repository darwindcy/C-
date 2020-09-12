using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Othello
{
    class Player
    {
        Color playerColor;
        String playerName;

        public Player(Color C, String Name)
        {
            playerColor = C;
            playerName = Name;
        }

        public Player(Color C)
        {
            playerColor = C;
            playerName = "NO Name";
        }

        public Color getPlayerColor()
        {
            return playerColor;
        }
        public String getName()
        {
            return playerName;
        }
    }
}
