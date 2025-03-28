using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace PharmaFast
{
    public partial class BillingForm : Form
    {
        ConnectionDB con = new ConnectionDB();
        private Button btnExport = new Button();
        public BillingForm()
        {
            InitializeComponent();
        }
        private void BillingForm_Load(object sender, EventArgs e)
        {
            loaddata();
            Main1();
            Main2();
        }
        private void Main1()
        {
            DateTime gregorianDate = dateTimePicker1.Value;
            PersianCalendar solarCalendar = new PersianCalendar();
            int solarDay = solarCalendar.GetDayOfMonth(gregorianDate);
            int solarMonth = solarCalendar.GetMonth(gregorianDate);
            int solarYear = solarCalendar.GetYear(gregorianDate);
            string[] afghanMonths = { "حمل", "ثور", "جوزا", "سرطان", "اسد", "سنبله",
                                  "میزان", "عقرب", "قوس", "جدی", "دلو", "حوت" };
            label6.Text = ($"تاریخ: {solarDay} {afghanMonths[solarMonth - 1]} {solarYear}");
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
            label5.Text = ($"تاریخ: {solarDay} {afghanMonths[solarMonth - 1]} {solarYear}");
        }
        private void loaddata()
        {
            string query = "SELECT * FROM Soldtbl where TransactionDate='" + dateTimePicker1.Text + "'";
            DataTable dataTable = new DataTable();
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
          .Sum(t => Convert.ToDecimal(t.Cells[5].Value));
                    Total.Text = total.ToString();
                    Main1();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            con.GetClose();
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = 50, y = 100;
            System.Drawing.Font font = new System.Drawing.Font("Arial", 10);
            Brush brush = Brushes.Black;

            // Print Column Headers
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                e.Graphics.DrawString(col.HeaderText, font, brush, x, y);
                x += 100;
            }

            y += 30;
            x = 50;

            // Print Rows
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    e.Graphics.DrawString(cell.Value?.ToString() ?? "", font, brush, x, y);
                    x += 100;
                }
                y += 30;
                x = 50;
            }
        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show Print Preview
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            PrintDocument pd = new PrintDocument();
            ppd.Text = "Print Preview";
            ppd.Size = new Size(1000, 700);  // Adjust the size as needed
            ppd.PrintPreviewControl.Zoom = 1;
            pd.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            ppd.Document = pd;
            ppd.ShowDialog();
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Soldtbl where TransactionDate between '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
            DataTable dataTable = new DataTable();
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
             .Sum(t => Convert.ToDecimal(t.Cells[5].Value));
                    Total.Text = total.ToString();
                    loadtotal();
                    con.GetClose();
                    Main2();
                    Main1();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Soldtbl where TransactionDate between '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
            DataTable dataTable = new DataTable();
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
                                .Sum(t => Convert.ToDecimal(t.Cells[4].Value));
                    Total.Text = total.ToString();
                    loadtotal();
                    Main2();
                    Main1();
                    con.GetClose();



                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void pDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                Title = "Save as PDF"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Document document = new Document(iTextSharp.text.PageSize.A4);
                PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                document.Open();

                PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);

                // Add headers
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                }

                // Add Data Rows
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Skip empty row
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            table.AddCell(cell.Value?.ToString() ?? "");
                        }
                    }
                }

                document.Add(table);
                document.Close();

                MessageBox.Show("PDF Exported Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int i = 1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[0].Value = i;
                i++;

            }
        }

        private void loadtotal()
        {
            dataGridView1[4, dataGridView1.Rows.Count - 1].Value = "Total";
            dataGridView1[5, dataGridView1.Rows.Count - 1].Value = Total.Text;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[5].Style.BackColor = Color.Yellow;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                string query = "SELECT * FROM Soldtbl";
                DataTable dataTable = new DataTable();
                {
                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                        dataAdapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                        decimal total = dataGridView1.Rows.Cast<DataGridViewRow>()
                                    .Sum(t => Convert.ToDecimal(t.Cells[4].Value));
                        Total.Text = total.ToString();
                        loadtotal();
                        con.GetClose();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
            else { loaddata(); }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete item?",
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
                        label4.Text = "Successfully Deleted!";
                        loaddata();
                    }
                }
                catch (Exception) { MessageBox.Show("Please select a row from datagridview"); }
            }
        }
    }
}

