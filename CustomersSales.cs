using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
    public partial class CustomersSales : Form
    {
        ConnectionDB con = new ConnectionDB();
        public CustomersSales()
        {
            InitializeComponent();
        }
        private void CustomersSales_Load(object sender, EventArgs e)
        {
            loaddata();
            loaddata2();
            totprofit2();
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
            label13.Text = ($"تاریخ: {solarDay} {afghanMonths[solarMonth - 1]} {solarYear}");
        }
        private void Main2()
        {
            DateTime gregorianDate = dateTimePicker2.Value;
            PersianCalendar solarCalendar = new PersianCalendar();
            int solarDay = solarCalendar.GetDayOfMonth(gregorianDate);
            int solarMonth = solarCalendar.GetMonth(gregorianDate);
            int solarYear = solarCalendar.GetYear(gregorianDate);
            string[] afghanMonths = { "حمل", "ثور", "جوزا", "سرطان", "اسد", "سنبله",
                                  "میزان", "عقرب", "قوس", "جدی", "دلو", "حوت" };
            label13.Text = ($"تاریخ: {solarDay} {afghanMonths[solarMonth - 1]} {solarYear}");
        }
        private void loaddata()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Salestbl where TDate between '" + dateTimePicker1.Text + "' And'" + dateTimePicker2.Text + "'", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                con.GetClose();
                label1.Text = dataGridView2.Rows.Count.ToString();
                total();

            }
            catch
            { }
        }
        private void loaddata2()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Soldtbl where TransactionDate between '" + dateTimePicker1.Text + "' And'" + dateTimePicker2.Text + "'", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.GetClose();
                label12.Text = dataGridView1.Rows.Count.ToString();
                total2();
                totalpaid();
                calc();

            }
            catch
            { }
        }
        private void calc()
        {
            try
            {
                // Read values from TextBoxes
                double num1 = double.Parse(totamount.Text);
                double num2 = double.Parse(totpaid.Text);
                // Perform the calculation (addition in this case)
                double result = num1 - num2;

                // Display the result in the result TextBox
                totpayable.Text = result.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void total()
        {
            int total = dataGridView2.Rows.Cast<DataGridViewRow>()
           .Sum(t => Convert.ToInt32(t.Cells[5].Value));
            totalamount.Text = total.ToString();
        }
        private void totalDisc()
        {
            int total2 = dataGridView2.Rows.Cast<DataGridViewRow>()
           .Sum(t => Convert.ToInt32(t.Cells[6].Value));
            label7.Text = total2.ToString();
        }
        private void total2()
        {
            int total = dataGridView1.Rows.Cast<DataGridViewRow>()
           .Sum(t => Convert.ToInt32(t.Cells[4].Value));
            totamount.Text = total.ToString();
        }
        private void totalpaid()
        {
            int total = dataGridView1.Rows.Cast<DataGridViewRow>()
           .Sum(t => Convert.ToInt32(t.Cells[5].Value));
            totpaid.Text = total.ToString();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Salestbl where CustomerID = '" + textBox1.Text + "'", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                con.GetClose();
                label1.Text = dataGridView2.Rows.Count.ToString();
                total();

            }
            catch
            { }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.Visible = true;
                textBox2.Visible = false;
                checkBox1.Text = "Search By ID";
            }
            else
            {
                textBox1.Visible = false;
                textBox2.Visible = true;
                checkBox1.Text = "Search By Name";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Salestbl where CustomerName like '%" + textBox2.Text + "%'", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                con.GetClose();
                label1.Text = dataGridView2.Rows.Count.ToString();
                total();
                totalDisc();
            }
            catch
            { }
        }

        private void byname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Soldtbl where CustomerName like '%" + byname.Text + "%'", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.GetClose();
                label1.Text = dataGridView1.Rows.Count.ToString();
                total2();
                totalpaid();
                calc();
            }
            catch
            { }
        }

        private void byid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Soldtbl where CustomerID =" + byid.Text + "", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.GetClose();
                label1.Text = dataGridView1.Rows.Count.ToString();
                total2();
                totalpaid();
                calc();
            }
            catch
            { }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox2.Checked == true)
            {
                byid.Visible = true;
                byname.Visible = false;
                checkBox2.Text = "Search By ID";
            }
            else
            {
                byid.Visible = false;
                byname.Visible = true;
                checkBox2.Text = "Search By Name";
            }
        }

        private void printReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (byid == null)
            {
                MessageBox.Show("Please enter the Customer ID");
                byid.Focus();
                byid.BackColor = Color.LightYellow;
            }
            else
            {
                string id = byid.Text;
                SoldPrintReport reportbyBID = new SoldPrintReport(id);
                reportbyBID.Show();
            }
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            loaddata();
            loaddata2();
            totalpaid();
            calc();
            Main();
        }
        
        private void Addnumbers()
        {
            string query = "UPDATE Supplytbl SET OrderedQ=(OrderedQ+" + textBox3.Text + ") where ProductName='" + textBox4.Text + "'";
            DataTable dataTable = new DataTable();
            {
         //     try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                }
           //   catch (Exception ex)
                {
               //   MessageBox.Show("An error occurred: " + ex.Message);
                }
                con.GetClose();
            }
        }
        private void deleteFromSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
                // Assuming you want to get the value from the first cell (column index 0)
                string cellValue = selectedRow.Cells[4].Value.ToString();
                string cellValue1 = selectedRow.Cells[3].Value.ToString();
                // Set the value to the TextBox
                textBox3.Text = cellValue;
                textBox4.Text = cellValue1;
            }
            DialogResult dr = MessageBox.Show("Are you sure to delete from sales table?",
        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                try
                {
                 
                    string query = null;
                    using (var cmd = new SqlCommand(query, con.GetConnection()))
                    {
                        cmd.CommandText = "delete from Salestbl where SalesID=" + dataGridView2.CurrentRow.Cells[0].Value + "";
                        // cmd.CommandText = "delete from Salestbl where SalesID=" + textBox4.Text + "";

                        con.GetConnection();
                        cmd.ExecuteNonQuery();
                        con.GetClose();
                        label9.Text = "Successfully Deleted!";
                        loaddata();
                        loaddata2();
                        totalpaid();
                        calc();
                        Addnumbers();

                    }
                }
                catch (Exception) { MessageBox.Show("Please select a row from datagridview"); }
            }
        }
        private void totprofit2()
        {
            SqlCommand cmd = new SqlCommand();
            SqlCommand cmd1 = new SqlCommand();
            {
                try
                {
                    string query = "SELECT SUM(Profit) FROM Profittbl";
                    string query1 = "SELECT SUM(Discount) FROM Salestbl";
                    cmd = new SqlCommand(query, con.GetConnection());
                    cmd1 = new SqlCommand(query1, con.GetConnection());
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    int count1 = Convert.ToInt32(cmd1.ExecuteScalar());
                    int result = count - count1;
                    totprofit.Text = result.ToString();
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }
        private void deleteFromSoldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete sold items?",
        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string query = null;
                    using (var cmd = new SqlCommand(query, con.GetConnection()))
                    {
                        cmd.CommandText = "delete from Soldtbl where SoldID=" + dataGridView1.CurrentRow.Cells[0].Value + "";

                        con.GetConnection();
                        cmd.ExecuteNonQuery();
                        con.GetClose();
                        label9.Text = "Successfully Deleted!";
                        loaddata();
                        loaddata2();
                        totalpaid();
                        calc();
                    }
                }
                catch (Exception) { MessageBox.Show("Please select a row from datagridview"); }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
                // Assuming you want to get the value from the first cell (column index 0)
                string cellValue = selectedRow.Cells[4].Value.ToString();
                string cellValue1= selectedRow.Cells[3].Value.ToString();
                // Set the value to the TextBox
                textBox3.Text = cellValue;
                textBox4.Text = cellValue1;
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            loaddata();
            loaddata2();
            totalpaid();
            calc();
            Main2();
        }
    }
}
