using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Uxnet.Com.Utility
{
    public class DBTool
    {
        private DBTool()
        { }

        public static void AttachDatabase(string dbName, IEnumerable<String> fileName)
        {
            using (SqlConnection dbConn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=localhost\\sqlexpress"))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("if not exists (select name from master.sys.databases where name='").Append(dbName).Append("') \r\n")
                    .Append("CREATE DATABASE [").Append(dbName).Append("] ON ")
                    .Append(String.Join(",", fileName.Select(f => String.Format(" ( FILENAME = N'{0}' )", f)).ToArray()))
                    .Append(" FOR ATTACH");

                using (SqlCommand sqlCmd = new SqlCommand(sb.ToString(), dbConn))
                {
                    dbConn.Open();
                    sqlCmd.ExecuteNonQuery();
                    dbConn.Close();
                }
            }
        }

        public static void BackupDatabase(string dbName, String fileName)
        { 
            using (SqlConnection dbConn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=localhost\\sqlexpress"))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("BACKUP DATABASE [")
                    .Append(dbName).Append("] TO  DISK = N'")
                    .Append(fileName).Append("' WITH NOFORMAT, INIT,  NAME = N'BORC-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10");

                using (SqlCommand sqlCmd = new SqlCommand(sb.ToString(), dbConn))
                {
                    dbConn.Open();
                    sqlCmd.ExecuteNonQuery();
                    dbConn.Close();
                }
            }
        }

        public static void RestoreDatabase(string dbName, String fileName)
        {
            using (SqlConnection dbConn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=localhost\\sqlexpress"))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("RESTORE DATABASE [")
                    .Append(dbName).Append("] FROM  DISK = N'")
                    .Append(fileName).Append("' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10");

                using (SqlCommand sqlCmd = new SqlCommand(sb.ToString(), dbConn))
                {
                    dbConn.Open();
                    sqlCmd.ExecuteNonQuery();
                    dbConn.Close();
                }
            }
        }

    }
}
