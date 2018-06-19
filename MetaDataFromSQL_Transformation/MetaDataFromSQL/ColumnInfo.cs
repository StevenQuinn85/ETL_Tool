using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MetaDataFromSQL
{
    class ColumnInfo
    {
        private string processName, columnName, dataType, action, replaceValue;
        int minLength, maxLength;
        bool nullPermitted;

        SqlConnection conn = new SqlConnection(@"Data Source=WINDOWS-I92V0KI\SQLEXPRESS;Initial Catalog=ThirdYear;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        public string ProcessName
        {
            get
            {
                return processName;
            }

            set
            {
                processName = value;
            }
        }

        public string ColumnName
        {
            get
            {
                return columnName;
            }

            set
            {
                columnName = value;
            }
        }

        public string DataType
        {
            get
            {
                return dataType;
            }

            set
            {
                dataType = value;
            }
        }

        public string Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
            }
        }

        public string ReplaceValue
        {
            get
            {
                return replaceValue;
            }

            set
            {
                replaceValue = value;
            }
        }

        public int MinLength
        {
            get
            {
                return minLength;
            }

            set
            {
                minLength = value;
            }
        }

        public int MaxLength
        {
            get
            {
                return maxLength;
            }

            set
            {
                maxLength = value;
            }
        }

        public bool NullPermitted
        {
            get
            {
                return nullPermitted;
            }

            set
            {
                nullPermitted = value;
            }
        }

        //public void GetColumnInfo()
        //{
        //    string query = "SELECT C.[ColumnName], C.[DataType], C.[MinLength], C.[MaxLength], C.[Nullable], C.[Action], C.[ReplaceValue] FROM [dbo].[Program_ColumnData] C JOIN [dbo].[Program_MetaData] M ON C.[Processid] = M.[Process_id] WHERE M.[PROCESSNAME] = '" + processName + "'";
        //    SqlCommand comm = new SqlCommand(query, conn);

        //    conn.Open();

        //    using (comm)
        //    {
        //        SqlDataReader reader = comm.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            columnName = reader.GetString(0);
        //            dataType = reader.GetString(1);
        //            minLength = reader.GetInt32(2);
        //            maxLength = reader.GetInt32(3);
        //            nullPermitted = (reader.GetString(4).Equals("True")) ? true : false;
        //            action = reader.GetString(5);
        //            replaceValue = reader.GetString(6);

        //        }


        //    }
                
        //}
    }
}
