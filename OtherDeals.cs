using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
    public partial class OtherDeals : Form
    {
        ConnectionDB con = new ConnectionDB();
        public OtherDeals()
        {
            InitializeComponent();
        }

        private void OtherDeals_Load(object sender, EventArgs e)
        {
            LoadData();
            Main();
        }
        private void Main()
        {
            DateTime gregorianDate = dateTimePicker1.Value;
            PersianCalendar solarCalendar = new PersianCalendar();
            int solarDay = solarCalendar.GetDayOfMonth(gregorianDate);
            int solarMonth = solarCalendar.GetMonth(gregorianDate);
            int solarYear = solarCalendar.GetYear(gregorianDate);
            string[] afghanMonths = { "حمل", "ثور", "جوزا", "سرطان", "اسد", "سنبله",
                                  "میزان", "عقرب", "قوس", "جدی", "دلو", "حوت" };
            sdate.Text = ($"تاریخ: {solarDay} {afghanMonths[solarMonth - 1]} {solarYear}");
        }
        private void total()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    string query = "SELECT SUM(Dealamount) FROM Dealstbl";
                    cmd = new SqlCommand(query, con.GetConnection());
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
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

            string query = "SELECT * FROM Dealstbl";
            DataTable dataTable = new DataTable();
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
          .Sum(t => Convert.ToDecimal(t.Cells[4].Value));
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
                cmd.CommandText = "insert into Dealstbl (Dealer,Dealnote,Dealtype,Dealamount,Date) values (@type, @ProductName,@dtype,@amount,@date)";
                cmd.Parameters.AddWithValue("@type", textBox1.Text);
                cmd.Parameters.AddWithValue("@ProductName", textBox3.Text);
                cmd.Parameters.AddWithValue("@amount", textBox2.Text);
                cmd.Parameters.AddWithValue("@dtype", comboBox1.Text);
                cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                con.GetConnection();
                cmd.ExecuteNonQuery();
                con.GetClose();
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                LoadData();
                total();
                label9.Text = "Successfully Added!";
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                cmd.CommandText = "update Dealstbl set Dealer=@type,Dealnote=@dnote, Dealtype= @ProductName,Dealamount=@amount,Date=@date where Dealid=@id";
                cmd.Parameters.AddWithValue("@id", textBox4.Text);
                cmd.Parameters.AddWithValue("@type", textBox1.Text);
                cmd.Parameters.AddWithValue("@ProductName", comboBox1.Text);
                cmd.Parameters.AddWithValue("@dnote", textBox3.Text);
                cmd.Parameters.AddWithValue("@amount", textBox2.Text);
                cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                con.GetConnection();
                cmd.ExecuteNonQuery();
                con.GetClose();
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                LoadData();
                total();
                label9.Text = "Successfully Updated!";
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void button4_Click(object sender, EventArgs e)
        {

            string query = "SELECT * FROM Dealstbl where Date='" + dateTimePicker1.Value + "'";
            DataTable dataTable = new DataTable();
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
          .Sum(t => Convert.ToDecimal(t.Cells[4].Value));
                    label5.Text = total.ToString();
                    con.GetClose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("Are you sure to delete item?",
        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {

                try
                {
                    string query = "delete from Dealstbl  where Dealid=" + dataGridView1.CurrentRow.Cells[0].Value + "";
                    SqlCommand cmd = new SqlCommand(query, con.GetConnection());
                    int x = cmd.ExecuteNonQuery();
                    LoadData();
                    MessageBox.Show("Record Successfully Deleted!");
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from Dealstbl where Dealer LIKE '%' + '" + textBox5.Text + "'+ '%'", con.GetConnection());
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da.SelectCommand = cmd;
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.GetClose();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Please select Type");
                comboBox1.Focus();
            }
            else
            {
                SqlCommand cmd = new SqlCommand("select * from Dealstbl where Dealtype= '" + comboBox1.Text + "'", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.GetClose();
                decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
            .Sum(t => Convert.ToDecimal(t.Cells[4].Value));
                label5.Text = total.ToString();
                con.GetClose();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            dateTimePicker1.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Main();
        }
    }
}