using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

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

        [Serializable]
        internal class Player : IComparable
        {
            public string playerName { get; set; }

            public int jerseyNum { get; set; }

            public int goals { get; set; }

            public static char sortID = 'n';    //Identifier for sorting


            public Player(string playerIn, int jerseyIn, int goalsIn)
            {
                playerName = playerIn;
                jerseyNum = jerseyIn;
                goals = goalsIn;
            }

            public int CompareTo(object obj)
            {
                Player temp = (Player)obj;

                switch (sortID)
                {
                    case 'j':
                        return this.jerseyNum.CompareTo(temp.jerseyNum);
                    case 'g':
                        return this.goals.CompareTo(temp.goals);
                    default:
                        return this.playerName.CompareTo(temp.playerName);
                }
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

            String rowString = String.Format("{0, -20}{1, 5}{2, 8}", player, jerseyToString, goalsToString);

            return rowString;
        }

        private void PlayerList_DoubleClick(object sender, EventArgs e)
        {
            int listIndex = PlayerList.SelectedIndex;

            PlayerNameBox.Text = playerArray[listIndex].playerName;
            JerseyNumBox.Text = playerArray[listIndex].jerseyNum.ToString();
            GoalsBox.Text = playerArray[listIndex].goals.ToString();
        }
        
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            int listIndex = PlayerList.SelectedIndex;

            playerArray[listIndex].playerName = PlayerNameBox.Text;
            playerArray[listIndex].jerseyNum = Convert.ToInt32(JerseyNumBox.Text);
            playerArray[listIndex].goals = Convert.ToInt32(GoalsBox.Text);

            updateList();
        }
        private void updateList()
        {
            String rowString;
            PlayerList.Items.Clear();

            foreach (Player player in playerArray)
            {
                if (player != null)
                {
                    rowString = stringToRow(player.playerName, player.jerseyNum, player.goals);
                    PlayerList.Items.Add(rowString);
                }
            }
        }
        

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (NameSort.Checked)
            {
                Player.sortID = 'n';
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (JerseySort.Checked)
            {
                Player.sortID = 'j';
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (GoalsSort.Checked)
            {
                Player.sortID = 'g';
            }
        }

        private void SortButton_Click(object sender, EventArgs e)
        {
            counter = 0;
            if (!ReverseSort.Checked)
            {
                Array.Sort(playerArray);
                for (int i = 0; i < playerArray.Length; i++)
                {
                    if (playerArray[i] != null)
                    {
                        Player temp = playerArray[i];
                        playerArray[counter] = temp;
                        playerArray[i] = null;
                        counter++;
                    }
                }
            }
            else
            {
                Array.Sort(playerArray);
                Array.Reverse(playerArray);
            }

            updateList();
        }
        
        private void Save_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            string fileName = saveFileDialog1.FileName;
            FileStream outFile = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryFormatter bFormatter = new BinaryFormatter();

            foreach (Player player in playerArray)
            {
                if (player != null)
                {
                    bFormatter.Serialize(outFile, player);
                }
            }

            outFile.Close();
        }

        private void Load_Click(object sender, EventArgs e)
        {
            counter = 0;
            openFileDialog1.ShowDialog();
            string fileName = openFileDialog1.FileName;
            FileStream inFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryFormatter bFormatter = new BinaryFormatter();
            for (int i = 0; i < playerArray.Length; i++)    //clears array when loading
            {
                playerArray[i] = null;
            }

            while (inFile.Position < inFile.Length)        //fills array with values from file
            {
                playerArray[counter] = (Player)bFormatter.Deserialize(inFile);
                counter++;
            }

            inFile.Close();
            updateList();
        }
        
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
