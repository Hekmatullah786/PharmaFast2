using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PharmaFast
{
    public partial class Products : Form
    {
        ConnectionDB con = new ConnectionDB();
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            LoadData();
            adddata();
            dataGridView1.Columns[6].DefaultCellStyle.Format = "0.00";
            dataGridView1.Columns[3].DefaultCellStyle.Format = "0.00";
        }
        private void calc()
        {
            try
            {
                // Read values from TextBoxes
                double num1 = double.Parse(textBox3.Text);
                double num2 = double.Parse(textBox8.Text);

                // Perform the calculation (addition in this case)
                double result = num1 * (1 + num2 / 100);

                // Display the result in the result TextBox
                textBox5.Text = result.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            calc();
        }
        private void LoadData()
        {

            string query = "SELECT * FROM Producttbl";
            DataTable dataTable = new DataTable();
            {
                try
                {

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    con.GetClose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string query = null;

            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                if (textBox1.Text != "" & textBox2.Text != "")
                {
                    cmd.CommandText = "insert into Producttbl (ProductID,ProductName,ProductDetails,Cost,Benifit,ExpiryDate,Price) values (@id, @name, @type, @cost, @benifit, @expdate, @price)";
                    cmd.Parameters.AddWithValue("@id", textBox1.Text);
                    cmd.Parameters.AddWithValue("@name", textBox2.Text);
                    cmd.Parameters.AddWithValue("@type", textBox4.Text);
                    cmd.Parameters.AddWithValue("@cost", textBox3.Text);
                    cmd.Parameters.AddWithValue("@benifit", textBox8.Text);
                    cmd.Parameters.AddWithValue("@expdate", dateTimePicker1.Text);
                    cmd.Parameters.AddWithValue("@price", textBox5.Text);

                    cmd.ExecuteNonQuery();
                    con.GetClose();
                    LoadData();
                    MessageBox.Show("Item Added Successfully");
                }
            }
        }
        public void adddata()
        {
            try
            {
                string query = "SELECT ProductID from Producttbl";

                using (SqlCommand cmd = new SqlCommand(query, con.GetConnection()))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //int value = int.Parse(reader[0].ToString());
                            int result = int.Parse(reader[0].ToString()) + 1;
                            textBox1.Text = result.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                con.GetClose();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            adddata();
            textBox2.Text = "";
            textBox3.Text = "0";
            textBox4.Text = "";
            textBox5.Text = "0";
            textBox8.Text = "0";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                cmd.CommandText = "delete from Producttbl where ProductID=" + textBox1.Text + "";
                cmd.ExecuteNonQuery();
                con.GetClose();
                adddata();
                MessageBox.Show("Successfully Deleted!");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
