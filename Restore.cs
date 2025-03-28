using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmaFast
{
    public partial class Restore : Form
    {
        ConnectionDB con = new ConnectionDB();
        public Restore()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete profit items?",
        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = "DELETE FROM Profittbl";
                    cmd.Connection = con.GetConnection();
                    int numberDeleted = cmd.ExecuteNonQuery();
                    MessageBox.Show("You have deleted " + numberDeleted + " items");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("Are you sure to delete customers?",
        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = "DELETE FROM Customertbl";
                    cmd.Connection = con.GetConnection();
                    int numberDeleted = cmd.ExecuteNonQuery();
                    MessageBox.Show("You have deleted " + numberDeleted + " customers");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete Sales?",
        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = "DELETE FROM Salestbl";
                    cmd.Connection = con.GetConnection();
                    int numberDeleted = cmd.ExecuteNonQuery();
                    MessageBox.Show("You have deleted " + numberDeleted + " Sales");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete Sold items?",
      "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = "DELETE FROM Soldtbl";
                    cmd.Connection = con.GetConnection();
                    int numberDeleted = cmd.ExecuteNonQuery();
                    MessageBox.Show("You have deleted " + numberDeleted + " Sold items");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete deals items?",
            "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = "DELETE FROM Dealstbl";
                    cmd.Connection = con.GetConnection();
                    int numberDeleted = cmd.ExecuteNonQuery();
                    MessageBox.Show("You have deleted " + numberDeleted + " items");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete expenses?",
                        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = "DELETE FROM Expensestbl";
                    cmd.Connection = con.GetConnection();
                    int numberDeleted = cmd.ExecuteNonQuery();
                    MessageBox.Show("You have deleted " + numberDeleted + " items");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete Products?",
                        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = "DELETE FROM Producttbl";
                    cmd.Connection = con.GetConnection();
                    int numberDeleted = cmd.ExecuteNonQuery();
                    MessageBox.Show("You have deleted " + numberDeleted + " items");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete supplied items?",
                        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = "DELETE FROM Supplytbl";
                    cmd.Connection = con.GetConnection();
                    int numberDeleted = cmd.ExecuteNonQuery();
                    MessageBox.Show("You have deleted " + numberDeleted + " items");
                }

            }
        }
    }
}
