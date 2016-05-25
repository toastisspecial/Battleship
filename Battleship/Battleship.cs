using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Battleship : Ship
    {
        public Battleship()
        {
            ShipLocations = new int[4, 2];
            Length = 4;
            ShipType = "Battleship";
            ShipAbbr = "BA";
        }
    }
}
