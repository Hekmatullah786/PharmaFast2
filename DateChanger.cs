using ABlue.PersianDateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmaFast
{
    public partial class DateChanger : Form
    {
        public DateChanger()
        {
            InitializeComponent();
        }
        private void main()
        {
            DateTime gregorianDate = dateTimePicker1.Value;
            PersianCalendar solarCalendar = new PersianCalendar();
            int solarDay = solarCalendar.GetDayOfMonth(gregorianDate);
            int solarMonth = solarCalendar.GetMonth(gregorianDate);
            int solarYear = solarCalendar.GetYear(gregorianDate);
            textBox1.Text = solarYear + "/" + solarMonth + "/" + solarDay;
            year.Text = solarYear.ToString();
            month.Text = solarMonth.ToString();
            day.Text = solarDay.ToString();
            string[] afghanMonths = { "حمل", "ثور", "جوزا", "سرطان", "اسد", "سنبله",
                                  "میزان", "عقرب", "قوس", "جدی", "دلو", "حوت" };
            textBox2.Text=($"تاریخ شمسی: {solarDay} {afghanMonths[solarMonth - 1]} {solarYear}");
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            main();
        }
        private void DateChanger_Load(object sender, EventArgs e)
        {main(); 
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            day2.ReadOnly = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void persiandate()
        {
            try
            {
                int pyear = Convert.ToInt32(year.Text);
                int pmonth = Convert.ToInt32(month.Text);
                int pday = Convert.ToInt32(day.Text);
                PersianCalendar persianCalendar = new PersianCalendar();
                DateTime dateTime = persianCalendar.ToDateTime(pyear, pmonth, pday, 0, 0, 0, 0);
                day2.Text = "Gregorian: " + dateTime.ToString("yyyy/dd/MM");
            }
            catch (Exception) { }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            persiandate();
  
        }

        private void month_TextChanged(object sender, EventArgs e)
        {
            persiandate();
        }

        private void day_TextChanged(object sender, EventArgs e)
        {
            persiandate();
        }
    }
}