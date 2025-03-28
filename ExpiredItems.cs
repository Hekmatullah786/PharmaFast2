using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PharmaFast
{
    public partial class ExpiredItems : Form
    {
        ConnectionDB con = new ConnectionDB();
        public ExpiredItems()
        {
            InitializeComponent();
        }
        private void loaddata()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    DataTable dataTable = new DataTable();
                    string query = "SELECT * FROM Supplytbl WHERE ExpDate < DATEADD(dd,60, DATEDIFF(dd, 0, GETDATE()))";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con.GetConnection());
                    dataAdapter.Fill(dataTable);
                    dataGridView2.DataSource = dataTable;
                    dataGridView2.Columns["Cost"].Visible = false;
                    dataGridView2.Columns["Benifit"].Visible = false;
                    dataGridView2.Columns["TPrice"].Visible = false;
                    dataGridView2.Columns["OrderStat"].Visible = false;
                    dataGridView2.Columns["Payment"].Visible = false;
                    dataGridView2.Columns["Barcode"].Visible = false;
                    label1.Text = dataGridView2.Rows.Count.ToString();

                }
                catch (Exception)
                {
                }
                con.GetClose();
            }
        }
        private void loaddata2()
        {

        }
        private void ExpiredItems_Load(object sender, EventArgs e)
        {

            loaddata();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to save Changes",
        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    int SID = int.Parse(dataGridView2.CurrentRow.Cells[0].Value.ToString());
                    string SPN = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                    string PN = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                    string PC = dataGridView2.CurrentRow.Cells[3].Value.ToString();
                    int Pcost = int.Parse(dataGridView2.CurrentRow.Cells[4].Value.ToString());
                    int Benifit = int.Parse(dataGridView2.CurrentRow.Cells[5].Value.ToString());
                    int OrderQ = int.Parse(dataGridView2.CurrentRow.Cells[6].Value.ToString());
                    int Price = int.Parse(dataGridView2.CurrentRow.Cells[7].Value.ToString());
                    DateTime dated = DateTime.Parse(dataGridView2.CurrentRow.Cells[8].Value.ToString());
                    string orderstat = dataGridView2.CurrentRow.Cells[9].Value.ToString();
                    int paid = int.Parse(dataGridView2.CurrentRow.Cells[10].Value.ToString());
                    string query = "update Supplytbl set SupplierName=@SPN,ProductName=@PN,Company=@PC,Cost=@PCost,Benifit=@Benifit,OrderedQ=@OrderQ,TPrice=@price,ExpDate=@edate,OrderStat=@orderstat,Payment=@paid where SupplyID=@SID";
                    SqlCommand cmd = new SqlCommand(query, con.GetConnection());
                    cmd.Parameters.AddWithValue("@SID", SID);
                    cmd.Parameters.AddWithValue("@SPN", SPN);
                    cmd.Parameters.AddWithValue("@PN", PN);
                    cmd.Parameters.AddWithValue("@PC", PC);
                    cmd.Parameters.AddWithValue("@PCost", Pcost);
                    cmd.Parameters.AddWithValue("@Benifit", Benifit);
                    cmd.Parameters.AddWithValue("@OrderQ", OrderQ);
                    cmd.Parameters.AddWithValue("@price", Price);
                    cmd.Parameters.AddWithValue("@edate", dated);
                    cmd.Parameters.AddWithValue("@orderstat", orderstat);
                    cmd.Parameters.AddWithValue("@paid", paid);

                    int x = cmd.ExecuteNonQuery();
                    loaddata();
                    con.GetClose();
                    MessageBox.Show("Record Successfully Updated!");
                }
                catch (Exception)
                {
                }
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("Are you sure to save Changes",
        "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {

                try
                {
                    int SID = int.Parse(dataGridView2.CurrentRow.Cells[0].Value.ToString());
                    string query = "delete from Supplytbl  where SupplyID=@SID";
                    SqlCommand cmd = new SqlCommand(query, con.GetConnection());
                    cmd.Parameters.AddWithValue("@SID", SID);
                    int x = cmd.ExecuteNonQuery();
                    loaddata();
                    con.GetClose();
                    MessageBox.Show("Record Successfully Deleted!");
                }
                catch (Exception)
                {
                }
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

