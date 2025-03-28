using Microsoft.Win32;
using PharmaFast;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmaFast
{
    public partial class LicenseKey : Form
    {
        public LicenseKey()
        {
            InitializeComponent();
        }
        public void AddToRegistry(string keyName, string valueName, string valueData)
        {
            // Open or create the registry key
            RegistryKey key = Registry.CurrentUser.CreateSubKey(keyName);

            if (key != null)
            {
                // Set the value
                key.SetValue(valueName, valueData);
                key.Close();
            }
        }
        public string RetrieveFromRegistry(string keyName, string valueName)
        {
            // Open the registry key
            RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName);

            if (key != null)
            {
                // Get the value
                object value = key.GetValue(valueName);
                key.Close();

                // Return the value as a string
                return value?.ToString();
            }

            return null;
        }


        private void LicenseKey_Load(object sender, EventArgs e)
        {
            string keyName = "Software\\PharmaFast";
            string valueName = "MySetting";
            string valueData = "saifi786-drhekmat786-4799100-2025";
            string retrievedData = RetrieveFromRegistry(keyName, valueName);
            {
                if (retrievedData == null) { MessageBox.Show("You Need To Register!"); }
                if (retrievedData != RetrieveFromRegistry(keyName, valueName))
                {
                    MessageBox.Show("Incorrect Register Code!");
                }
                else
                {
                    if (retrievedData == RetrieveFromRegistry(keyName, valueName)) { textBox1.Text = retrievedData; }
                    if (textBox1.Text == "saifi786-drhekmat786-4799100-2025")
                    {
                    }
                    {
                    }
                }
            }
        }
    }
}