
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmaFast
{
    public partial class TopSoldItems : Form
    {
        ConnectionDB con = new ConnectionDB();
        public TopSoldItems()
        {
            InitializeComponent();
        }

        private void TopSoldItems_Load(object sender, EventArgs e)
        {
            string query = "SELECT TOP 50 ProductName, SUM(Quantity) AS TotalSold FROM Salestbl GROUP BY ProductName ORDER BY TotalSold DESC";

            {
                DataTable dataTable = new DataTable();
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    con.GetClose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
    }
}
