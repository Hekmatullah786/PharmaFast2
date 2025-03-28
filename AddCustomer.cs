using Microsoft.ReportingServices.Diagnostics.Internal;
using System;
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

namespace PharmaFast
{
    public partial class AddCustomer : Form
    {

        ConnectionDB con = new ConnectionDB();
        public AddCustomer()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                cmd.CommandText = "insert into Customertbl (CustomerName,Address,Contact) values (@type, @ProductName,@tdate)";
                cmd.Parameters.AddWithValue("@type", textBox1.Text);
                cmd.Parameters.AddWithValue("@ProductName", textBox2.Text);
                cmd.Parameters.AddWithValue("@tdate", textBox3.Text);
                con.GetConnection();
                cmd.ExecuteNonQuery();
                con.GetClose();
                adddata();
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                MessageBox.Show("Successfully Added!");
            }
        }
        private void AddCustomer_Load(object sender, EventArgs e)
        {
            adddata();
        }
        public void adddata()
        {
            try
            {
                string query = "SELECT CustomerID from Customertbl";
                using (SqlCommand cmd = new SqlCommand(query, con.GetConnection()))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //int value = int.Parse(reader[0].ToString());
                            int result = int.Parse(reader[0].ToString()) + 1;
                            textBox4.Text = result.ToString();
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
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                cmd.CommandText = "delete from Customertbl where CustomerID=" + textBox4.Text + "";

                cmd.ExecuteNonQuery();
                con.GetClose();
                adddata();
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                MessageBox.Show("Successfully Deleted!");
            }
        }
    }
}