using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
    public partial class OrderSupply : Form
    {
        ConnectionDB con = new ConnectionDB();
        public OrderSupply()
        {
            InitializeComponent();
        }
        private void OrderSupply_Load(object sender, EventArgs e)
        {
            Loadsupplier();
            LoadData();
            LoadData2();
            LoadData3();
            adddata();
            dataGridView1.Columns["ExpiryDate"].Visible = false;
            dataGridView1.Columns["ProductID"].Visible = false;
            dataGridView1.Columns["Benifit"].Visible = false;
            textBox6.Text = "Enter Item Name To Search...";
            comboBox2.Text = "Recieved";
            comboBox1.Select();
            dataGridView1.Visible = false;
        }
        private void Loadsupplier()
        {
            string sql = "SELECT * FROM Supplytbl";
            SqlCommand cmd = new SqlCommand(sql, con.GetConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable Tbl = new DataTable();
            adapter.Fill(Tbl);
            comboBox1.DataSource = Tbl;
            comboBox1.DisplayMember = "SupplierName";
            comboBox1.ValueMember = "SupplyID";
            con.GetClose();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }
        public void adddata()
        {
            try
            {
                string query = "SELECT SupplyID from Supplytbl";
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                con.GetClose();
            }
        }
        private void LoadData2()
        {
            string query = "SELECT * FROM Supplytbl";
            DataTable dataTable = new DataTable();
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView2.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                con.GetClose();
            }
        }
        private void LoadData3()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    string query = "SELECT SUM(Cost*OrderedQ) FROM Supplytbl";
                    cmd = new SqlCommand(query, con.GetConnection());
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    TotalSum.Text = count.ToString();
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from Producttbl where ProductName LIKE '%' + '" + textBox6.Text + "'+ '%'", con.GetConnection());
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da.SelectCommand = cmd;
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.GetClose();
            dataGridView1.Visible = true;
        }
        private void textBox6_Enter(object sender, EventArgs e)
        {
            textBox6.Text = string.Empty;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                if (textBox1.Text != "" & textBox2.Text != "")
                {
                    decimal cost = decimal.Parse(textBox3.Text);
                    decimal cost1 = decimal.Parse(textBox8.Text);
                    decimal cost2 = decimal.Parse(textBox2.Text);
                    decimal cost3 = decimal.Parse(textBox4.Text);
                    decimal cost4 = decimal.Parse(textBox5.Text);
                    cmd.CommandText = "insert into Supplytbl (SupplyID,SupplierName, ProductName,Company,Cost,Benifit,Price,TPrice,OrderedQ,ExpDate,OrderStat,Payment,Barcode) values (@id, @suppliername, @name, @company, @cost, @benifit,@price,@Tprice,@OrderQ, @ExpDate, @orderstat, @payment,@barcode)";
                    cmd.Parameters.AddWithValue("@id", textBox1.Text);
                    cmd.Parameters.AddWithValue("@suppliername", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@name", textBox7.Text);
                    cmd.Parameters.AddWithValue("@company", textBox10.Text);
                    cmd.Parameters.AddWithValue("@cost", cost);
                    cmd.Parameters.AddWithValue("@benifit", cost1);
                    cmd.Parameters.AddWithValue("@price", cost3);
                    cmd.Parameters.AddWithValue("@OrderQ", cost2);
                    cmd.Parameters.AddWithValue("@Tprice", cost4);
                    cmd.Parameters.AddWithValue("@ExpDate", dateTimePicker1.Text);
                    cmd.Parameters.AddWithValue("@orderstat", comboBox2.Text);
                    cmd.Parameters.AddWithValue("@payment", textBox9.Text);
                    cmd.Parameters.AddWithValue("@barcode", textBox12.Text);
                    cmd.ExecuteNonQuery();
                    con.GetClose();
                    LoadData2();
                    adddata();
                    MessageBox.Show("Item Added Successfully");
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox7.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox10.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBox8.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            try
            {
                double num1 = double.Parse(textBox3.Text);
                double num2 = double.Parse(textBox8.Text);
                double result = num1 * (1 + num2 / 100);
                textBox4.Text = result.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dataGridView1.Visible = false;
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            comboBox1.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox7.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox10.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBox3.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
            textBox8.Text = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();
            textBox4.Text = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();
            textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString();
            textBox2.Text = dataGridView2.Rows[e.RowIndex].Cells[8].Value.ToString();
            dateTimePicker1.Text = dataGridView2.Rows[e.RowIndex].Cells[9].Value.ToString();
            comboBox2.Text = dataGridView2.Rows[e.RowIndex].Cells[10].Value.ToString();
            textBox9.Text = dataGridView2.Rows[e.RowIndex].Cells[11].Value.ToString();
            textBox12.Text = dataGridView2.Rows[e.RowIndex].Cells[12].Value.ToString();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox7.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox10.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBox8.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            try
            {
                double num1 = double.Parse(textBox3.Text);
                double num2 = double.Parse(textBox8.Text);
                double result = num1 * (1 + num2 / 100);
                textBox4.Text = result.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dataGridView1.Visible = false;
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from Supplytbl where ProductName LIKE '%' + '" + textBox11.Text + "'+ '%'", con.GetConnection());
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da.SelectCommand = cmd;
            dt.Clear();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            con.GetClose();
        }
        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                cmd.CommandText = "delete from Supplytbl where SupplyID=" + textBox1.Text + "";
                cmd.ExecuteNonQuery();
                con.GetClose();
                MessageBox.Show("Successfully Deleted!");
                LoadData();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            adddata();
            comboBox1.Text = "";
            textBox7.Text = "";
            textBox10.Text = "";
            textBox3.Text = "0";
            textBox8.Text = "0";
            textBox2.Text = "0";
            textBox12.Text = "";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                if (textBox1.Text != "" & textBox2.Text != "")
                {
                    decimal cost = decimal.Parse(textBox3.Text);
                    decimal cost1 = decimal.Parse(textBox8.Text);
                    decimal cost2 = decimal.Parse(textBox2.Text);
                    decimal cost3 = decimal.Parse(textBox4.Text);
                    decimal cost4 = decimal.Parse(textBox5.Text);
                    cmd.CommandText = "update Supplytbl set SupplierName=@suppliername, ProductName= @name,Company=@company,Cost= @cost,Benifit=@benifit,Price=@price,TPrice=@Tprice,OrderedQ=@OrderQ,ExpDate=@ExpDate,OrderStat=@orderstat,Payment=@payment,Barcode=@barcode where SupplyID=@id";
                    cmd.Parameters.AddWithValue("@id", textBox1.Text);
                    cmd.Parameters.AddWithValue("@suppliername", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@name", textBox7.Text);
                    cmd.Parameters.AddWithValue("@company", textBox10.Text);
                    cmd.Parameters.AddWithValue("@cost", cost);
                    cmd.Parameters.AddWithValue("@benifit", cost1);
                    cmd.Parameters.AddWithValue("@price", cost3);
                    cmd.Parameters.AddWithValue("@OrderQ", cost2);
                    cmd.Parameters.AddWithValue("@Tprice", cost4);
                    cmd.Parameters.AddWithValue("@ExpDate", dateTimePicker1.Text);
                    cmd.Parameters.AddWithValue("@orderstat", comboBox2.Text);
                    cmd.Parameters.AddWithValue("@payment", textBox9.Text);
                    cmd.Parameters.AddWithValue("@barcode", textBox12.Text);
                    cmd.ExecuteNonQuery();
                    con.GetClose();
                    LoadData2();
                    adddata();
                    MessageBox.Show("Item Updated Successfully");
                }
            }
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            double num1 = double.Parse(textBox3.Text);
            double num2 = double.Parse(textBox8.Text);
            double result = num1 * (1 + num2 / 100);
            textBox4.Text = result.ToString();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double num1 = double.Parse(textBox4.Text);
                double num2 = double.Parse(textBox2.Text);
                double result = num1 * num2;
                textBox5.Text = result.ToString();
                textBox9.Text = textBox5.Text;
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Products products = new Products();
            products.Show();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            double num1 = double.Parse(textBox3.Text);
            double num2 = double.Parse(textBox8.Text);
            double result = num1 * (1 + num2 / 100);
            textBox4.Text = result.ToString();
        }
    }
}
