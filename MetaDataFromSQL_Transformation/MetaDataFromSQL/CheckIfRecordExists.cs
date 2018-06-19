using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace MetaDataFromSQL
{
    class CheckIfRecordExists
    {
        public CheckIfRecordExists()
        {

        }

        public bool CheckForRecord(string SQLConnectString, string SQLCommand)
        {
            bool exists = false;

            SqlConnection Conn = new SqlConnection(SQLConnectString);
            Conn.Open();

            SqlCommand Comm = new SqlCommand(SQLCommand, Conn);

            Int32 count = Convert.ToInt32(Comm.ExecuteScalar());

            Conn.Close();
            if (count > 0)
            {
                exists = true;
            }


            return exists;
        }

    }
}
