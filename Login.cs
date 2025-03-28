using Microsoft.Win32;
using PharmaFast;
using PharmaFast.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.util;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PharmaFast
{
    public partial class Login : Form
    {
        ConnectionDB con = new ConnectionDB();
        public Login()
        {
            InitializeComponent();
            panel1.BackColor = Color.FromArgb(50, Color.Black);
        }

        public void AddToRegistry(string keyName, string valueName, string valueData)
        {
            // Open or create the registry key
            RegistryKey key = Registry.CurrentUser.CreateSubKey(keyName);

            if (key != null)
            {
                // Set the value
                key.SetValue(valueName, valueData);
                key.Close();
            }
        }
        public string RetrieveFromRegistry(string keyName, string valueName)
        {
            // Open the registry key
            RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName);

            if (key != null)
            {
                // Get the value
                object value = key.GetValue(valueName);
                key.Close();

                // Return the value as a string
                return value?.ToString();
            }

            return null;
        }
        private void Login_Load(object sender, EventArgs e)
        {
            string keyName = "Software\\PharmaFast";
            string valueName = "MySetting";
            string valueData = "saifi786-drhekmat786-4799100-2025";
            string retrievedData = RetrieveFromRegistry(keyName, valueName);
            if (retrievedData == null) { label6.Visible = true; }
            if (retrievedData != RetrieveFromRegistry(keyName, valueName)) { label6.Visible = true; }
            //    else if (retrievedData == RetrieveFromRegistry(keyName, valueName)) { label6 .Visible = false; }
            //  if(label6.Visible=true) {button2.Visible = false; }else{ button2.Visible = true; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string keyName = "Software\\PharmaFast";
            string valueName = "MySetting";
            string valueData = "saifi786-drhekmat786-4799100-2025";
            string user = txtuser.Text;
            string pos = postxt.Text;
            if (txtuser.Text == "" || txtpass.Text == "")
            {
                MessageBox.Show("Please Provide Username And Password", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                SqlCommand cmd = new SqlCommand("select * from usertbl where username = @username and userpass = @password", con.GetConnection());
                cmd.Parameters.AddWithValue("@username", txtuser.Text);
                cmd.Parameters.AddWithValue("@password", txtpass.Text);
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                System.Data.DataSet ds = new System.Data.DataSet();
                adapt.Fill(ds);
                int count = ds.Tables[0].Rows.Count;
                if (count == 1)
                {
                    string retrievedData = RetrieveFromRegistry(keyName, valueName);
                    if (retrievedData == null) { MessageBox.Show("You need to purchase a key!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information); this.Close(); }
                    {
                        if (retrievedData == RetrieveFromRegistry(keyName, valueName))
                        {

                            this.Hide();
                            HomePage f1 = new HomePage(user, pos);
                            f1.Show();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Login Failed, Please enter the correct username or password!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtuser.Focus();
                }
                con.GetClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        static bool toggle = true;
        private void button3_Click(object sender, EventArgs e)
        {

            {
                if (toggle)
                {
                    txtpass.PasswordChar = '\0';
                }
                else
                {
                    txtpass.PasswordChar = '*';
                }
                toggle = !toggle;
            }
        }

        private void txtpass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
                e.Handled = e.SuppressKeyPress = true;
            }
        }
        private void txtuser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.SuppressKeyPress = (e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
        private void txtuser_Leave(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from usertbl where username = @username", con.GetConnection());
                cmd.Parameters.AddWithValue("@username", txtuser.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                postxt.Text = reader["userpos"].ToString();
            }
            catch (Exception)
            { }
        }
    }
}