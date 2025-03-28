using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaFast
{
    public class ConnectionDB
    {
        private SqlConnection con;
        public SqlConnection GetConnection()
        {
            con = new SqlConnection("Data Source=(localdb)\\localdb;AttachDbFilename=D:\\MYDB\\MyDataBase.mdf;Integrated Security=True");
            if (con.State == ConnectionState.Open)
            { con.Close(); con.Open(); }
            else
            { con.Open(); }
            return con;
        }
        public void GetClose()
        {
            if (con == null)
            { con.Close(); }
        }
    }
}
