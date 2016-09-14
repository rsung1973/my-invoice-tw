using System;
using System.Data.OleDb;
using System.Data;
using System.Collections;
using System.IO;
using System.Web.Configuration;
using System.Configuration;


namespace DataAccessLayer.basis
{
	/// <summary>
	/// </summary>
	public class OleDbLayerBase : System.IDisposable
	{
        internal static ConnectionStringSettings _ConnectionString;

		private bool _bDisposed = false;

		protected OleDbConnection _conn;
		protected object _uid;
		protected bool _useUID = false;
		protected string _orderBy;
		protected OleDbTransaction _sqlTran;

        static OleDbLayerBase()
        {
            _ConnectionString = WebConfigurationManager.ConnectionStrings["masterOleDB"];
        }

		public OleDbLayerBase()
		{
			//
			// TODO: Add constructor logic here
			//
			_conn = new OleDbConnection();
            _conn.ConnectionString = _ConnectionString.ConnectionString;
        }

        public OleDbLayerBase(String connectionString)
        {
            //
            // TODO: Add constructor logic here
            //
            _conn = new OleDbConnection();
            _conn.ConnectionString = connectionString;
        }


        ~OleDbLayerBase()
		{
			Dispose();
		}

		public void Dispose()
		{
			if(!_bDisposed)
			{
				_bDisposed = true;
				if(_conn.State == System.Data.ConnectionState.Open )
				{
					_conn.Close ();
				}
			}
			else
				return;
		}

		public OleDbConnection Connection
		{
			get
			{
				return _conn;
			}
		}

		protected DataSet dumpSchema(string sqlCmd)
		{
			DataSet ds = new DataSet();
			using(OleDbDataAdapter da = new OleDbDataAdapter(sqlCmd,this._conn))
			{
				da.FillSchema(ds,SchemaType.Source);
			}
			return ds;
		}

		protected DataSet dumpSchema(OleDbCommand sqlCmd)
		{
			DataSet ds = new DataSet();
			using(OleDbDataAdapter da = new OleDbDataAdapter(sqlCmd))
			{
				da.FillSchema(ds,SchemaType.Source);
			}
			return ds;
		}

		protected int contains(string tableName,string columnName , object columnValue)
		{
			//
			// TODO: 到DB中讀取UserProfile資料
			//

			try
			{
				_conn.Open();
				OleDbCommand sqlCmd = new OleDbCommand(String.Format("select count(*) from {0} where {1} = @ParamValue ",
					tableName,columnName),_conn);
				sqlCmd.CommandType = CommandType.StoredProcedure;

				sqlCmd.Parameters.AddWithValue("@ParamValue",columnValue);

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
				OleDbCommand sqlCmd = new OleDbCommand(cmdText,_conn);
				sqlCmd.ExecuteNonQuery();
			}
			finally
			{
				_conn.Close();
			}
		}

		protected void executeOleDbCommand(OleDbCommand sqlCmd)
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


		protected OleDbCommand executeOleDbCommand(string storedProcedure,string[] paramName,object[] paramValue)
		{
			try
			{
				_conn.Open();

				OleDbCommand sqlCmd;

				if(_useUID)
					sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure,_conn,_uid);
				else
					sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure,_conn);


				for(int i=0;i<paramName.Length;i++)
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

		protected object executeProcedure(string storedProcedure,string[] paramName,object[] paramValue)
		{
			return executeOleDbCommand(storedProcedure,paramName,paramValue).Parameters["@RETURN_VALUE"].Value;
		}


		protected OleDbCommand executeOleDbCommand(string storedProcedure,IDictionary paramValue)
		{

			try
			{
				_conn.Open();

				OleDbCommand sqlCmd;

				if(_useUID)
					sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure,_conn,_uid);
				else
					sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure,_conn);

				OleDbModalUtility.AssignCommandParameter(sqlCmd,paramValue);

				sqlCmd.ExecuteNonQuery();
				return sqlCmd;
			}
			finally
			{
				_conn.Close();
			}
		}

        protected OleDbDataReader executeReader(string storedProcedure, IDictionary paramValue)
        {
            _conn.Open();

            OleDbCommand sqlCmd;

            if (_useUID)
                sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure, _conn, _uid);
            else
                sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure, _conn);

            OleDbModalUtility.AssignCommandParameter(sqlCmd, paramValue);

            return sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }


		protected object executeProcedure(string storedProcName,IDictionary paramValue)
		{
			return	executeOleDbCommand(storedProcName,paramValue).Parameters["@RETURN_VALUE"].Value;
		}


		protected OleDbCommand executeOleDbCommand(string storedProcedure,params object[] cmdParams)
		{
			try
			{
				_conn.Open();
				OleDbCommand sqlCmd;

				if(_useUID)
					sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure,_conn,_uid);
				else
					sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure,_conn);


				OleDbModalUtility.AssignCommandParameter(sqlCmd,cmdParams);

				sqlCmd.ExecuteNonQuery();

				return sqlCmd;
			}
			finally
			{
				_conn.Close();
			}
		}

		protected object executeProcedure(string storedProcedure,params object[] cmdParams)
		{
			return executeOleDbCommand(storedProcedure,cmdParams).Parameters["@RETURN_VALUE"].Value;
		}


		protected OleDbCommand executeOleDbCommand(string storedProcedure)
		{
			try
			{
				_conn.Open();

				OleDbCommand sqlCmd;

				if(_useUID)
					sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure,_conn,_uid);
				else
					sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure,_conn);


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
			return executeOleDbCommand(storedProcedure).Parameters["@RETURN_VALUE"].Value;
		}

		protected OleDbCommand executeOleDbCommand(string storedProcedure,DataRow row)
		{
			try
			{
				_conn.Open();
				OleDbCommand sqlCmd;

				if(_useUID)
					sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure,_conn,_uid);
				else
					sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcedure,_conn);

				OleDbModalUtility.AssignCommandParameter(sqlCmd,row);

				sqlCmd.ExecuteNonQuery();

				return sqlCmd;
			}
			finally
			{
				_conn.Close();
			}
		}

		protected object executeProcedure(string storedProcedure,DataRow row)
		{
			return executeOleDbCommand(storedProcedure,row).Parameters["@RETURN_VALUE"].Value;
		}


		public DataSet FillSqlDataSet(string storedProcName,params object[] paramValue)
		{
			return this.FillSqlDataSet(null,null,storedProcName,paramValue);
		}


		private DataSet fillDataSet(DataSet ds,string tableName,OleDbCommand sqlCmd,params object[] paramValue)
		{
			if(ds==null)
				ds = new DataSet();

			for(int i=0;i<sqlCmd.Parameters.Count-1 && i<paramValue.Length;i++)
			{
				sqlCmd.Parameters[i+1].Value = paramValue[i];
			}
	
			using (OleDbDataAdapter da = new OleDbDataAdapter(sqlCmd))
			{
				if(null!=tableName)
				{
					da.Fill(ds,tableName);
				}
				else
				{
					da.Fill(ds);
				}
			}

			return ds;

		}

		protected DataSet fillXmlDataSet(DataSet ds,string storedProcName,params object[] paramValue)
		{
			if(ds==null)
				ds = new DataSet();

			try
			{
				_conn.Open();
				OleDbCommand sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcName,_conn);

				for(int i=0;i<sqlCmd.Parameters.Count-1 && i<paramValue.Length;i++)
				{
					sqlCmd.Parameters[i+1].Value = paramValue[i];
				}

				object obj = sqlCmd.ExecuteScalar();
				if(obj!=DBNull.Value)
				{
					StringReader sr = new StringReader((string)obj);
					ds.ReadXml(sr,XmlReadMode.IgnoreSchema);
					sr.Close();
				}

				return ds;
			}
			finally
			{
				_conn.Close();
			}
		}


		protected DataSet fillSqlDataSet(DataSet ds,string tableName,OleDbCommand sqlCmd,params object[] paramValue)
		{

			try
			{
				this._conn.Open();
				sqlCmd.Connection = this._conn;

				return this.fillDataSet(ds,tableName,sqlCmd,paramValue);

			}
			finally
			{
				this._conn.Close();
			}

		}


		protected DataSet fillSqlDataSet(DataSet ds,string tableName,string storedProcName,out OleDbCommand sqlCmd,params object[] paramValue)
		{
			try
			{
				this._conn.Open();
				sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcName,this._conn);

				return this.fillDataSet(ds,tableName,sqlCmd,paramValue);

			}
			finally
			{
				this._conn.Close();
			}

		}

		public DataSet FillSqlDataSet(DataSet ds,string tableName,string storedProcName,params object[] paramValue)
		{
			OleDbCommand sqlCmd;
			return this.fillSqlDataSet(ds,tableName,storedProcName,out sqlCmd,paramValue);
		}



		public DataSet FillSqlDataSet(string sqlCmd)
		{
			DataSet ds = new DataSet();
			using(OleDbDataAdapter da = new OleDbDataAdapter(sqlCmd,this._conn))
			{
				da.Fill(ds);
			}
			return ds;
		}


		public DataSet FillSqlDataSet(string storedProcName,IDictionary paramValue)
		{
			return this.FillSqlDataSet(null,null,storedProcName,paramValue);
		}

		public DataSet FillSqlDataSet(DataSet ds,string tableName,string storedProcName,IDictionary paramValue,int startRecord,int maxRecords,out int recordCount)
		{
			OleDbCommand sqlCmd;
			ds = this.fillSqlDataSet(ds,tableName,storedProcName,out sqlCmd,paramValue,startRecord,maxRecords);
			recordCount = (int)sqlCmd.Parameters["@RecordCount"].Value;
			return ds;
		}

		public DataSet FillSqlDataSet(string storedProcName,IDictionary paramValue,int startRecord,int maxRecords,out int recordCount)
		{
			return FillSqlDataSet(null,null,storedProcName,paramValue,startRecord,maxRecords,out recordCount);
		}

		protected DataSet fillSqlDataSet(DataSet ds,string tableName,OleDbCommand sqlCmd,IDictionary paramValue)
		{
			if(ds==null)
				ds = new DataSet();

			try
			{
				this._conn.Open();
				sqlCmd.Connection = this._conn;
				OleDbModalUtility.AssignCommandParameter(sqlCmd,paramValue);
	
				using (OleDbDataAdapter da = new OleDbDataAdapter(sqlCmd))
				{
					if(null!=tableName)
					{
						da.Fill(ds,tableName);
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


		protected DataSet fillSqlDataSet(DataSet ds,string tableName,string storedProcName,out OleDbCommand sqlCmd,IDictionary paramValue)
		{
			if(ds==null)
				ds = new DataSet();

			try
			{
				this._conn.Open();
				sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcName,this._conn);
				OleDbModalUtility.AssignCommandParameter(sqlCmd,paramValue);
	
				using (OleDbDataAdapter da = new OleDbDataAdapter(sqlCmd))
				{
					if(null!=tableName)
					{
						da.Fill(ds,tableName);
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


		protected DataSet fillSqlDataSet(DataSet ds,string tableName,string storedProcName,out OleDbCommand sqlCmd,IDictionary paramValue,int startRecord,int maxRecords)
		{
			if(ds==null)
				ds = new DataSet();

			try
			{
				this._conn.Open();
				sqlCmd = OleDbModalUtility.InvokeStoredProcedure(storedProcName,this._conn,this._orderBy);

				if(paramValue!=null)
					OleDbModalUtility.AssignCommandParameter(sqlCmd,paramValue);
	
				using (OleDbDataAdapter da = new OleDbDataAdapter(sqlCmd))
				{
					if(null!=tableName)
					{
						da.Fill(ds,startRecord,maxRecords,tableName);
					}
					else
					{
						da.Fill(ds,startRecord,maxRecords,"Table");
					}
				}
			}
			finally
			{
				this._conn.Close();
			}

			return ds;

		}


		public DataSet FillSqlDataSet(DataSet ds,string tableName,string storedProcName,IDictionary paramValue)
		{
			OleDbCommand sqlCmd;
			return this.fillSqlDataSet(ds,tableName,storedProcName,out sqlCmd,paramValue);
		}

		public DataSet FillSqlDataSet(DataSet ds,string sqlCmd,string tableName)
		{
			if(ds==null)
			{
				ds = new DataSet();
			}

			using(OleDbDataAdapter da = new OleDbDataAdapter(sqlCmd,this._conn))
			{
                if (String.IsNullOrEmpty(tableName))
                {
                    da.Fill(ds);
                }
                else
                {
                    da.Fill(ds, tableName);
                }
			}
			return ds;
		}

		public DataSet FillSqlDataSet(string sqlCmd,out int recordCount)
		{
			DataSet ds = FillSqlDataSet(sqlCmd);

			if(ds.Tables.Count>0)
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
			if(_sqlTran!=null)
			{
				_sqlTran.Commit();
			}
		}

		public void Rollback()
		{
			if(_sqlTran!=null)
			{
				_sqlTran.Rollback();
			}
		}


	}
}
