using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Carrier : Ship
    {

        public Carrier()
        {
            ShipLocations = new int[5, 2];
            Length = 5;
            ShipType = "Carrier";
            ShipAbbr = "CA";
        }


    }
}
