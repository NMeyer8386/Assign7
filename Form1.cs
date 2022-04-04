using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assign7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Player[] playerArray = new Player[30];  //Array of player objects
        int counter = 0;                        //Counter to keep track of players

        public class Player
        {
            public readonly string readPlayer;
            public string playerName { get { return readPlayer; } }

            public int jerseyNum { get; }

            public int goals { get; set; }

            public Player(string playerIn, int jerseyIn, int goalsIn)
            {
                readPlayer = playerIn;
                jerseyNum = jerseyIn;
                goals = goalsIn;
            }
        }

        private void CreatePlayer_Click(object sender, EventArgs e)
        {
            string player = PlayerNameBox.Text;
            int jersey, goals;
            bool success = true;

            if (counter < 30)
            {
                while (!int.TryParse(JerseyNumBox.Text, out jersey))    //checks for number in jersey
                {
                    MessageBox.Show("Please enter a number.", "Error");
                    JerseyNumBox.Text = "";
                    success = false;
                    break;
                }

                while (!int.TryParse(GoalsBox.Text, out goals))         //checks for number in goals
                {
                    MessageBox.Show("Please enter a number.", "Error");
                    GoalsBox.Text = "";
                    success = false;
                    break;
                }

                if (success)    //no errors
                {
                    playerArray[counter] = new Player(player, jersey, goals);
                    counter++;
                    String rowString = stringToRow(player, jersey, goals);
                    PlayerList.Items.Add(rowString);
                    PlayerNameBox.Text = "";
                    JerseyNumBox.Text = "";
                    GoalsBox.Text = "";
                }
            }
            else   //Max player count error
            {
                MessageBox.Show("Maximum Player Count Reached.", "Error");
                PlayerNameBox.Text = "";
                JerseyNumBox.Text = "";
                GoalsBox.Text = "";
            }
        }

        public String stringToRow(String name, int jersey, int goals)
        {
            String player = name;
            String jerseyToString = Convert.ToString(jersey);
            String goalsToString = Convert.ToString(goals);

            String rowString = String.Format("{0, -5}{1, 16}{2, 8}", player, jerseyToString, goalsToString);

            return rowString;
        }

        private void PlayerList_DoubleClick(object sender, EventArgs e)
        {

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {

        }
    }
}
