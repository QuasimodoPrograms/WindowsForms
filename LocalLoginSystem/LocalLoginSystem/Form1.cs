using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LocalLoginSystem
{
    /// <summary>
    /// Subscribe to the YouTube channel: https://www.youtube.com/c/QuasimodoPrograms?sub_confirmation=1
    /// </summary>
    public partial class Form1 : Form
    {
        #region Private members

        /// <summary>
        /// Hard-coded usernames. Not recommended to use.
        /// </summary>
        private readonly string[] mUsernames = { "username1", "username2", "username3" };

        /// <summary>
        /// Hard-coded passwords. Not recommended to use.
        /// </summary>
        private readonly string[] mPasswords = { "password1", "password2", "password1" };

        /// <summary>
        /// A list of usernames filled from outer resources such as text files and databases
        /// </summary>
        private readonly List<string> mUserList = new List<string>();

        /// <summary>
        /// A list of passwords filled from outer resources such as text files and databases
        /// </summary>
        private readonly List<string> mPasswordList = new List<string>();

        #endregion

        #region Сonstructor

        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Load usernames and passwords on startup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Find a text file with usernames and passwords
            StreamReader sr = new StreamReader("vars.txt");

            // Create an empty string which will be filled with a text file line
            string line;

            // If the text file still has lines...
            while ((line = sr.ReadLine()) != null)
            {
                // split the line with a whitespace
                string[] components = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                // add the first part of the line to the list of usernames
                mUserList.Add(components[0]);

                // add the second part of the line to the list of passwords
                mPasswordList.Add(components[1]);
            }

            // Close the StreamReader
            sr.Close();

            // If the program is told to load usernames and passwords from the database...
            if (MessageBox.Show("Do you want to load usernames and passwords from the database?", "Database", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Create a new connection to a database
                OleDbConnection connect = new OleDbConnection
                {
                    ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=myDB.mdb;Persist Security Info=False;"
                };

                try
                {
                    // Try to open the database file with this provider
                    connect.Open();
                }
                // If there is an error...
                catch (Exception)
                {
                    // try another database provider
                    connect.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=myDB.mdb;Persist Security Info=False;";

                    // Open the database file with this provider (there will be an exception if this provider is not istalled either)
                    connect.Open();
                }

                // Create a new command to select users and passwords from a database table
                OleDbCommand command = new OleDbCommand
                {
                    Connection = connect,
                    CommandText = "SELECT user, pass FROM tbl1"
                };

                // Execute the command
                OleDbDataReader reader = command.ExecuteReader();

                // If there are more rows... 
                while (reader.Read())
                {
                    // add the first cell of the row to the list of usernames
                    mUserList.Add((string)reader[0]);

                    // add the second cell of the row to the list of passwords
                    mPasswordList.Add((string)reader[1]);
                }

                // Close the connection
                connect.Close();
            }
        }

        /// <summary>
        /// Click the Login button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // If there are such a hard-coded username and such a password in respective arrays, and the password of the username matches the entered passsword (thanks to fox mulder for finding a mistake)...
            if (mUsernames.Contains(textBox1.Text) && mPasswords.Contains(textBox2.Text) && mPasswords[Array.IndexOf(mUsernames, textBox1.Text)] == textBox2.Text)
            {
                // Create the second form
                Form2 f2 = new Form2();

                // Show the second form
                f2.ShowDialog();
            }
            // If the list of usernames contains the specified username and the list of passwords, and the password of the username matches the entered passsword (thanks to fox mulder for finding a mistake)...
            else if (mUserList.Contains(textBox1.Text) && mPasswordList.Contains(textBox2.Text) && mPasswordList[mUserList.IndexOf(textBox1.Text)] == textBox2.Text)
            {
                // Create the second form
                Form2 f2 = new Form2();

                // Show the second form
                f2.ShowDialog();
            }
            else
                // Show the error message
                MessageBox.Show("The username and/or password is incorrect.");
        }
    }
}
