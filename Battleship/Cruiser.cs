using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Cruiser : Ship
    {
        public Cruiser()
        {
            ShipLocations = new int[3, 2];
            Length = 3;
            ShipType = "Cruiser";
            ShipAbbr = "CR";
        }

    }
}
