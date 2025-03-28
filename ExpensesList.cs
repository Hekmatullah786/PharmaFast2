using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PharmaFast
{
    public partial class ExpensesList : Form
    {
        ConnectionDB con = new ConnectionDB();
        public ExpensesList()
        {
            InitializeComponent();
        }

        private void ExpensesList_Load(object sender, EventArgs e)
        {
            LoadData();
            total();
        }
        private void Main1()
        {
            DateTime gregorianDate = dateTimePicker2.Value;
            PersianCalendar solarCalendar = new PersianCalendar();
            int solarDay = solarCalendar.GetDayOfMonth(gregorianDate);
            int solarMonth = solarCalendar.GetMonth(gregorianDate);
            int solarYear = solarCalendar.GetYear(gregorianDate);
            string[] afghanMonths = { "حمل", "ثور", "جوزا", "سرطان", "اسد", "سنبله",
                                  "میزان", "عقرب", "قوس", "جدی", "دلو", "حوت" };
            label9.Text = ($"تاریخ: {solarDay} {afghanMonths[solarMonth - 1]} {solarYear}");
        }
        private void Main2()
        {
            DateTime gregorianDate = dateTimePicker3.Value;
            PersianCalendar solarCalendar = new PersianCalendar();
            int solarDay = solarCalendar.GetDayOfMonth(gregorianDate);
            int solarMonth = solarCalendar.GetMonth(gregorianDate);
            int solarYear = solarCalendar.GetYear(gregorianDate);
            string[] afghanMonths = { "حمل", "ثور", "جوزا", "سرطان", "اسد", "سنبله",
                                  "میزان", "عقرب", "قوس", "جدی", "دلو", "حوت" };
            label10.Text = ($"تاریخ: {solarDay} {afghanMonths[solarMonth - 1]} {solarYear}");
        }
        private void total()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    string query = "SELECT SUM(Amountpaid) FROM Expensestbl where datepaid='" + dateTimePicker1.Value + "'";
                    cmd = new SqlCommand(query, con.GetConnection());
                    decimal count = Convert.ToDecimal(cmd.ExecuteScalar());
                    label5.Text = count.ToString();
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }
        private void LoadData()
        {

            string query = "SELECT * FROM Expensestbl where datepaid='" + dateTimePicker1.Text + "'";
            DataTable dataTable = new DataTable();
            {
                try
                {

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
          .Sum(t => Convert.ToDecimal(t.Cells[2].Value));
                    label5.Text = total.ToString();
                    con.GetClose();
                    Main1();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void LoadData6()
        {
            string query = "SELECT * FROM Expensestbl";
            DataTable dataTable = new DataTable();
            {
                try
                {

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
          .Sum(t => Convert.ToDecimal(t.Cells[2].Value));
                    label5.Text = total.ToString();
                    con.GetClose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void LoadData1()
        {

            string query = "SELECT * FROM Expensestbl";
            DataTable dataTable = new DataTable();
            {
                try
                {

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
          .Sum(t => Convert.ToDecimal(t.Cells[2].Value));
                    label5.Text = total.ToString();
                    con.GetClose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                decimal amountp = Convert.ToDecimal(textBox2.Text);

                cmd.CommandText = "insert into Expensestbl (reason,Amountpaid,datepaid) values (@type, @ProductName,@tdate)";
                cmd.Parameters.AddWithValue("@type", textBox1.Text);
                cmd.Parameters.AddWithValue("@ProductName", amountp);
                cmd.Parameters.AddWithValue("@tdate", dateTimePicker1.Value);
                con.GetConnection();
                cmd.ExecuteNonQuery();
                con.GetClose();
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                LoadData();
                total();
                label6.Text = "Successfully Added!";
            }
        }
        private void AddCustomer_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) { LoadData1(); label7.Visible = true; label8.Visible = true; dateTimePicker2.Visible = true; dateTimePicker3.Visible = true; panel1.Visible = true; } else { LoadData(); label7.Visible = false; label8.Visible = false; dateTimePicker2.Visible = false; dateTimePicker3.Visible = false; panel1.Visible = false; }
        }
        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Expensestbl where datepaid between '" + dateTimePicker2.Text + "' AND '" + dateTimePicker3.Text + "'";
            DataTable dataTable = new DataTable();
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
          .Sum(t => Convert.ToDecimal(t.Cells[2].Value));
                    label5.Text = total.ToString();
                    con.GetClose();
                    Main1();
                    Main2();
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
                    decimal cost = decimal.Parse(textBox2.Text);
                    int Id = int.Parse(textBox3.Text);

                    cmd.CommandText = "update Expensestbl set reason=@Reas, Amountpaid= @amount,datepaid=@pdate where expid=@id";
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.Parameters.AddWithValue("@Reas", textBox1.Text);
                    cmd.Parameters.AddWithValue("@amount", cost);
                    cmd.Parameters.AddWithValue("@pdate", dateTimePicker1.Text);
                    cmd.ExecuteNonQuery();
                    con.GetClose();
                    LoadData();
                    MessageBox.Show("Item Updated Successfully");
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                cmd.CommandText = "delete from Expensestbl where expid=" + dataGridView1.CurrentRow.Cells[0].Value + "";

                con.GetConnection();
                cmd.ExecuteNonQuery();
                con.GetClose();
                LoadData();
                total();
                label6.Text = "Successfully Deleted!";
            }
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Expensestbl where datepaid between '" + dateTimePicker2.Text + "' AND '" + dateTimePicker3.Text + "'";
            DataTable dataTable = new DataTable();
            {
                try
                {

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
          .Sum(t => Convert.ToDecimal(t.Cells[2].Value));
                    label5.Text = total.ToString();
                    con.GetClose();
                    Main1();
                    Main2();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            dateTimePicker1.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }
    }
}