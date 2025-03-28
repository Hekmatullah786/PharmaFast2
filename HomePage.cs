using CircularProgressBar;
using LiveCharts;
using LiveCharts.Wpf;
using PharmaFast;
using PharmaFast.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Tulpep.NotificationWindow;

namespace PharmaFast
{
    public partial class HomePage : Form
    {
        ConnectionDB con = new ConnectionDB();
        public HomePage(string user, string pos)
        {
            InitializeComponent();
           welcometxt.Text = "Welcome "+user+" as "+pos;
            postxt.Text = pos;
           

        }
        private void button2_Click(object sender, EventArgs e)
        {
            ExpensesList products = new ExpensesList();
            products.Show();
        }
        private void chart()
        {
            Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0}", chartPoint.Y, chartPoint.Participation);
            SeriesCollection piechartData = new SeriesCollection
    {
        new PieSeries
        {
            Title = "Sales",
            Values = new ChartValues<double>{Convert.ToDouble(Sale.Text)},
            DataLabels = true,
           LabelPoint = labelPoint,
           Fill = System.Windows.Media.Brushes.BlueViolet
        },
        new PieSeries
        {
            Title = "Expenses",
            Values = new ChartValues<double> {Convert.ToDouble(expenses.Text)},
            DataLabels = true,
            LabelPoint = labelPoint,
            Fill = System.Windows.Media.Brushes.Gold
        },
        new PieSeries
        {
            Title = "Profit",
            Values = new ChartValues<double> {Convert.ToDouble(totprofit.Text)},
            DataLabels = true,
            LabelPoint = labelPoint,
            Fill = System.Windows.Media.Brushes.LimeGreen
        }
    };

            // You can add a new item dinamically with the add method of the collection
            // Useful when you define the data dinamically and not statically
            //piechartData.Add(
            //    new PieSeries
            //    {
            //        Title = "Sales",
            //        Values = new ChartValues<double> { Convert.ToDouble(Sale.Text) },
            //        DataLabels = true,
            //        LabelPoint = labelPoint,
            //        Fill = System.Windows.Media.Brushes.Black
            //    }
            //);

            // Define the collection of Values to display in the Pie Chart
            pieChart1.Series = piechartData;

            // Set the legend location to appear in the Right side of the chart
            pieChart1.LegendLocation = LegendLocation.Bottom;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            chart();
            todaystotal();
            totaldisc();
            totalcustomers();
            expireditems();
            notifier();
            totalexpenses();
            totaldebit();
            Incometotal();
            calc();
            Main();
            topsolditems();
            totalinvest();
            DateTime currentDate = DateTime.Now;
            date.Text = currentDate.ToString("dd/MM/yyyy");
            name.Text = Settings.Default["name"].ToString();
            info.Text = Settings.Default["info"].ToString();
            logo.Text = Settings.Default["logo"].ToString();
            dash.Text = Settings.Default["dashboard"].ToString();
            theme.Text = Settings.Default["theme"].ToString();
            label11.Text = Settings.Default["labelshow"].ToString();
            if (theme.Text == "Blue") { this.BackColor = Color.SteelBlue; panel1.BackColor = Color.DarkBlue; panel4.BackColor = Color.DarkBlue; info.BackColor = Color.DarkBlue; name.BackColor = Color.DarkBlue; date.ForeColor = Color.White; label10.ForeColor = Color.White; }
            if (theme.Text == "Default") { this.BackColor = SystemColors.Control; panel1.BackColor = SystemColors.ControlDark; }
            if (label11.Text == "Show")
            {
                Sale.Visible = true; textBox3.Visible = true; customers.Visible = true; totprofit.Visible = true;
                Supplieddrugs.Visible = true; expenses.Visible = true; debit.Visible = true; ExpDate.Visible = true;
                Investment.Visible = true; income.Visible = true; label13.Visible = true; label14.Visible = true; Topsold.Visible = true; label17.Visible = true; ; label15.Visible = true; label16.Visible = true;
            }
            else
            {
                Sale.Visible = false; textBox3.Visible = false; customers.Visible = false; totprofit.Visible = false;
                Supplieddrugs.Visible = false; expenses.Visible = false; debit.Visible = false; ExpDate.Visible = false;
                Investment.Visible = false; income.Visible = false; label13.Visible = false; label14.Visible = false; Topsold.Visible = false; label17.Visible = false; label15.Visible = false; label16.Visible = false;
            }
            string filePath = logo.Text;
            Image image = Image.FromFile(filePath);
            pictureBox1.Image = image;
            if (postxt.Text == "Pharmacist") { button3.Enabled = false;button13.Enabled = false;button5.Enabled = false;button10.Enabled = false; } else {  button3.Enabled = true;button13.Enabled = true; button5.Enabled = true; button10.Enabled = true; }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            circularProgressBar1.Invoke((MethodInvoker)delegate
            {
                circularProgressBar1.Text = DateTime.Now.ToString("hh:mm:ss");
                circularProgressBar1.SubscriptText = DateTime.Now.ToString("tt");
                circularProgressBar1.SubscriptColor = Color.Yellow;
            });
        }

        private void Main()
        {
            DateTime gregorianDate = dateTimePicker1.Value;
            PersianCalendar solarCalendar = new PersianCalendar();
            int solarDay = solarCalendar.GetDayOfMonth(gregorianDate);
            int solarMonth = solarCalendar.GetMonth(gregorianDate);
            int solarYear = solarCalendar.GetYear(gregorianDate);
            label10.Text = solarYear + "/" + solarMonth + "/" + solarDay;
        }
        private void notifier()
        {
            if (Int32.Parse(ExpDate.Text) >= 1)
            {
                popupNotifier1.Popup();
                popupNotifier1.ContentText = "You Have " + ExpDate.Text + " Items Expired Or Near To Expire!" + Environment.NewLine + "> Click Here To Check It!";
            }
        }
        private void topsolditems()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    string query = "SELECT TOP 1 ProductName, SUM(Quantity) AS TotalSold FROM Salestbl GROUP BY ProductName ORDER BY TotalSold DESC";

                    con.GetConnection();
                    cmd = new SqlCommand(query, con.GetConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string itemName = reader["ProductName"].ToString();
                        double totalSold = Convert.ToDouble(reader["TotalSold"]);
                        Topsold.Text = itemName;
                        label17.Text = Convert.ToString(totalSold);
                        con.GetClose();
                    }
                    reader.Close();
                }
                catch (Exception)
                { }
            }
        }
        private void calc()
        {
            decimal profit1 = Convert.ToDecimal(Profit.Text);
            decimal Dc1 = Convert.ToDecimal(DC.Text);
            decimal result = profit1 - Dc1;
            totprofit.Text = result.ToString();
        }
        private void todaystotal()
        {
            SqlCommand cmd = new SqlCommand();
            SqlCommand cmd1 = new SqlCommand();
            {
                try
                {
                    string query = "SELECT SUM(TotalAmount) FROM Soldtbl where TransactionDate='" + dateTimePicker1.Value + "'";
                    string query1 = "SELECT SUM(Profit) FROM Profittbl where Today='" + dateTimePicker1.Value + "'";
                    cmd = new SqlCommand(query, con.GetConnection());
                    cmd1 = new SqlCommand(query1, con.GetConnection());
                    decimal count = Convert.ToDecimal(cmd.ExecuteScalar());
                    decimal count1 = Convert.ToDecimal(cmd1.ExecuteScalar());
                    Sale.Text = count.ToString();
                    Profit.Text = count1.ToString();
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }
        private void Incometotal()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    string query = "SELECT SUM(TotalAmount) FROM Soldtbl";
                    cmd = new SqlCommand(query, con.GetConnection());
                    decimal count = Convert.ToDecimal(cmd.ExecuteScalar());
                    income.Text = count.ToString();
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }
        private void totaldebit()
        {
            SqlCommand cmd = new SqlCommand();
            SqlCommand cmd1 = new SqlCommand();
            {
                try
                {
                    string query = "SELECT SUM(TotalAmount) FROM Soldtbl";
                    string query1 = "SELECT SUM(Paidamount) FROM Soldtbl";
                    cmd = new SqlCommand(query, con.GetConnection());
                    decimal count = Convert.ToDecimal(cmd.ExecuteScalar());
                    cmd1 = new SqlCommand(query1, con.GetConnection());
                    decimal count1 = Convert.ToDecimal(cmd1.ExecuteScalar());
                    decimal result = count - count1;
                    debit.Text = result.ToString();
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }
        private void totalinvest()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    string query = "SELECT SUM(Cost*OrderedQ) FROM Supplytbl";
                    cmd = new SqlCommand(query, con.GetConnection());
                    decimal count = Convert.ToDecimal(cmd.ExecuteScalar());
                    Investment.Text = count.ToString();
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }
        private void totaldisc()
        {
            SqlCommand cmd5 = new SqlCommand();
            {
                try
                {
                    string query5 = "SELECT SUM(Discount) FROM Salestbl where TDate='" + dateTimePicker1.Value + "'";
                    cmd5 = new SqlCommand(query5, con.GetConnection());
                    decimal count = Convert.ToDecimal(cmd5.ExecuteScalar());
                    DC.Text = count.ToString();
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }
        private void totalcustomers()
        {
            SqlCommand cmd = new SqlCommand();
            SqlCommand cmd1 = new SqlCommand();
            {
                try
                {
                    string query = "SELECT count(CustomerID) FROM Customertbl";
                    string query1 = "SELECT count(SupplyID) FROM Supplytbl";
                    cmd = new SqlCommand(query, con.GetConnection());
                    cmd1 = new SqlCommand(query1, con.GetConnection());
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    int count1 = Convert.ToInt32(cmd1.ExecuteScalar());
                    customers.Text = count.ToString();
                    Supplieddrugs.Text = count1.ToString();
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }
        private void totalexpenses()
        {
            SqlCommand cmd2 = new SqlCommand();
            {
                try
                {
                    string query3 = "SELECT SUM(Amountpaid) FROM Expensestbl where datepaid='" + dateTimePicker1.Value + "'";
                    cmd2 = new SqlCommand(query3, con.GetConnection());
                    decimal count = Convert.ToDecimal(cmd2.ExecuteScalar());
                    expenses.Text = count.ToString();
                    con.GetClose();
                }
                catch (Exception)
                {
                }
            }
        }
        private void expireditems()
        {
            SqlCommand cmd = new SqlCommand();
            {
                try
                {
                    string query = "SELECT count(SupplyID) FROM Supplytbl WHERE ExpDate < DATEADD(dd,60, DATEDIFF(dd, 0, GETDATE()))";
                    cmd = new SqlCommand(query, con.GetConnection());
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    ExpDate.Text = count.ToString();
                }
                catch (Exception)
                {
                }
                con.GetClose();
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            OrderSupply orderSupply = new OrderSupply();
            orderSupply.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            bool isOpen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Customer List")
                {
                    isOpen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (isOpen == false)
            {
                CustomerList addc = new CustomerList();
                addc.Show();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            BillingForm billingForm = new BillingForm();
            billingForm.Show();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            bool isOpen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "SalesForm")
                {
                    isOpen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (isOpen == false)
            {string ad=postxt.Text.Trim();
                SalesForm salesForm = new SalesForm(ad);
                salesForm.Show();
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            DebitSales debitSales = new DebitSales();
            debitSales.Show();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            CustomersSales customersSales = new CustomersSales();
            customersSales.Show();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            OrderSupply orderSupply = new OrderSupply();
            orderSupply.Show();
        }

        private void ExpDate_Click(object sender, EventArgs e)
        {
            ExpiredItems expiredItems = new ExpiredItems();
            expiredItems.Show();
        }
        private void popupNotifier1_Click(object sender, EventArgs e)
        {
            ExpiredItems expiredItems = new ExpiredItems();
            expiredItems.Show();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            ExpiredItems exp = new ExpiredItems();
            exp.Show();
        }
        private void addProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Products products = new Products();
            products.Show();
        }
        private void addCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCustomer addCustomer = new AddCustomer();
            addCustomer.Show();
        }
        private void button13_Click(object sender, EventArgs e)
        {
            HospitalSetting newhos = new HospitalSetting();
            newhos.ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {string ad=postxt.Text;
            SalesForm salesForm = new SalesForm(ad);
            salesForm.Show();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            CustomersSales customersSales = new CustomersSales();
            customersSales.Show();
        }
        private void supplyItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderSupply orderSupply = new OrderSupply();
            orderSupply.Show();
        }
        private void searchACustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomersSales customersSales = new CustomersSales();
            customersSales.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DebitSales debitSales = new DebitSales();
            debitSales.Show();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Login login = new Login();
            login.Show();
        }
        private void otherDealsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OtherDeals otherDeals = new OtherDeals();
            otherDeals.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void topSoldItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopSoldItems topSoldItems = new TopSoldItems();
            topSoldItems.Show();
        }

        private void income_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            //ContextMenu cm = new ContextMenu();
            //contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 24F);
            //cm.MenuItems.Add(new MenuItem("Refresh", new EventHandler(button14_Click)));
            //cm.MenuItems.Add(new MenuItem("Sales Form", new EventHandler(button1_Click)));
            //cm.MenuItems.Add(new MenuItem("Other Deals", new EventHandler(otherDealsToolStripMenuItem_Click)));
            //cm.MenuItems.Add(new MenuItem("Top Sold", new EventHandler(topSoldItemsToolStripMenuItem_Click)));
            //cm.MenuItems.Add(new MenuItem("Sign Out", new EventHandler(signOutToolStripMenuItem_Click)));
            //cm.MenuItems.Add(new MenuItem("Exit", new EventHandler(exitToolStripMenuItem_Click)));
            //contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 24F);
            //textBox1.ContextMenu = cm;

        }
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart();
            todaystotal();
            totaldisc();
            totalcustomers();
            expireditems();
            notifier();
            totalexpenses();
            totaldebit();
            Incometotal();
            calc();
            Main();
            topsolditems();
            totalinvest();
            name.Text = Settings.Default["name"].ToString();
            info.Text = Settings.Default["info"].ToString();
            logo.Text = Settings.Default["logo"].ToString();
            dash.Text = Settings.Default["dashboard"].ToString();
            theme.Text = Settings.Default["theme"].ToString();
            label11.Text = Settings.Default["labelshow"].ToString();
            if (theme.Text == "Blue") { this.BackColor = Color.SteelBlue; panel1.BackColor = Color.DarkBlue; info.BackColor = Color.DarkBlue; name.BackColor = Color.DarkBlue; date.ForeColor = Color.White; label10.ForeColor = Color.White; }
            if (theme.Text == "Default") { this.BackColor = SystemColors.Control; panel1.BackColor = SystemColors.ControlDark; }
            if (label11.Text == "Show")
            {
                Sale.Visible = true; textBox3.Visible = true; customers.Visible = true; totprofit.Visible = true;
                Supplieddrugs.Visible = true; expenses.Visible = true; debit.Visible = true; ExpDate.Visible = true;
                Investment.Visible = true; income.Visible = true; label13.Visible = true; label14.Visible = true; Topsold.Visible = true; label17.Visible = true; ; label15.Visible = true; label16.Visible = true;
            }
            else
            {
                Sale.Visible = false; textBox3.Visible = false; customers.Visible = false; totprofit.Visible = false;
                Supplieddrugs.Visible = false; expenses.Visible = false; debit.Visible = false; ExpDate.Visible = false;
                Investment.Visible = false; income.Visible = false; label13.Visible = false; label14.Visible = false; Topsold.Visible = false; label17.Visible = false; label15.Visible = false; label16.Visible = false;
            }
            string filePath = logo.Text;
            Image image = Image.FromFile(filePath);
            pictureBox1.Image = image;
        }

        private void HomePage_MouseDown(object sender, MouseEventArgs e)
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

        private void salesFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesForm salesForm = new SalesForm(ad:HomePage.ActiveForm.Text);
            salesForm.Show();
        }

        private void otherDealsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OtherDeals otherDeals = new OtherDeals();
            otherDeals.Show();
        }

        private void topSoldItemsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TopSoldItems topSoldItems = new TopSoldItems();
            topSoldItems.Show();
        }

        private void signOutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
            Login login = new Login();
            login.Show();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void registrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LicenseKey licenseKey = new LicenseKey();
            licenseKey.Show();
        }

        private void dateChangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateChanger dateChanger = new DateChanger();
            dateChanger.Show();
        }

        private void dateChangerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DateChanger dateChanger1 = new DateChanger();
            dateChanger1.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            panel4.Hide();
            label18.Visible = true;
        }

        private void label18_Click(object sender, EventArgs e)
        {
            panel4.Show();
            label18.Visible = false;
        }
    }
}