using Microsoft.Reporting.WinForms;
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

namespace PharmaFast
{
    public partial class SoldPrintReport : Form
    {
        ConnectionDB con = new ConnectionDB();
        public SoldPrintReport(string id)
        {
            InitializeComponent();
            label1.Text = id;
        }

        private void SoldPrintReport_Load(object sender, EventArgs e)
        {

            label2.Text = Properties.Settings.Default["name"].ToString();
            try
            {
                string note = label2.Text;
                string query = "SELECT * FROM Soldtbl WHERE CustomerID=" + label1.Text + "";
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
                    reportViewer1.LocalReport.ReportPath = Application.StartupPath + "\\SoldRep.rdlc";
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dataTable);
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportParameter parm1 = new ReportParameter("Title", note);
                    reportViewer1.LocalReport.SetParameters(parm1);
                    //  reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                    reportViewer1.RefreshReport();
                    con.GetClose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
