using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;

using Uxnet.Com.Properties;

namespace DataAccessLayer.basis
{
    /// <summary>
    /// </summary>
    public class LayerBase : System.IDisposable
    {
        internal static ConnectionStringSettings _ConnectionString;

        private bool _bDisposed = false;

        protected SqlConnection _conn;
        protected object _uid;
        protected bool _useUID = false;
        protected string _orderBy;
        protected SqlTransaction _sqlTran;
        protected internal bool _isInstance = true;


        static LayerBase()
        {
            _ConnectionString = WebConfigurationManager.ConnectionStrings[Settings.Default.DBConnectionStringName];
        }


        public LayerBase()
        {
            //
            // TODO: Add constructor logic here
            //
            _conn = new SqlConnection();
            _conn.ConnectionString = _ConnectionString.ConnectionString;
        }

        public LayerBase(String connectionString)
        {
            //
            // TODO: Add constructor logic here
            //
            _conn = new SqlConnection();
            _conn.ConnectionString = connectionString;
        }

        public LayerBase(IDbConnection conn)
        {
            _conn = conn as SqlConnection;
            _isInstance = false;
        }

        ~LayerBase()
        {
            Dispose();
        }

        public void Dispose()
        {
            dispose(true);
        }

        private void dispose(bool disposing)
        {
            if (!_bDisposed)
            {
                if (disposing)
                {
                    if (_isInstance)
                    {
                        if (_conn.State == System.Data.ConnectionState.Open)
                        {
                            _conn.Close();
                        }
                    }
                }
                _bDisposed = true;
            }
        }


        public SqlConnection Connection
        {
            get
            {
                return _conn;
            }
        }

        protected DataSet dumpSchema(string sqlCmd)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd, this._conn))
            {
                da.FillSchema(ds, SchemaType.Source);
            }
            return ds;
        }

        protected DataSet dumpSchema(SqlCommand sqlCmd)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
            {
                da.FillSchema(ds, SchemaType.Source);
            }
            return ds;
        }

        protected int contains(string tableName, string columnName, object columnValue)
        {
            //
            // TODO: 到DB中讀取UserProfile資料
            //

            try
            {
                _conn.Open();
                SqlCommand sqlCmd = new SqlCommand(String.Format("select count(*) from {0} where {1} = @ParamValue ",
                    tableName, columnName), _conn);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@ParamValue", columnValue);

                return (int)sqlCmd.ExecuteScalar();
            }
            finally
            {
                _conn.Close();
            }
        }


        protected void executeSql(string cmdText)
        {
            try
            {
                _conn.Open();
                SqlCommand sqlCmd = new SqlCommand(cmdText, _conn);
                sqlCmd.ExecuteNonQuery();
            }
            finally
            {
                _conn.Close();
            }
        }

        protected void executeSqlCommand(SqlCommand sqlCmd)
        {
            try
            {
                _conn.Open();

                sqlCmd.Connection = _conn;

                sqlCmd.ExecuteNonQuery();

            }
            finally
            {
                _conn.Close();
            }
        }


        protected SqlCommand executeSqlCommand(string storedProcedure, string[] paramName, object[] paramValue)
        {
            try
            {
                _conn.Open();

                SqlCommand sqlCmd;

                if (_useUID)
                    sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn, _uid);
                else
                    sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn);


                for (int i = 0; i < paramName.Length; i++)
                {
                    sqlCmd.Parameters[paramName[i]].Value = paramValue[i];
                }

                sqlCmd.ExecuteNonQuery();
                return sqlCmd;
            }
            finally
            {
                _conn.Close();
            }
        }

        protected object executeProcedure(string storedProcedure, string[] paramName, object[] paramValue)
        {
            return executeSqlCommand(storedProcedure, paramName, paramValue).Parameters["@RETURN_VALUE"].Value;
        }


        protected SqlCommand executeSqlCommand(string storedProcedure, IDictionary paramValue)
        {

            try
            {
                _conn.Open();

                SqlCommand sqlCmd;

                if (_useUID)
                    sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn, _uid);
                else
                    sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn);

                ModalUtility.AssignCommandParameter(sqlCmd, paramValue);

                sqlCmd.ExecuteNonQuery();
                return sqlCmd;
            }
            finally
            {
                _conn.Close();
            }
        }

        protected SqlDataReader executeReader(string storedProcedure, IDictionary paramValue)
        {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }

            SqlCommand sqlCmd;

            if (_useUID)
                sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn, _uid);
            else
                sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn);

            ModalUtility.AssignCommandParameter(sqlCmd, paramValue);

            return sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        protected SqlDataReader executeReader(string storedProcedure, params object[] paramValue)
        {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }

            SqlCommand sqlCmd;

            if (_useUID)
                sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn, _uid);
            else
                sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn);

            ModalUtility.AssignCommandParameter(sqlCmd, paramValue);

            return sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }



        protected object executeProcedure(string storedProcName, IDictionary paramValue)
        {
            return executeSqlCommand(storedProcName, paramValue).Parameters["@RETURN_VALUE"].Value;
        }


        protected SqlCommand executeSqlCommand(string storedProcedure, params object[] cmdParams)
        {
            try
            {
                _conn.Open();
                SqlCommand sqlCmd;

                if (_useUID)
                    sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn, _uid);
                else
                    sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn);


                ModalUtility.AssignCommandParameter(sqlCmd, cmdParams);

                sqlCmd.ExecuteNonQuery();

                return sqlCmd;
            }
            finally
            {
                _conn.Close();
            }
        }

        protected object executeProcedure(string storedProcedure, params object[] cmdParams)
        {
            return executeSqlCommand(storedProcedure, cmdParams).Parameters["@RETURN_VALUE"].Value;
        }


        protected SqlCommand executeSqlCommand(string storedProcedure)
        {
            try
            {
                _conn.Open();

                SqlCommand sqlCmd;

                if (_useUID)
                    sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn, _uid);
                else
                    sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn);


                sqlCmd.ExecuteNonQuery();

                return sqlCmd;
            }
            finally
            {
                _conn.Close();
            }
        }

        protected object executeProcedure(string storedProcedure)
        {
            return executeSqlCommand(storedProcedure).Parameters["@RETURN_VALUE"].Value;
        }

        protected SqlCommand executeSqlCommand(string storedProcedure, DataRow row)
        {
            try
            {
                _conn.Open();
                SqlCommand sqlCmd;

                if (_useUID)
                    sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn, _uid);
                else
                    sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcedure, _conn);

                ModalUtility.AssignCommandParameter(sqlCmd, row);

                sqlCmd.ExecuteNonQuery();

                return sqlCmd;
            }
            finally
            {
                _conn.Close();
            }
        }

        protected object executeProcedure(string storedProcedure, DataRow row)
        {
            return executeSqlCommand(storedProcedure, row).Parameters["@RETURN_VALUE"].Value;
        }


        public DataSet FillSqlDataSet(string storedProcName, params object[] paramValue)
        {
            return this.FillSqlDataSet(null, null, storedProcName, paramValue);
        }


        private DataSet fillDataSet(DataSet ds, string tableName, SqlCommand sqlCmd, params object[] paramValue)
        {
            if (ds == null)
                ds = new DataSet();

            for (int i = 0; i < sqlCmd.Parameters.Count - 1 && i < paramValue.Length; i++)
            {
                sqlCmd.Parameters[i + 1].Value = paramValue[i];
            }

            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
            {
                if (null != tableName)
                {
                    da.Fill(ds, tableName);
                }
                else
                {
                    da.Fill(ds);
                }
            }

            return ds;

        }

        protected DataSet fillXmlDataSet(DataSet ds, string storedProcName, params object[] paramValue)
        {
            if (ds == null)
                ds = new DataSet();

            try
            {
                _conn.Open();
                SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcName, _conn);

                for (int i = 0; i < sqlCmd.Parameters.Count - 1 && i < paramValue.Length; i++)
                {
                    sqlCmd.Parameters[i + 1].Value = paramValue[i];
                }

                object obj = sqlCmd.ExecuteScalar();
                if (obj != DBNull.Value)
                {
                    StringReader sr = new StringReader((string)obj);
                    ds.ReadXml(sr, XmlReadMode.IgnoreSchema);
                    sr.Close();
                }

                return ds;
            }
            finally
            {
                _conn.Close();
            }
        }


        protected DataSet fillSqlDataSet(DataSet ds, string tableName, SqlCommand sqlCmd, params object[] paramValue)
        {

            try
            {
                this._conn.Open();
                sqlCmd.Connection = this._conn;

                return this.fillDataSet(ds, tableName, sqlCmd, paramValue);

            }
            finally
            {
                this._conn.Close();
            }

        }


        protected DataSet fillSqlDataSet(DataSet ds, string tableName, string storedProcName, out SqlCommand sqlCmd, params object[] paramValue)
        {
            try
            {
                this._conn.Open();
                sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcName, this._conn);

                return this.fillDataSet(ds, tableName, sqlCmd, paramValue);

            }
            finally
            {
                this._conn.Close();
            }

        }

        public DataSet FillSqlDataSet(DataSet ds, string tableName, string storedProcName, params object[] paramValue)
        {
            SqlCommand sqlCmd;
            return this.fillSqlDataSet(ds, tableName, storedProcName, out sqlCmd, paramValue);
        }



        public DataSet FillSqlDataSet(string sqlCmd)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd, this._conn))
            {
                da.Fill(ds);
            }
            return ds;
        }


        public DataSet FillSqlDataSet(string storedProcName, IDictionary paramValue)
        {
            return this.FillSqlDataSet(null, null, storedProcName, paramValue);
        }

        public DataSet FillSqlDataSet(DataSet ds, string tableName, string storedProcName, IDictionary paramValue, int startRecord, int maxRecords, out int recordCount)
        {
            SqlCommand sqlCmd;
            ds = this.fillSqlDataSet(ds, tableName, storedProcName, out sqlCmd, paramValue, startRecord, maxRecords);
            recordCount = (int)sqlCmd.Parameters["@RecordCount"].Value;
            return ds;
        }

        public DataSet FillSqlDataSet(string storedProcName, IDictionary paramValue, int startRecord, int maxRecords, out int recordCount)
        {
            return FillSqlDataSet(null, null, storedProcName, paramValue, startRecord, maxRecords, out recordCount);
        }

        protected DataSet fillSqlDataSet(DataSet ds, string tableName, SqlCommand sqlCmd, IDictionary paramValue)
        {
            if (ds == null)
                ds = new DataSet();

            try
            {
                this._conn.Open();
                sqlCmd.Connection = this._conn;
                ModalUtility.AssignCommandParameter(sqlCmd, paramValue);

                using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                {
                    if (null != tableName)
                    {
                        da.Fill(ds, tableName);
                    }
                    else
                    {
                        da.Fill(ds);
                    }
                }
            }
            finally
            {
                this._conn.Close();
            }

            return ds;

        }


        protected DataSet fillSqlDataSet(DataSet ds, string tableName, string storedProcName, out SqlCommand sqlCmd, IDictionary paramValue)
        {
            if (ds == null)
                ds = new DataSet();

            try
            {
                this._conn.Open();
                sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcName, this._conn);
                ModalUtility.AssignCommandParameter(sqlCmd, paramValue);

                using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                {
                    if (null != tableName)
                    {
                        da.Fill(ds, tableName);
                    }
                    else
                    {
                        da.Fill(ds);
                    }
                }
            }
            finally
            {
                this._conn.Close();
            }

            return ds;

        }


        protected DataSet fillSqlDataSet(DataSet ds, string tableName, string storedProcName, out SqlCommand sqlCmd, IDictionary paramValue, int startRecord, int maxRecords)
        {
            if (ds == null)
                ds = new DataSet();

            try
            {
                this._conn.Open();
                sqlCmd = ModalUtility.InvokeStoredProcedure(storedProcName, this._conn, this._orderBy);

                if (paramValue != null)
                    ModalUtility.AssignCommandParameter(sqlCmd, paramValue);

                using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                {
                    if (null != tableName)
                    {
                        da.Fill(ds, startRecord, maxRecords, tableName);
                    }
                    else
                    {
                        da.Fill(ds, startRecord, maxRecords, "Table");
                    }
                }
            }
            finally
            {
                this._conn.Close();
            }

            return ds;

        }


        public DataSet FillSqlDataSet(DataSet ds, string tableName, string storedProcName, IDictionary paramValue)
        {
            SqlCommand sqlCmd;
            return this.fillSqlDataSet(ds, tableName, storedProcName, out sqlCmd, paramValue);
        }

        public DataSet FillSqlDataSet(DataSet ds, string sqlCmd, string tableName)
        {
            if (ds == null)
            {
                ds = new DataSet();
            }

            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd, this._conn))
            {
                da.Fill(ds, tableName);
            }
            return ds;
        }

        public DataSet FillSqlDataSet(string sqlCmd, out int recordCount)
        {
            DataSet ds = FillSqlDataSet(sqlCmd);

            if (ds.Tables.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
            }
            else
            {
                recordCount = 0;
            }

            return ds;
        }

        public void Commit()
        {
            if (_sqlTran != null)
            {
                _sqlTran.Commit();
            }
        }

        public void Rollback()
        {
            if (_sqlTran != null)
            {
                _sqlTran.Rollback();
            }
        }


    }
}
