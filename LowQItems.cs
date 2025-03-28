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
    public partial class LowQItems : Form
    {
        ConnectionDB con = new ConnectionDB();
        public LowQItems()
        {
            InitializeComponent();
        }

        private void LowQItems_Load(object sender, EventArgs e)
        {
            itemslow();
        }
        private void itemslow()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    DataTable dataTable = new DataTable();
                    string query = "SELECT * FROM Supplytbl WHERE OrderedQ <30";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns["Cost"].Visible = false;
                    dataGridView1.Columns["Benifit"].Visible = false;
                    dataGridView1.Columns["TPrice"].Visible = false;
                    dataGridView1.Columns["OrderStat"].Visible = false;
                    dataGridView1.Columns["Payment"].Visible = false;
                    dataGridView1.Columns["SupplyID"].Visible = false;
                    dataGridView1.Columns["Barcode"].Visible = false;
                    dataGridView1.Columns["Price"].Visible = false;
                    label1.Text = dataGridView1.Rows.Count.ToString();

                }
                catch (Exception)
                {
                }
                con.GetClose();
            }
        }
    }
}
