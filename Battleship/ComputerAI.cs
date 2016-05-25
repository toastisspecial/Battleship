using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class ComputerAI
    {
        
        public static void SettingAIShips(ref List<Ship> computerShips)
        {
            //sets the ships for the computer in random places

            //Direction Constants
            const string UP = "Up";
            const string DOWN = "Down";
            const string LEFT = "Left";
            const string RIGHT = "Right";

            Random r = new Random();
            bool shipValid = false;
            int startingColumn;
            int startingRow;
            int directionNum;
            string direction = UP;


            foreach (Ship s in computerShips)//setting location for each ship
            {
                while (shipValid == false) // repeat until ship placement is valid
                {
                    // getting new random numbers
                    startingColumn = r.Next(12);
                    startingRow = r.Next(12);
                    directionNum = r.Next(4);
                    //switch case to convert number to direction
                    switch (directionNum)
                    {
                        case 0:
                            direction = UP;
                            break;
                        case 1:
                            direction = LEFT;
                            break;
                        case 2:
                            direction = RIGHT;
                            break;
                        case 3:
                            direction = DOWN;
                            break;
                    }
                    //send to check if placement is valid
                    shipValid = GameMechanics.PlacingShips(ref computerShips, startingColumn, startingRow, direction, s.ShipType);

                }
                s.IsPlaced = true; // sets ship property to ship is placed
                shipValid = false; // reseting shipValid
            }
        }

        
        public static int[] ComputerMove(int[,] computerShots, int computerShotsCounter)
        {
            //Gets unique coordinates for computer to fire at

            Random r = new Random();
            int callColumn;
            int callRow;
            int counter;
            int[] rowCol = new int[2]; //array to send back row and column
            bool alreadyFired = false;
            do//checks to make sure computer has not shot at this location before
            {
                callColumn = r.Next(12); //getting random
                callRow = r.Next(12);
                counter = 0;
                alreadyFired = false;// reseting bool
                while (counter < computerShotsCounter)
                {
                    // if true this is a location the computer has already shot at
                    if (computerShots[counter, 0] == callRow && computerShots[counter, 1] == callColumn)
                    {
                        alreadyFired = true;
                        break;
                    }
                    counter++;
                }
            } while (alreadyFired == true);

            //sets coords for computer to shoot at
            rowCol[0] = callRow;
            rowCol[1] = callColumn;

            return rowCol;
        }
    }
}
