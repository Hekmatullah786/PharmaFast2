using Microsoft.VisualBasic;
using PharmaFast;
using PharmaFast.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PharmaFast
{
    public partial class HospitalSetting : Form
    {
        ConnectionDB con = new ConnectionDB();
        public HospitalSetting()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            Settings.Default["name"] = apptitle.Text;
            Settings.Default["info"] = info.Text;
            Settings.Default["dashboard"] = dashboard.Text;
            Settings.Default["logo"] = imageloc.Text;
            Settings.Default["theme"] = comboBox1.Text;
            Settings.Default["labelshow"] = comboBox4.Text;
            Settings.Default.Save();
            MessageBox.Show("Please restart the Application,\nOr go to home page and refresh the screen for applying the changes", "Setting Saved");
        }

        private void HospitalSetting_Load(object sender, EventArgs e)
        {

            apptitle.Text = Settings.Default["name"].ToString();
            info.Text = Settings.Default["info"].ToString();
            imageloc.Text = Settings.Default["logo"].ToString();
            dashboard.Text = Settings.Default["dashboard"].ToString();
            comboBox1.Text = Settings.Default["theme"].ToString();
            comboBox4.Text = Settings.Default["labelshow"].ToString();
            string filePath = imageloc.Text;
            Image image = Image.FromFile(filePath);
            pictureBox1.Image = image;
            this.Width = 614;
            dispUser();
            dispUsers();
        }
        public void dispUser()
        {
            string sql = "SELECT * FROM usertbl";
            SqlCommand cmd = new SqlCommand(sql, con.GetConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable Tbl = new DataTable();
            adapter.Fill(Tbl);
            comboBox3.DataSource = Tbl;
            comboBox3.DisplayMember = "username";
            comboBox3.ValueMember = "userid";
            con.GetClose();
        }
        public void dispUsers()
        {
            string sql = "SELECT * FROM usertbl";
            SqlCommand cmd = new SqlCommand(sql, con.GetConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable Tbl = new DataTable();
            adapter.Fill(Tbl);
            dataGridView1.DataSource = Tbl;
            con.GetClose();
            dataGridView1.Columns[2].Visible = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void imageloc_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image|*.png;*.jpg;*.bmp;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(@openFileDialog.FileName);
                    string sFileName = openFileDialog.FileName;
                    //string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                    // if (directoryPath != null)
                    {
                        imageloc.Text = sFileName;
                    }
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label7.Visible = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            label7.Visible = false;

        }
        private void button2_Click_2(object sender, EventArgs e)
        {
            string query = "Insert INTO usertbl (userid,username,userpass,userpos) VALUES (@userid,@username,@userpass,@userpos)";
            using (SqlCommand cmd = new SqlCommand(query, con.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@userid", textBox4.Text);
                cmd.Parameters.AddWithValue("@username", comboBox3.Text);
                cmd.Parameters.AddWithValue("@userpass", txtpass.Text);
                cmd.Parameters.AddWithValue("@userpos", comboBox2.Text);
                cmd.ExecuteNonQuery();
                con.GetClose();
            }
            MessageBox.Show("User Registered Successfully");
            dispUsers();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int aid = int.Parse(textBox4.Text);
                string query = "delete from usertbl where userid=@userid";
                using (SqlCommand cmd = new SqlCommand(query, con.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@userid", aid);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Deleted Successfully");
                }
            }
            catch (Exception)
            {
                dispUsers();
            }
        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int nid = int.Parse(textBox4.Text);
                string nurname = comboBox3.Text;
                string Age = txtpass.Text;
                string Gender = comboBox2.Text;
                string query = "update usertbl set username=@nurname, userpass=@Age, userpos=@Gender where userid=@nid";
                SqlCommand cmd = new SqlCommand(query, con.GetConnection());
                cmd.Parameters.AddWithValue("@nid", nid);
                cmd.Parameters.AddWithValue("@nurname", nurname);
                cmd.Parameters.AddWithValue("@Age", Age);
                cmd.Parameters.AddWithValue("@Gender", Gender);
                int x = cmd.ExecuteNonQuery();
                MessageBox.Show("User successfully updated!");
            }
            catch (Exception)
            {
            }
            {
                dispUsers();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Please enter the password to continue...", "Enter Password", "", -1, -1);
            if (input == "Admin") {
                this.Width = 1103;
        }
            else { this.Width = 617;MessageBox.Show("Wrong Password, Retry!"); }
                }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                string sql = "SELECT * FROM usertbl where username='" + comboBox3.Text + "'";
                SqlCommand cmd = new SqlCommand(sql, con.GetConnection());
                SqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    textBox4.Text = r[0].ToString();
                    txtpass.Text = r[2].ToString();
                    comboBox2.Text = r[3].ToString();
                    r.Close();
                    con.GetClose();
                }
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {
            comboBox3.Text = "";
            comboBox2.Text = "";
            txtpass.Text = "";
            textBox4.Text = "";
        }
        private bool toggle = true;
        private void button6_Click(object sender, EventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Restore restore = new Restore();
            restore.ShowDialog();
        }
    }
}