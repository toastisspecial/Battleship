using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Ship
    {
        bool issunk;
        public string Player { get; set; } //Player name of ship
        public string ShipType { get; set; } //string type of ship
        public int Length { get; set; } // length of ship
        public bool IsSunk // keeps track if ships has sunk
        {
            get { return issunk; }
            set
            {
                issunk = value;
                if ( issunk == true )
                {
                    OnShipSunk(EventArgs.Empty);
                }

            }
        }
        public bool IsPlaced { get; set; } // checks if ship is placed
        public int HitCount { get; set; } // number of hits taken
        public int[,] ShipLocations { get; set; } //array holds ship locations
        public string ShipAbbr { get; set; } //holds two letter ship name


        protected virtual void OnShipSunk(EventArgs e)//event for when ship is sunk
        {
            EventHandler shipSunk = ShipSunk;
            if (shipSunk != null)
            {
                shipSunk(this, e);
            }

        }

        public event EventHandler ShipSunk;

    }
}