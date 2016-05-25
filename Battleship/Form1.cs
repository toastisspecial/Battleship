using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{

    public partial class Form1 : Form
    {
        //for these arrays the first 2 columns hold the row and column and the 3rd column holds whether shot was a hit or miss
        int[,] computerShotsFired = new int[100, 3]; // Array of shots fired by the computer
        int computerShotsFiredCounter = 0; //counter for shots fire by computer

        int[,] playerShotsFired = new int[200, 3]; // Array of shots fired by player
        int playerShotsFiredCount = 0; //counter for shots fired by player

        const string ALPHA = "ABCDEFGHIJKL"; //used for alpha-numeric numbering
        string Player1Name = "Player 1"; //name for player 1
        string Player2Name = "Computer"; //name for player 2
        bool isWinner = false;

        List<Ship> playerShips = new List<Ship> //list of player ships
        {
            new PtBoat(), new Submarine(), new Cruiser(), new Battleship(), new Carrier()
        };

        List<Ship> computerShips = new List<Ship> //list of computer ships
        {
            new PtBoat(), new Submarine(), new Cruiser(), new Battleship(), new Carrier()
        };



        public Form1()
        {
            InitializeComponent();

            //below is naming the player of the ship and subscribing to each ships event
            foreach(Ship s in playerShips)
            {
                s.ShipSunk += s_ShipSunk;
                s.Player = Player1Name;
            }
            foreach (Ship s in computerShips)
            {
                s.ShipSunk += s_ShipSunk;
                s.Player = Player2Name;
            }

            //Below is creating the radio buttons for each cell in the tablelayoutpanels
            int checkrow = 0; //counter for filling rows
            int checkcol = 0; //counter for filling columns
            int countArrayFill = 0;//counts for fill array
            int counter = 0;//counter for while loops

            ComputerAI.SettingAIShips(ref computerShips); //setting Computer Ships


            //creates 144 check boxes
            RadioButton[] radioButtonsShips = new RadioButton[144]; //array of checkboxes
            while (countArrayFill < 144) //filling array
            {
                RadioButton putIn = new RadioButton(); //creates new checkbox
                putIn.Text = ""; //removing text
                putIn.Size = new System.Drawing.Size(14, 15);// resizing
                putIn.Anchor = AnchorStyles.None; //removing anchors
                putIn.BackColor = Color.Transparent; //change back color
                radioButtonsShips[countArrayFill] = putIn; //adding checkbox to array
                radioButtonsShips[countArrayFill].Name = "rbShip" + countArrayFill; //adding checkbox name

                countArrayFill++;
            }

            //while loop that fills tablelayoutpanel with checkboxes
            while (checkrow < 12)
            {
                while (checkcol < 12)
                {
                    tableLayoutPanel2.Controls.Add(radioButtonsShips[counter], checkcol, checkrow);
                    checkcol++;
                    counter++;
                }
                checkrow++;
                checkcol = 0;
            }


            //creates 144 radio buttons
            RadioButton[] radioButtons = new RadioButton[144];// creates an array of radio buttons

            countArrayFill = 0; //reseting counter
            while (countArrayFill < 144) //filling array
            {
                RadioButton putIn = new RadioButton(); //creating radio button
                putIn.Text = ""; //removing text
                putIn.Size = new System.Drawing.Size(14, 15); //resizing
                putIn.Anchor = AnchorStyles.None;// removing anchor
                putIn.BackColor = Color.Transparent; //change back color
                putIn.CheckedChanged += new System.EventHandler(checkBoxUnlockButton); //event to unlock button
                radioButtons[countArrayFill] = putIn; //adding check box to array
                radioButtons[countArrayFill].Name = "rbTarget" + countArrayFill; //giving checkbox a name

                countArrayFill++;
            }

            //reseting counters
            checkrow = 0;
            checkcol = 0;
            counter = 0;
            while (checkrow < 12) //while loop to fill tablelayoutpanel
            {
                while (checkcol < 12)
                {
                    tableLayoutPanel1.Controls.Add(radioButtons[counter], checkcol, checkrow);
                    checkcol++;
                    counter++;
                }
                checkrow++;
                checkcol = 0;
            }
        }



        private void buttonFire_Click(object sender, EventArgs e)
        {
            // This event handles what to do when a player fires.
            // After the player fires it lets the computer firing using ComputerTurn().
            // It also checks if there is a winner.


            bool isHit;
            //LINQ to find which radio is checked
            var checkedRadio = tableLayoutPanel1.Controls.OfType<RadioButton>()
                                                         .FirstOrDefault(r => r.Checked);
            checkedRadio.Checked = false; //unchecks the selected radio button

            checkedRadio.Enabled = false; //disables the selected radio button



            int controlRow = tableLayoutPanel1.GetRow(checkedRadio); //gets radiobutton row
            int controlColumn = tableLayoutPanel1.GetColumn(checkedRadio); //gets radiobutton column

            //checks to see if the any ships are hit from the player firing. This method returns a bool.
            //If any ships are hit that is also handled in GameMechanics.FireIsHit.
            isHit = GameMechanics.FireIsHit(ref computerShips, ref playerShotsFired, ref playerShotsFiredCount, controlColumn, controlRow);


            lbMoves.Items.Add("Player fires at " + ALPHA[controlRow] + "-" + (controlColumn + 1) + ".  Was a ship hit: " + isHit);

            buttonFire.Enabled = false; //disabling button

            tableLayoutPanel1.Refresh(); //redraws tablelayoutpanel so cells can be painted

            //computer can only go if player hasn't won
            if( isWinner == false)
            {
                ComputerTurn();
            }


            tableLayoutPanel2.Refresh(); //redraws tablelayoutpanel so cells can be painted


        }

        private void checkBoxUnlockButton(object sender, EventArgs e)
        {
            buttonFire.Enabled = true; //enabling button when checkbox is checked
        }


        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            //painting cells for tableLayoutPanel1
            if (playerShotsFired != null) //if array is not null
            {

                int counter = 0;
                while (counter < playerShotsFiredCount) //counter is less than array
                {
                    int rowFire = playerShotsFired[counter, 0]; //getting row of cell
                    int colFire = playerShotsFired[counter, 1]; //getting column of cell

                    if (playerShotsFired[counter, 2] == 1)
                    {
                        if (e.Row == rowFire && e.Column == colFire) //painting cell
                        {
                            Graphics g = e.Graphics;
                            Rectangle r = e.CellBounds;
                            g.FillRectangle(new SolidBrush(Color.Red), r);
                        }
                    }
                    else if (playerShotsFired[counter, 2] == 0)
                    {
                        if (e.Row == rowFire && e.Column == colFire) //painting cell
                        {
                            Graphics g = e.Graphics;
                            Rectangle r = e.CellBounds;
                            g.FillRectangle(new SolidBrush(Color.White), r);
                        }
                    }

                    counter++;
                }
            }

        }

        private void bttnPlace_Click(object sender, EventArgs e)
        {
            try
            {
                int controlRow; // gets starting row for ship placement
                int controlColumn;// gets starting column for ship placement
                string direction;//gets direction for ship placement
                string shipType;//gets shiptype for ship placement
                bool shipValid; //keeps track of whether ship placement is valid

                //LINQ to find which radio is checked
                var checkedRadio = tableLayoutPanel2.Controls.OfType<RadioButton>()
                                                             .FirstOrDefault(r => r.Checked);

                controlRow = tableLayoutPanel2.GetRow(checkedRadio); //gets radiobutton row
                controlColumn = tableLayoutPanel2.GetColumn(checkedRadio); //gets radiobutton column
                checkedRadio.Checked = false;

                //Linq to find which direction is selected
                checkedRadio = gbDirection.Controls.OfType<RadioButton>()
                                                             .FirstOrDefault(r => r.Checked);
                direction = checkedRadio.Text;
                checkedRadio.Checked = false;

                //Linq to find out type of ship selected
                checkedRadio = gbShip.Controls.OfType<RadioButton>()
                                                 .FirstOrDefault(r => r.Checked);
                shipType = checkedRadio.Tag.ToString();

                //sends user data to isShipPlaceValid() to see if ship placement is valid
                shipValid = GameMechanics.PlacingShips(ref playerShips, controlColumn, controlRow, direction, shipType);

                //if ship placement is valid sets ship placement in object list
                if (shipValid == true)
                {

                    foreach (Ship s in playerShips.Where(s => s.ShipType == shipType)) //LINQ to get right ship to set
                    {

                        s.IsPlaced = true;

                    }

                    checkedRadio.Enabled = false; //disables set ship radio button
                    checkedRadio.Checked = false; // unselects ship

                }
                else //if ships placement is not valid sends message to user
                {
                    MessageBox.Show("Ship placement invalid.");
                }
                tableLayoutPanel2.Refresh();
            }
            catch (ArgumentNullException)//catch statement to catch unselected radio buttons
            {
                MessageBox.Show("Please select a ship location on the grid.");
            }
            catch (NullReferenceException)//catch statement to catch unselected radio buttons
            {
                MessageBox.Show("Please choose a ship or a direction for the ship.");
            }

        }


        private void tableLayoutPanel2_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            //Painting cells for tableLayoutPanel2

            //Painting ship locations
            foreach (Ship s in playerShips)
            {
                if (s.IsPlaced == true) //if array is not null
                {
                    int counter = 0;
                    while (counter < s.Length)
                    {
                        int rowFire = s.ShipLocations[counter, 0];
                        int colFire = s.ShipLocations[counter, 1];

                        if (e.Row == rowFire && e.Column == colFire) //painting cell
                        {
                            Graphics g = e.Graphics;
                            Rectangle r = e.CellBounds;
                            g.FillRectangle(new SolidBrush(Color.Gray), r);
                        }
                        counter++;

                    }
                }

            }

            //Painting computershotsfired
            if (computerShotsFired != null) //if array is not null
            {

                int counter = 0;
                while (counter < computerShotsFiredCounter) //counter is less than array
                {
                    int rowFire = computerShotsFired[counter, 0]; //getting row of cell
                    int colFire = computerShotsFired[counter, 1]; //getting column of cell

                    if (computerShotsFired[counter, 2] == 1)
                    {
                        if (e.Row == rowFire && e.Column == colFire) //painting cell
                        {
                            Graphics g = e.Graphics;
                            Rectangle r = e.CellBounds;
                            g.FillRectangle(new SolidBrush(Color.Red), r);
                        }
                    }
                    else if (computerShotsFired[counter, 2] == 0)
                    {
                        if (e.Row == rowFire && e.Column == colFire) //painting cell
                        {
                            Graphics g = e.Graphics;
                            Rectangle r = e.CellBounds;
                            g.FillRectangle(new SolidBrush(Color.White), r);
                        }
                    }

                    counter++;
                }
            }

        }

        private void ShipCheckedChanged(object sender, EventArgs e)
        {
            //Checks to see if all ships have been placed. If they have it allows player to start the game

            if (rbBattleship.Enabled == false && rbCarrier.Enabled == false && rbCruiser.Enabled == false
                && rbPTBoat.Enabled == false && rbSubmarine.Enabled == false)
            {
                bttnPlace.Enabled = false;
                bttnStart.Enabled = true;

            }
        }

        private void bttnStart_Click(object sender, EventArgs e)
        {
            //Getting the game setup
            tableLayoutPanel1.Enabled = true;
            pnlPlaceShip.Visible = false;
            pnlMoves.Visible = true;
            tableLayoutPanel2.Controls.Clear();
            LabelPlayerShips();

        }
        private void LabelPlayerShips()
        {
            //removing radio buttons and labeling player ships
            Label[] lblShipArray = new Label[17]; //array of labels
            int counterFill = 0;

            while (counterFill < lblShipArray.Length) // filling array of labels
            {
                Label lblShipAbbr = new Label();
                lblShipAbbr.ForeColor = Color.White;
                lblShipAbbr.Anchor = AnchorStyles.Left;
                lblShipAbbr.BackColor = Color.Transparent;
                lblShipAbbr.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
                lblShipAbbr.Size = new System.Drawing.Size(23, 19);


                lblShipArray[counterFill] = lblShipAbbr;
                counterFill++;

            }

            counterFill = 0;

            foreach (Ship sh in playerShips)// placing the label for each player ship
            {
                int counter = 0;
                while (counter < sh.Length)
                {

                    lblShipArray[counterFill].Text = sh.ShipAbbr;
                    tableLayoutPanel2.Controls.Add(lblShipArray[counterFill], sh.ShipLocations[counter, 1], sh.ShipLocations[counter, 0]);
                    counter++;
                    counterFill++;
                }
            }

        }
        private void ComputerTurn()
        {
            //This gets the coordinates for the computer to fire at then sends it to see if its a hit.
            bool isHit;
            int[] compHitCords = ComputerAI.ComputerMove(computerShotsFired, computerShotsFiredCounter);
            int compShotRow = compHitCords[0];
            int compShotCol = compHitCords[1];
            isHit = GameMechanics.FireIsHit(ref playerShips, ref computerShotsFired, ref computerShotsFiredCounter, compShotCol, compShotRow);
            lbMoves.Items.Add("Computer fires at " + ALPHA[compShotRow] + "-" + (compShotCol + 1) + ".  Was a ship hit: " + isHit);
        }

        private void s_ShipSunk(object sender, EventArgs e)
        {
            //what to do when a ship is sunk
            bool allShipsSunk;
            var ship = sender as Ship;
            string messageLb = "The following hit has sunk " + ship.Player + "'s " + ship.ShipType + ".";
            string messageBox = ship.Player + "'s " + ship.ShipType + " has been sunk.";
            MessageBox.Show(messageBox);
            lbMoves.Items.Add(messageLb);

            //if a ship is sunk checks to see if all ships are sunk
            //if all ships are sunk determines winner
            if(ship.Player == Player1Name)
            {
                allShipsSunk = GameMechanics.CheckAllShipsSunk(playerShips);
                if( allShipsSunk == true)
                {
                    MessageBox.Show("Game Over. The computer has won.");                   
                    tableLayoutPanel1.Enabled = false;
                    isWinner = true;
                }
            }
            else if(ship.Player == Player2Name)
            {
                allShipsSunk = GameMechanics.CheckAllShipsSunk(computerShips);
                if (allShipsSunk == true)
                {
                    MessageBox.Show("Congratulations, you won!");
                    tableLayoutPanel1.Enabled = false;
                    isWinner = true;
                }
            }
        }

        private void bttnQuit_Click(object sender, EventArgs e)
        {
            //asking user if they are sure they want to quit
            DialogResult quitConfirm = MessageBox.Show("Are you sure you want to quit?","Battleship",MessageBoxButtons.YesNo);
            if(quitConfirm == DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }
    }
}
