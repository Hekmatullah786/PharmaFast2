using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PharmaFast
{
    public partial class SalesForm : Form
    {
        public string ReceivedValue { get; set; }
        public string ReceivedValue2 { get; set; }
        ConnectionDB con = new ConnectionDB();
        public SalesForm(string ad)
        {
            InitializeComponent();
            postxtt.Text = ad;
        }
        private void calc()
        {
            try
            {
                double num1 = double.Parse(price1.Text);
                double num2 = double.Parse(qty.Text);
                double num3 = double.Parse(dc.Text);
                double result = num1 * num2 - num3;
                tprice.Text = result.ToString();
                double number = Convert.ToDouble(tprice.Text);
                double percentWanted = Convert.ToDouble(Profit.Text);
                double divideBy = 100 / percentWanted;
                double percentOfNumberAsNumber = number / divideBy;
                PercentofNo.Text = percentOfNumberAsNumber.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SalesForm_Load(object sender, EventArgs e)
        {
            LoadData2();
            adddata();
            TodaysSale();
            lowQ();
            Main();
            KeyPreview = true;
            comboBox1.Text = "Paid";
            if (Int32.Parse(lowitemsQ.Text) >= 1)
            {
                LowQItems lowQItems = new LowQItems();
                lowQItems.ShowDialog();
            }
            if (postxtt.Text == "Pharmacist") { customersAndSalesToolStripMenuItem.Enabled = false; }
            else
            {
                customersAndSalesToolStripMenuItem.Enabled = true;
            }
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
        public void lowQ()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    string query = "SELECT count(SupplyID) FROM Supplytbl WHERE OrderedQ <=30";
                    cmd = new SqlCommand(query, con.GetConnection());
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    lowitemsQ.Text = count.ToString();
                }
                catch (Exception)
                {
                }
                con.GetClose();
            }
        }
        public void adddata()
        {
            try
            {
                string query = "SELECT SalesID from Salestbl";
                using (SqlCommand cmd = new SqlCommand(query, con.GetConnection()))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //int value = int.Parse(reader[0].ToString());
                            int result = int.Parse(reader[0].ToString()) + 1;
                            saleID.Text = result.ToString();
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
        private void TodaysSale()
        {
            int total = dataGridView2.Rows.Cast<DataGridViewRow>()
   .Sum(t => Convert.ToInt32(t.Cells[4].Value));
            label14.Text = total.ToString();
        }
        private void LoadData3()
        {
            string query = "SELECT * From Salestbl where CustomerID=" + custid.Text + " AND TDate = '" + dateTimePicker1.Value + "'";
            DataTable dataTable = new DataTable();
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    //dataGridView1.Columns["SalesID"].Visible = false;
                    dataGridView1.Columns["CustomerName"].Visible = false;
                    dataGridView1.Columns["CustomerID"].Visible = false;
                    dataGridView1.Columns["TDate"].Visible = false;
                    dataGridView1.Columns["CustomerID"].Visible = false;
                    int i2 = 1;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.Cells["Num"].Value = i2;
                        i2++;
                    }
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
           .Sum(t => Convert.ToDecimal(t.Cells[7].Value));
                    TotAmount.Text = total.ToString();
                    label11.Text = dataGridView1.Rows.Count.ToString();
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
            string query = "SELECT * From Soldtbl where TransactionDate='" + dateTimePicker1.Value + "'";
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
        private void SubtractData()
        {
            string query = "UPDATE Supplytbl SET OrderedQ=(OrderedQ-" + qty.Text + ") where SupplyID=" + textBox11.Text + "";
            DataTable dataTable = new DataTable();
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                con.GetClose();
            }
        }
        private void LoadCust()
        {
            string sql = "SELECT * FROM Customertbl ORDER BY CustomerID DESC";
            SqlCommand cmd = new SqlCommand(sql, con.GetConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable Tbl = new DataTable();
            adapter.Fill(Tbl);
            custid.DataSource = Tbl;
            custid.DisplayMember = "CustomerID";
            custid.ValueMember = "CustomerID"; ;
            con.GetClose();
        }
        private void fetchname()
        {
            try
            {
                string query = "SELECT * FROM Customertbl where CustomerID=" + custid.SelectedValue.ToString() + "";
                SqlCommand cmd = new SqlCommand(query, con.GetConnection());
                DataTable dt = new DataTable();
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                reader.Read();
                custname.Text = reader[1].ToString();
                con.GetClose();
            }
            catch (Exception) { }
        }
        private void fetchname2()
        {
            try
            {
                string query = "SELECT * FROM Customertbl where CustomerID=" + custid.Text.ToString() + "";
                SqlCommand cmd = new SqlCommand(query, con.GetConnection());
                DataTable dt = new DataTable();
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                reader.Read();
                custname.Text = reader[1].ToString();
                con.GetClose();
            }
            catch (Exception) { }
        }
        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fetchname2();
                e.Handled = e.SuppressKeyPress = true;
            }
            if (e.SuppressKeyPress = (e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }
        private void comboBox2_DropDownClosed(object sender, EventArgs e)
        {
            fetchname();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from Supplytbl where ProductName LIKE '%' + '" + textBox4.Text + "'+ '%' AND OrderStat= 'Recieved'", con.GetConnection());
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da.SelectCommand = cmd;
            dt.Clear();
            da.Fill(dt);
            dataGridView3.DataSource = dt;
            dataGridView3.Width = 664;
            dataGridView3.Height = 177;
            dataGridView3.Columns[0].Visible = false;
            dataGridView3.Columns[1].Visible = false;
            dataGridView3.Columns[4].Visible = false;
            dataGridView3.Columns[5].Visible = false;
            dataGridView3.Columns[7].Visible = false;
            dataGridView3.Columns[11].Visible = false;
            dataGridView3.Columns[12].Visible = false;
            dataGridView3.Columns[10].Visible = false;
            dataGridView3.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView3.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView3.Columns[6].HeaderCell.Value = "Price";
            dataGridView3.Columns[8].HeaderCell.Value = "Stock";
            con.GetClose();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox11.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
            itemname.Text = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
            itemdetails.Text = dataGridView3.Rows[e.RowIndex].Cells[3].Value.ToString();
            expdate.Text = dataGridView3.Rows[e.RowIndex].Cells[9].Value.ToString();
            price1.Text = dataGridView3.Rows[e.RowIndex].Cells[6].Value.ToString();
            Profit.Text = dataGridView3.Rows[e.RowIndex].Cells[5].Value.ToString();
            panel2.BackColor = Color.DarkGoldenrod;

        }
        private void textBox4_Enter(object sender, EventArgs e)
        {
            textBox4.Text = "";
        }

        private void dataGridView3_Leave(object sender, EventArgs e)
        {
            dataGridView3.Width = 0;
            dataGridView3.Height = 0;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            calc();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            calc();
        }
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox11.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
            itemname.Text = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
            itemdetails.Text = dataGridView3.Rows[e.RowIndex].Cells[3].Value.ToString();
            expdate.Text = dataGridView3.Rows[e.RowIndex].Cells[9].Value.ToString();
            price1.Text = dataGridView3.Rows[e.RowIndex].Cells[6].Value.ToString();
            Profit.Text = dataGridView3.Rows[e.RowIndex].Cells[5].Value.ToString();
        }
        private void addToBillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                if (saleID.Text != "" & itemname.Text != "")
                {
                    cmd.CommandText = "insert into Salestbl (SalesID,CustomerID,CustomerName,ProductName,Quantity,Price,Discount,TDate) values (@id, @name, @type, @ProductName, @Quantity, @Price, @Discount, @TDate)";
                    cmd.Parameters.AddWithValue("@id", saleID.Text);
                    cmd.Parameters.AddWithValue("@name", custid.Text);
                    cmd.Parameters.AddWithValue("@type", custname.Text);
                    cmd.Parameters.AddWithValue("@ProductName", itemname.Text);
                    cmd.Parameters.AddWithValue("@Quantity", qty.Text);
                    cmd.Parameters.AddWithValue("@Price", tprice.Text);
                    cmd.Parameters.AddWithValue("@Discount", dc.Text);
                    cmd.Parameters.AddWithValue("@TDate", dateTimePicker1.Text);
                    cmd.ExecuteNonQuery();
                    con.GetClose();
                    LoadData3();
                    adddata();
                }
            }
        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            calc();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string query = null;
            string query1 = null;
            SqlCommand cmd1 = new SqlCommand(query1, con.GetConnection());
            using (var cmd = new SqlCommand(query, con.GetConnection()))

            {
                if (saleID.Text != "" & itemname.Text != "")
                    try
                    {
                        decimal price = decimal.Parse(tprice.Text);
                        cmd.CommandText = "insert into Salestbl (SalesID,CustomerID,CustomerName,ProductName,Quantity,Price,Discount,TDate) values (@id, @name, @type, @ProductName, @Quantity, @Price, @Discount, @TDate)";
                        cmd.Parameters.AddWithValue("@id", saleID.Text);
                        cmd.Parameters.AddWithValue("@name", custid.Text);
                        cmd.Parameters.AddWithValue("@type", custname.Text);
                        cmd.Parameters.AddWithValue("@ProductName", itemname.Text);
                        cmd.Parameters.AddWithValue("@Quantity", qty.Text);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Discount", dc.Text);
                        cmd.Parameters.AddWithValue("@TDate", dateTimePicker1.Text);
                        decimal Percent = decimal.Parse(PercentofNo.Text);
                        decimal price2 = decimal.Parse(tprice.Text);
                        cmd1.CommandText = "insert into Profittbl (ProductID,Price,Profit,Today) values (@PID, @PPrice, @Profit,@Today)";
                        cmd1.Parameters.AddWithValue("@PID", textBox11.Text);
                        cmd1.Parameters.AddWithValue("@PPrice", price2);
                        cmd1.Parameters.AddWithValue("@Profit", Percent);
                        cmd1.Parameters.AddWithValue("@Today", dateTimePicker1.Text);
                        cmd.ExecuteNonQuery();
                        cmd1.ExecuteNonQuery();
                        con.GetClose();
                        LoadData3();
                        adddata();
                        SubtractData();
                        textBox1.Text = TotAmount.Text;
                        panel2.BackColor = Color.ForestGreen;
                    }
                    catch (Exception ex) {}
            }
        }
        private void dataGridView3_MouseLeave(object sender, EventArgs e)
        {
          
        }
        private void insertBillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                if (saleID.Text != "" & itemname.Text != "")
                {
                    cmd.CommandText = "insert into Soldtbl (CustomerID,SaleID,TotalAmount,TransactionDate) values (@name, @type, @ProductName,@tdate)";

                    cmd.Parameters.AddWithValue("@name", custid.Text);
                    cmd.Parameters.AddWithValue("@type", saleID.Text);
                    cmd.Parameters.AddWithValue("@ProductName", TotAmount.Text);
                    cmd.Parameters.AddWithValue("@tdate", dateTimePicker1.Text);
                    cmd.ExecuteNonQuery();
                    con.GetClose();
                    LoadData2();
                    adddata();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                if (saleID.Text != "" & itemname.Text != "")
                {
                    float paid = float.Parse(textBox1.Text);
                    float tot = float.Parse(TotAmount.Text);
                    DateTime dt = DateTime.Parse(dateTimePicker1.Text);
                    cmd.CommandText = "insert into Soldtbl (CustomerID,CustomerName,SaleID,TotalAmount,Paidamount,TransactionDate) values (@ID, @name, @type, @ProductName,@paid,@tdate)";
                    cmd.Parameters.AddWithValue("@ID", custid.Text);
                    cmd.Parameters.AddWithValue("@name", custname.Text);
                    cmd.Parameters.AddWithValue("@type", saleID.Text);
                    cmd.Parameters.AddWithValue("@ProductName", tot);
                    cmd.Parameters.AddWithValue("@paid", paid);
                    cmd.Parameters.AddWithValue("@tdate", dt);
                    cmd.ExecuteNonQuery();
                    con.GetClose();
                    LoadData2();
                    adddata();
                    TodaysSale();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Assuming you want to get the value from the first cell (column index 0)
                string cellValue = selectedRow.Cells[6].Value.ToString();

                // Set the value to the TextBox
                textBox2.Text = cellValue;
                if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
                {
                    DeleteRows();
                }
            }
        }
        private void DeleteRows()
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                //Fetch the CustomerId from the DataGridView row.
                object SaleId = row.Cells["SalesID"].Value;

                int deleted = 0;
                string sql = "DELETE FROM Salestbl WHERE SalesID = @SaleId";
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con.GetConnection()))
                    {
                        cmd.Parameters.AddWithValue("@SaleId", SaleId);
                        deleted = cmd.ExecuteNonQuery();
                        con.GetClose();
                    }
                }

                if (deleted > 0)
                {
                    dataGridView1.Rows.Remove(row);
                    Addnumbers();
                    LoadData3();
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Assuming you want to get the value from the first cell (column index 0)
                string cellValue = selectedRow.Cells[6].Value.ToString();

                // Set the value to the TextBox
                textBox2.Text = cellValue;
            }
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
            {
                DeleteRows();
            }
        }
        private void Addnumbers()
        {
            string query = "UPDATE Supplytbl SET OrderedQ=(OrderedQ+" + textBox2.Text + ") where SupplyID=" + textBox11.Text + "";
            DataTable dataTable = new DataTable();
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                con.GetClose();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            LoadData3();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                DeleteRows();
        }
        private void custid_DropDown(object sender, EventArgs e)
        {
            LoadCust();
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            // this.Close();
            AddCustomer addCustomer = new AddCustomer();
            addCustomer.Show();
            addCustomer.Focus();
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                cmd.CommandText = "insert into Customertbl (CustomerName) values (@type)";
                cmd.Parameters.AddWithValue("@type", custname.Text);
                cmd.ExecuteNonQuery();
                con.GetClose();
                LoadCust();
                MessageBox.Show("Customer Added Successfully!");
            }
        }

        private void multipleFormsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesForm sales = new SalesForm(ad:HomePage.ActiveForm.Text);
            sales.Show();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.Equals("Unpaid"))
            {
                textBox1.Visible = true;
                textBox1.Select();
                textBox1.Text = "0";
            }
            else
                textBox1.Visible = false;
            if (comboBox1.SelectedItem.Equals("Paid"))
            {
                textBox1.Text = TotAmount.Text;
                textBox1.Visible = false;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.Visible = false;
        }



        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                textBox11.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                itemname.Text = dataGridView3.CurrentRow.Cells[2].Value.ToString();
                itemdetails.Text = dataGridView3.CurrentRow.Cells[3].Value.ToString();
                expdate.Text = dataGridView3.CurrentRow.Cells[9].Value.ToString();
                price1.Text = dataGridView3.CurrentRow.Cells[6].Value.ToString();
                Profit.Text = dataGridView3.CurrentRow.Cells[5].Value.ToString();
                panel2.BackColor = Color.DarkGoldenrod;
                dataGridView3.Height = 0;
                dataGridView3.Width = 0;
                qty.Select();
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = custid.Text;
            SalesReport soldPrintReport = new SalesReport(id);
            soldPrintReport.Show();
        }

        private void qty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.SuppressKeyPress = (e.KeyCode == Keys.Enter))
            {
                string query = null;
                string query1 = null;
                SqlCommand cmd1 = new SqlCommand(query1, con.GetConnection());
                using (var cmd = new SqlCommand(query, con.GetConnection()))
                {
                    if (saleID.Text != "" & itemname.Text != "")
                    {
                        decimal price = decimal.Parse(tprice.Text);
                        cmd.CommandText = "insert into Salestbl (SalesID,CustomerID,CustomerName,ProductName,Quantity,Price,Discount,TDate) values (@id, @name, @type, @ProductName, @Quantity, @Price, @Discount, @TDate)";
                        cmd.Parameters.AddWithValue("@id", saleID.Text);
                        cmd.Parameters.AddWithValue("@name", custid.Text);
                        cmd.Parameters.AddWithValue("@type", custname.Text);
                        cmd.Parameters.AddWithValue("@ProductName", itemname.Text);
                        cmd.Parameters.AddWithValue("@Quantity", qty.Text);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Discount", dc.Text);
                        cmd.Parameters.AddWithValue("@TDate", dateTimePicker1.Text);
                        decimal Percent = decimal.Parse(PercentofNo.Text);
                        decimal price2 = decimal.Parse(tprice.Text);
                        cmd1.CommandText = "insert into Profittbl (ProductID,Price,Profit,Today) values (@PID, @PPrice, @Profit,@Today)";
                        cmd1.Parameters.AddWithValue("@PID", textBox11.Text);
                        cmd1.Parameters.AddWithValue("@PPrice", price2);
                        cmd1.Parameters.AddWithValue("@Profit", Percent);
                        cmd1.Parameters.AddWithValue("@Today", dateTimePicker1.Text);
                        cmd.ExecuteNonQuery();
                        cmd1.ExecuteNonQuery();
                        con.GetClose();
                        LoadData3();
                        adddata();
                        SubtractData();
                        textBox1.Text = TotAmount.Text;
                        panel2.BackColor = Color.ForestGreen;
                        qty.Text = "0";
                        textBox4.Focus();
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { textBox4.Visible = false; textBox3.Visible = true; textBox3.Select(); textBox3.Clear(); } else { textBox4.Visible = true; textBox3.Visible = false; textBox4.Focus(); }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from Supplytbl where Barcode='" + textBox3.Text + "' AND OrderStat= 'Recieved'", con.GetConnection());
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da.SelectCommand = cmd;
            dt.Clear();
            da.Fill(dt);
            dataGridView3.DataSource = dt;
            dataGridView3.Width = 664;
            dataGridView3.Height = 177;
            dataGridView3.Columns[0].Visible = false;
            dataGridView3.Columns[1].Visible = false;
            dataGridView3.Columns[4].Visible = false;
            dataGridView3.Columns[5].Visible = false;
            dataGridView3.Columns[7].Visible = false;
            dataGridView3.Columns[11].Visible = false;
            dataGridView3.Columns[12].Visible = false;
            dataGridView3.Columns[10].Visible = false;
            con.GetClose();
            panel2.BackColor = Color.DarkGoldenrod;
        }

        private void custid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            adddata();
            custid.Text = "";
            itemname.Text = "";
            custname.Text = "";
            qty.Text = "0";
        }

        private void orderSupplyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderSupply orderSupply = new OrderSupply();
            orderSupply.Show();
        }

        private void customersAndSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomersSales customersSales = new CustomersSales();
            customersSales.Show();
        }

        private void debitSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebitSales debitSales = new DebitSales();
            debitSales.Show();
        }

        private void expiredItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpiredItems expiredItems = new ExpiredItems();
            expiredItems.Show();
        }
        private void Toggle(CheckBox checkbox)
        {
            checkbox.Checked = !checkbox.Checked;
        }
        private void SalesForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.SuppressKeyPress = (e.Control && e.KeyCode == Keys.B))

            {
                Toggle(checkBox1);
            }
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void newToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            adddata();
            custid.Text = "";
            itemname.Text = "";
            custname.Text = "";
            qty.Text = "0";
        }

        private void addToCartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string query = null;
            string query1 = null;
            SqlCommand cmd1 = new SqlCommand(query1, con.GetConnection());
            using (var cmd = new SqlCommand(query, con.GetConnection()))

            {
                if (saleID.Text != "" & itemname.Text != "")
                {
                    decimal price = decimal.Parse(tprice.Text);
                    cmd.CommandText = "insert into Salestbl (SalesID,CustomerID,CustomerName,ProductName,Quantity,Price,Discount,TDate) values (@id, @name, @type, @ProductName, @Quantity, @Price, @Discount, @TDate)";
                    cmd.Parameters.AddWithValue("@id", saleID.Text);
                    cmd.Parameters.AddWithValue("@name", custid.Text);
                    cmd.Parameters.AddWithValue("@type", custname.Text);
                    cmd.Parameters.AddWithValue("@ProductName", itemname.Text);
                    cmd.Parameters.AddWithValue("@Quantity", qty.Text);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Discount", dc.Text);
                    cmd.Parameters.AddWithValue("@TDate", dateTimePicker1.Text);
                    decimal Percent = decimal.Parse(PercentofNo.Text);
                    decimal price2 = decimal.Parse(tprice.Text);

                    cmd1.CommandText = "insert into Profittbl (ProductID,Price,Profit,Today) values (@PID, @PPrice, @Profit,@Today)";
                    cmd1.Parameters.AddWithValue("@PID", textBox11.Text);
                    cmd1.Parameters.AddWithValue("@PPrice", price2);
                    cmd1.Parameters.AddWithValue("@Profit", Percent);
                    cmd1.Parameters.AddWithValue("@Today", dateTimePicker1.Text);
                    cmd.ExecuteNonQuery();
                    cmd1.ExecuteNonQuery();
                    con.GetClose();
                    LoadData3();
                    adddata();
                    SubtractData();
                    textBox1.Text = TotAmount.Text;
                }
            }
        }
        private void sellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string query = null;
            using (var cmd = new SqlCommand(query, con.GetConnection()))
            {
                if (saleID.Text != "" & itemname.Text != "")
                {
                    float paid = float.Parse(textBox1.Text);
                    float tot = float.Parse(TotAmount.Text);
                    DateTime dt = DateTime.Parse(dateTimePicker1.Text);
                    cmd.CommandText = "insert into Soldtbl (CustomerID,CustomerName,SaleID,TotalAmount,Paidamount,TransactionDate) values (@ID, @name, @type, @ProductName,@paid,@tdate)";
                    cmd.Parameters.AddWithValue("@ID", custid.Text);
                    cmd.Parameters.AddWithValue("@name", custname.Text);
                    cmd.Parameters.AddWithValue("@type", saleID.Text);
                    cmd.Parameters.AddWithValue("@ProductName", tot);
                    cmd.Parameters.AddWithValue("@paid", paid);
                    cmd.Parameters.AddWithValue("@tdate", dt);
                    cmd.ExecuteNonQuery();
                    con.GetClose();
                    LoadData2();
                    adddata();
                    TodaysSale();
                }
            }
        }
        private void printToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string id = custid.Text;
            SalesReport soldPrintReport = new SalesReport(id);
            soldPrintReport.Show();
        }

        private void SalesForm_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        contextMenuStrip1.Show(this, new Point(e.X, e.Y));//places the menu at the pointer position
                    }
                    break;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        contextMenuStrip1.Show(this, new Point(e.X, e.Y));//places the menu at the pointer position
                    }
                    break;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        contextMenuStrip1.Show(this, new Point(e.X, e.Y));//places the menu at the pointer position
                    }
                    break;
            }
        }

        private void dateChangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateChanger dateChanger = new DateChanger();
            dateChanger.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateChangerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DateChanger dateChanger1 = new DateChanger();
            dateChanger1.Show();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Main();
        }

        private void custname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.SuppressKeyPress = (e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                textBox4.Focus();
                //this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox11.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                itemname.Text = dataGridView3.CurrentRow.Cells[2].Value.ToString();
                itemdetails.Text = dataGridView3.CurrentRow.Cells[3].Value.ToString();
                expdate.Text = dataGridView3.CurrentRow.Cells[9].Value.ToString();
                price1.Text = dataGridView3.CurrentRow.Cells[6].Value.ToString();
                Profit.Text = dataGridView3.CurrentRow.Cells[5].Value.ToString();
                panel2.BackColor = Color.DarkGoldenrod;
                dataGridView3.Height = 0;
                dataGridView3.Width = 0;
                qty.Select();
            }
        }

        private void dataGridView2_MouseEnter(object sender, EventArgs e)
        {
            dataGridView3.Height = 0;
            dataGridView3.Width = 0;
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            dataGridView3.Height = 0;
            dataGridView3.Width = 0;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
  
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
           
        }
    }
}