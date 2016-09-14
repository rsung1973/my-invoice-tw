using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Utility
{
    public class ExcelStore : IDisposable
    {
        private OleDbConnection _conn;
        private string _targetPath;
        private bool _bDisposed = false;
        private String _xlsSample;

        public ExcelStore(string xlsSample)
            : this(xlsSample, Path.Combine(Logger.LogDailyPath, System.Guid.NewGuid().ToString() + ".xls"))
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public ExcelStore(string xlsSample, string target)
        {
            //
            // TODO: Add constructor logic here
            //
            _xlsSample = xlsSample;
            _targetPath = target;

            if (!String.IsNullOrEmpty(_xlsSample) && File.Exists(_xlsSample))
            {
                File.Copy(_xlsSample, _targetPath);
            }

            _conn = new OleDbConnection();
            _conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _targetPath + ";Extended Properties=Excel 8.0";
        }

        public string TargetPath
        {
            get
            {
                return _targetPath;
            }
        }
        

        public void Save(DataTable tbl, OleDbCommand cmd)
        {
            _conn.Open();
            try
            {
                cmd.Connection = _conn;

                foreach (DataRow row in tbl.Rows)
                {
                    foreach (OleDbParameter param in cmd.Parameters)
                    {
                        param.Value = row[param.ParameterName];
                    }

                    cmd.ExecuteNonQuery();
                }

            }
            finally
            {
                _conn.Close();
            }
        }

        public void Save<T>(IEnumerable<T> tbl, OleDbCommand cmd)
        {
            _conn.Open();
            try
            {
                cmd.Connection = _conn;

                foreach (var row in tbl)
                {
                    foreach (OleDbParameter param in cmd.Parameters)
                    {
                        param.Value = row.GetPropertyValue(param.ParameterName);
                        if (param.Value == null)
                        {
                            param.Value = DBNull.Value;
                        }
                    }

                    cmd.ExecuteNonQuery();
                }

            }
            finally
            {
                _conn.Close();
            }
        }



        public void Save(DataSet ds)
        {
            List<string> list = new List<string>();
            foreach (DataTable table in ds.Tables)
            {
                list.Clear();

                foreach (DataColumn column in table.Columns)
                {
                    list.Add(column.ColumnName);
                }

                createTable(table.TableName, list);
                putData(table, list);

            }
        }

        public void PutData(DataTable table, IList<string> columnName)
        {
            createTable(table.TableName, columnName);
            putData(table, columnName);
        }


        private void putData(DataTable table, IList<string> columnName)
        {
            using (System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand())
            {
                cmd.Connection = _conn;
                cmd.CommandType = CommandType.Text;

                System.Text.StringBuilder sb = new System.Text.StringBuilder("insert into [");
                sb.Append(table.TableName).Append("$] values ( @").Append(columnName[0]);
                cmd.Parameters.Add("@" + columnName[0], System.Data.OleDb.OleDbType.BSTR);

                for (int i = 1; i < columnName.Count; i++)
                {
                    string c = "@" + columnName[i];
                    sb.Append(",").Append(c);
                    cmd.Parameters.Add(c, System.Data.OleDb.OleDbType.BSTR);
                }
                sb.Append(" )");
                cmd.CommandText = sb.ToString();

                foreach (DataRow row in table.Rows)
                {
                    for (int i = 0; i < columnName.Count; i++)
                    {
                        cmd.Parameters[i].Value = row[columnName[i]];
                    }
                    cmd.ExecuteNonQuery();
                }
            }

        }

        private void createTable(string tableName, IList<string> columnName)
        {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }

            using (System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand())
            {
                cmd.Connection = _conn;
                cmd.CommandType = CommandType.Text;
                System.Text.StringBuilder sb = new System.Text.StringBuilder("Create table `");
                sb.Append(tableName).Append("` ( ");

                sb.Append("`").Append(columnName[0]).Append("` LongText ");
                for (int i = 1; i < columnName.Count; i++)
                {
                    sb.Append(",`").Append(columnName[i]).Append("` LongText ");
                }
                sb.Append(" )");

                cmd.CommandText = sb.ToString();

                cmd.ExecuteNonQuery();
            }

        }

        ~ExcelStore()
        {
            Dispose();
        }


        #region IDisposable Members

        public void Dispose()
        {
            if (!_bDisposed)
            {
                _bDisposed = true;
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            else
                return;

        }

        #endregion
    }
}
