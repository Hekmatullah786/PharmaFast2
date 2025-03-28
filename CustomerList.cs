using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PharmaFast
{
    public partial class CustomerList : Form
    {
        ConnectionDB con = new ConnectionDB();
        public CustomerList()
        {
            InitializeComponent();
        }

        private void byid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Customertbl where CustomerID =" + byid.Text + "", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.GetClose();
            }
            catch
            { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from Customertbl where CustomerName LIKE '%' + '" + textBox1.Text + "'+ '%'", con.GetConnection());
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da.SelectCommand = cmd;
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.GetClose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                SqlCommand cmd = new SqlCommand("select * from Customertbl", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.GetClose();
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select * from Customertbl", con.GetConnection());
                    SqlDataAdapter da = new SqlDataAdapter();
                    DataTable dt = new DataTable();
                    da.SelectCommand = cmd;
                    dt.Clear();
                    dataGridView1.DataSource = dt;
                    con.GetClose();
                }
                catch
                { }
            }
        }

        private void topcustomer()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    string query = "SELECT TOP 1 CustomerID, SUM(Price) AS TotalPrice FROM Salestbl GROUP BY CustomerID ORDER BY TotalPrice DESC";

                    con.GetConnection();
                    cmd = new SqlCommand(query, con.GetConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string itemName = reader["CustomerID"].ToString();
                        double totalSold = Convert.ToDouble(reader["TotalPrice"]);
                        TopCustomer.Text = itemName;
                        Amount.Text = Convert.ToString(totalSold);
                        con.GetClose();
                    }
                    reader.Close();
                }
                catch (Exception)
                { }
            }
        }
        private void addCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCustomer addCustomer = new AddCustomer();
            addCustomer.ShowDialog();
        }

        private void CustomerList_Load(object sender, EventArgs e)
        {
            topcustomer();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                SqlCommand cmd = new SqlCommand("SELECT TOP 10 CustomerID, SUM(Price) AS TotalPrice FROM Salestbl GROUP BY CustomerID ORDER BY TotalPrice DESC", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.GetClose();
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select * from Customertbl", con.GetConnection());
                    SqlDataAdapter da = new SqlDataAdapter();
                    DataTable dt = new DataTable();
                    da.SelectCommand = cmd;
                    dt.Clear();
                    dataGridView1.DataSource = dt;
                    con.GetClose();
                }
                catch
                { }
            }
        }
    }
}