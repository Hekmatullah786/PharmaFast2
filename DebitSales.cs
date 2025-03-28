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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PharmaFast
{
    public partial class DebitSales : Form
    {
        ConnectionDB con = new ConnectionDB();
        public DebitSales()
        {
            InitializeComponent();
        }

        private void DebitSales_Load(object sender, EventArgs e)
        {
            loaddata();
        }
        private void loaddata()
        {
            try
            {
                //  string note = label2.Text;
                string query = "SELECT * FROM Soldtbl where Paidamount<TotalAmount";
                using (SqlCommand command = new SqlCommand(query, con.GetConnection()))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    con.GetClose();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to save Changes",
       "Update", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {

                try
                {
                    int SID = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    decimal SPN = decimal.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                    decimal PN = decimal.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString());
                    string query = "update Soldtbl set TotalAmount=@SPN,Paidamount=@PN where SoldID=@SID";
                    SqlCommand cmd = new SqlCommand(query, con.GetConnection());
                    cmd.Parameters.AddWithValue("@SID", SID);
                    cmd.Parameters.AddWithValue("@SPN", SPN);
                    cmd.Parameters.AddWithValue("@PN", PN);
                     cmd.ExecuteNonQuery();
                    loaddata();
                    con.GetClose();
                    MessageBox.Show("Record Successfully Updated!");
                }
                catch (Exception)
                {
                }
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You Can Only Update The Data,\nPlease Do Not Delete The Item To Prevent Losing Data!");
        }
        private void byid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Soldtbl where CustomerID =" + byid.Text + "AND Paidamount<TotalAmount", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.GetClose();
                double sum = 0.00;
                double sum2 = 0.00;
                double result = 0.00;
                for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                {
                    sum = sum + double.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    sum2 = sum2 + double.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                    result = sum - sum2;
                }
                Total.Text = sum.ToString();
                label1.Text = sum2.ToString();
                payable.Text = result.ToString();

                con.GetClose();
            }
            catch
            { }
        }
        private void label4_Click(object sender, EventArgs e)
        { 
        }
        private void byname_TextChanged(object sender, EventArgs e)
        {

            try
            {
                SqlCommand cmd = new SqlCommand("select * from Soldtbl where CustomerName like '%" + byname.Text + "%' AND Paidamount < TotalAmount", con.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                dt.Clear();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.GetClose();
                double sum = 0.00;
                double sum2 = 0;
                double result = 0;
                for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                {
                    sum = sum + double.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    sum2 = sum2 + double.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                    result = sum - sum2;
                }
                Total.Text = sum.ToString();
                label1.Text = sum2.ToString();
                payable.Text = result.ToString();
                con.GetClose();
            }
            catch
            { }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Text = "Search By Name";
                byname.Visible = true;
                byid.Visible = false;
            }else {
                checkBox1.Text = "Search By ID";
                byname.Visible = false;
                byid.Visible = true;
            }
    }
    }
}


