using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class PtBoat : Ship
    {
        public PtBoat()
        {
            ShipLocations = new int[2, 2];
            Length = 2;
            ShipType = "Pt Boat";
            ShipAbbr = "PT";
        }

    }
}
