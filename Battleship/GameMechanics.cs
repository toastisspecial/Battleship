using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class GameMechanics
    {
        //This class contains the code for placing ships, when ships fire, and determining if a player has won
        public static bool PlacingShips(ref List<Ship> ships, int startingCol, int startingRow, string direction, string shipType)
        {
            //This method checks to see if ship placement is valid
            //First it checks to make sure the ship isn't being place outside the grid
            //After that it sends it to CheckIfOccupied to see it's intersecting any other ships.
            //This method sets the ship locations even if they are not valid. If the ship placement is not valid this method will return false.
            //The player or the computer will have to pick a new place for the ships if it returns false.
            //When the player picks a new location the ships locations are overriden with the new locations.

            bool isValid = true;
            const string UP = "Up";
            const string DOWN = "Down";
            const string LEFT = "Left";
            const string RIGHT = "Right";

            foreach (Ship s in ships.Where(s => s.ShipType == shipType)) //LINQ to get right ship to set
            {
                
                int prevCol = startingCol;
                int prevRow = startingRow;
                int curCol = 0;
                int curRow = 0;
                int counter = 1;


                s.ShipLocations[0, 0] = startingRow;
                s.ShipLocations[0, 1] = startingCol;

                while (counter < s.Length)
                {
                    switch (direction) // how the rest of the ships is placed is determined by the direction
                    {
                        case UP:
                            curRow = prevRow - 1;
                            s.ShipLocations[counter, 0] = curRow;
                            s.ShipLocations[counter, 1] = startingCol;
                            prevRow = curRow;
                            counter++;
                            break;

                        case DOWN:
                            curRow = prevRow + 1;
                            s.ShipLocations[counter, 0] = curRow;
                            s.ShipLocations[counter, 1] = startingCol;
                            prevRow = curRow;
                            counter++;
                            break;

                        case LEFT:
                            curCol = prevCol - 1;
                            s.ShipLocations[counter, 0] = startingRow;
                            s.ShipLocations[counter, 1] = curCol;
                            prevCol = curCol;
                            counter++;
                            break;

                        case RIGHT:
                            curCol = prevCol + 1;
                            s.ShipLocations[counter, 0] = startingRow;
                            s.ShipLocations[counter, 1] = curCol;
                            prevCol = curCol;
                            counter++;
                            break;
                    }
                }

                if (curCol > 11 || curCol < 0 || curRow > 11 || curRow < 0) //if any of these are true it means the ships went off the grid
                {
                    isValid = false;
                    return isValid;
                }

                // sends to CheckIfOccupied to make sure no ships intersect.
                isValid = CheckIfOccupied(ref ships, s.ShipLocations);
            }
            return isValid;

        }
        public static bool CheckIfOccupied(ref List<Ship> ships, int[,] shipLocations)
        {
            //This method checks to see if the location of the new ship
            //already has a ship in it.

            bool isValid = true;
            int curCol;
            int curRow;
            int curTestRow;
            int curTestCol;
            int counter = 0;

            while (counter < shipLocations.GetLength(0) )
            {
                curRow = shipLocations[counter, 0];
                curCol = shipLocations[counter, 1];

                foreach(Ship curShip in ships)//checks each ship that is placed to make sure ships will not cross
                {
                    if(curShip.IsPlaced == true)
                    {
                        int counter2 = 0;
                        while(counter2 < curShip.ShipLocations.GetLength(0))
                        {
                            curTestRow = curShip.ShipLocations[counter2, 0];
                            curTestCol = curShip.ShipLocations[counter2, 1];
                            if(curCol == curTestCol && curRow == curTestRow)
                            {
                                isValid = false;
                                return isValid;
                            }
                            counter2++;
                        }
                    }
                }
                counter++;
            }


            return isValid;
        }

        public static bool FireIsHit(ref List<Ship> ships, ref int[,] fireHistory, ref int fireHistoryCounter, int fireCol, int fireRow)
        {
            //test to see if fire is a hit or miss on opponents ships
            //adds players shots to an array that keeps track of where players have shot at
            //returns whether the shot hit a ship or not

            int curTestRow;
            int curTestCol;
            bool isHit = false;

            fireHistory[fireHistoryCounter, 0] = fireRow;
            fireHistory[fireHistoryCounter, 1] = fireCol;

            //checking each ship location to see if there is a ship at the coordinates being fired at
            foreach (Ship curShip in ships)
            {
                if (curShip.IsPlaced == true)
                {
                    int counter = 0;
                    while (counter < curShip.ShipLocations.GetLength(0))
                    {
                        curTestRow = curShip.ShipLocations[counter, 0];
                        curTestCol = curShip.ShipLocations[counter, 1];
                        if (fireCol == curTestCol && fireRow == curTestRow)
                        {
                            isHit = true;
                            curShip.HitCount += 1;
                            fireHistory[fireHistoryCounter, 2] = 1;
                            if (curShip.HitCount == curShip.Length)
                            {
                                curShip.IsSunk = true;
                            }
                            fireHistoryCounter++;
                            return isHit;
                        }
                        else
                        {
                            fireHistory[fireHistoryCounter, 2] = 0;
                        }
                        
                        counter++;
                    }

                }
            }
            fireHistoryCounter++;
            return isHit;
        }

        public static bool CheckAllShipsSunk(List<Ship> shipstocheck)
        {
            // this checks to see all of a players ships are sunk.
            bool allSunk = true;
            foreach(Ship s in shipstocheck)
            {
                if (s.IsSunk == false)
                {
                    allSunk = false;
                }
            }
            return allSunk;
        }





    }
}
