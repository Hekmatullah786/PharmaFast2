using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmaFast
{
    public partial class SalesReport : Form
    {
        ConnectionDB con = new ConnectionDB();
        public SalesReport(string id)
        {
            InitializeComponent();
            label2.Text = id;
        }
        private void printrep()
        {
            try
            {
                string note = label1.Text;
                string query = "SELECT * FROM Salestbl WHERE CustomerID=" + label2.Text + " AND TDate='" + dateTimePicker1.Value + "'";
                using (SqlCommand command = new SqlCommand(query, con.GetConnection()))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No records found for the given ID.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    reportViewer1.LocalReport.ReportPath = Application.StartupPath + "\\SalesRep.rdlc";
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dataTable);
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportParameter parm1 = new ReportParameter("Title", note);
                    reportViewer1.LocalReport.SetParameters(parm1);
                    // reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                    reportViewer1.RefreshReport();
                    con.GetClose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SalesReport_Load(object sender, EventArgs e)
        {

            label1.Text = Properties.Settings.Default["name"].ToString();
            printrep();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            printrep();
        }

        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {
            printrep();
        }
    }
}
