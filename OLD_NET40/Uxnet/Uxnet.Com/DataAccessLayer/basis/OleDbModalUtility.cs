using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

using Document = System.Xml.XmlDocument;
using Element = System.Xml.XmlElement;
using Node = System.Xml.XmlNode;
using OperationFailureException = System.Exception;


namespace DataAccessLayer.basis
{
	/// <summary>
	/// OleDbModalUtility 的摘要描述。
	/// </summary>
	public class OleDbModalUtility
	{
		private static OleDbModalUtility modalUtil = new OleDbModalUtility();

		private OleDbModalUtility()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

		public static string GetFieldStringValue(object obj)
		{
			return (obj!=DBNull.Value)?(string)obj:null;
		}

		public static string GetFieldDateValue(object obj)
		{
			return (obj!=DBNull.Value)?((System.DateTime)obj).ToShortDateString():null;
		}

		public static string GetFieldNumericStringValue(object obj)
		{
			return (obj!=DBNull.Value)?System.Convert.ToString(obj):null;
		}


		public static OleDbCommand InvokeStoredProcedure(string spName,OleDbConnection conn)
		{
			OleDbCommand sqlCmd = new OleDbCommand(spName,conn);
			sqlCmd.CommandType = CommandType.StoredProcedure;
			OleDbCommandBuilder.DeriveParameters(sqlCmd);
			sqlCmd.CommandTimeout = 86400;
			return sqlCmd;
		}


		public static OleDbCommand InvokeStoredProcedure(string spName,OleDbConnection conn,object uid)
		{
			OleDbCommand sqlCmd = InvokeStoredProcedure(spName,conn);
			sqlCmd.Parameters["UID"].Value = uid;
			return sqlCmd;
		}

		public static OleDbCommand InvokeStoredProcedure(string spName,OleDbConnection conn,string orderBy)
		{
			OleDbCommand sqlCmd = InvokeStoredProcedure(spName,conn);
			if(sqlCmd.Parameters.Contains("OrderBy") && orderBy!=null)
                sqlCmd.Parameters["OrderBy"].Value = orderBy;
			return sqlCmd;
		}




		public static void AssignCommandParameter(OleDbCommand sqlCmd,DataRow row)
		{
			OleDbParameterCollection cmdParams = sqlCmd.Parameters;
			DataColumnCollection columns = row.Table.Columns;

			string paramName;

			for(int i=0;i<columns.Count;i++)
			{
				paramName = columns[i].ColumnName;
				if(cmdParams.Contains(paramName))
				{
					cmdParams[paramName].Value = row[i];
				}
			}
		}

		public static void AssignCommandParameter(OleDbCommand sqlCmd,object[] param)
		{
			OleDbParameterCollection cmdParams = sqlCmd.Parameters;
			for(int i=0;i<cmdParams.Count && i<param.Length;i++)
			{
				cmdParams[i].Value = param[i];
			}
		}


		public static void AssignCommandParameter(OleDbCommand sqlCmd,IDataReader dr)
		{
			OleDbParameterCollection cmdParams = sqlCmd.Parameters;
			int fieldCount = dr.FieldCount;

			string paramName;

			for(int i=0;i<fieldCount;i++)
			{
				paramName = dr.GetName(i);
				if(cmdParams.Contains(paramName))
				{
					cmdParams[paramName].Value = dr[i];
				}
			}
		}

		public static DataRow ConvertToDataRow(DataTable table,System.Collections.Specialized.NameValueCollection item)
		{
			DataRow row = null;
			if(table!=null)
			{
				DataColumnCollection columns = table.Columns;
				row = table.NewRow();

				for(int i=0;i<item.Count;i++)
				{
					string name = item.GetKey(i);
					if(columns.Contains(name) && item[i].Length>0)
					{
						row[name] = Convert.ChangeType(item[i],columns[name].DataType);
					}
				}
			}
			return row;
		}

		public static DataRow ConvertToDataRow(DataTable table,ControlCollection controls)
		{
			DataRow row = null;
			if(table!=null)
			{
				DataColumnCollection columns = table.Columns;
				row = table.NewRow();
				string assignValue;

				foreach(Control control in controls)
				{
					if(control.ID!=null && columns.Contains(control.ID))
					{
						if(control is TextBox)
						{
							assignValue = ((TextBox)control).Text;
							row[control.ID] = Convert.ChangeType(assignValue,columns[control.ID].DataType);
						}
						else if(control is HtmlInputText)
						{
							assignValue = ((HtmlInputText)control).Value;
							row[control.ID] = Convert.ChangeType(assignValue,columns[control.ID].DataType);
						}
					}
				}
			}
			return row;
		}



		public static XmlNode CreateResultSet(XmlDocument document,string rootName)
		{
			if (document == null) 
			{
				document = new XmlDocument();
				document.AppendChild(document.CreateElement("Root"));
			}

			if (rootName == null || rootName.Length == 0)
				rootName = "ResultSet";

			Element root = document.DocumentElement;
			Element table = document.CreateElement("Table");
			table.SetAttribute("name", rootName);
			root.AppendChild(table);
			return table;

		}

		public static XmlDocument AppendResultSet(XmlNode table,IDataReader dr)
		{
			XmlDocument document = table.OwnerDocument;

			if(dr.Read())
			{
				int i;
				String[] columnName = new string[dr.FieldCount];
				for(i=0;i<dr.FieldCount;i++)
					columnName[i] = dr.GetName(i);

				do 
				{
					Element elmt = document.CreateElement("item");
					for (i = 0; i <dr.FieldCount; i++) 
					{
						Element field = document.CreateElement(columnName[i]);
						if (!dr.IsDBNull(i)) 
						{
							if(dr[i] is System.DateTime)
							{
								field.AppendChild(document.CreateTextNode(String.Format("{0:yyyy/M/d}",dr[i])));
							}
							else
							{
								field.AppendChild(document.CreateTextNode(dr[i].ToString().Replace('\0',' ')));
							}
						}
						else 
						{
							field.AppendChild(document.CreateTextNode(""));
						}
						elmt.AppendChild(field);
					}
					table.AppendChild(elmt);
				} while(dr.Read());
			}
			return document;

		}


		public static XmlDocument AppendResultSet(XmlDocument document,IDataReader dr,string rootName)
		{
			XmlNode table = CreateResultSet(document,rootName);
			AppendResultSet(table,dr);
			return table.OwnerDocument;
		}

		public static void AssignCommandParameter(OleDbCommand sqlCmd,IDictionary param)
		{
			OleDbParameterCollection cmdParams = sqlCmd.Parameters;
			string paramName;
			foreach(object key in param.Keys)
			{
				paramName = key.ToString();
				if(cmdParams.Contains(paramName))
				{
					cmdParams[paramName].Value = param[key];
				}
			}
		}

		public static XmlNode BuildXmlParam(string paramName,IList paramList)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("ROOT");
			doc.AppendChild(root);
			foreach(object param in paramList)
			{
				XmlElement elmt = doc.CreateElement(paramName);
				elmt.AppendChild(doc.CreateTextNode(param.ToString()));
				root.AppendChild(elmt);
			}

			return doc;

		}

		public static XmlNode BuildXmlParam(string paramName,DataRow[] rows,params string[] columnName)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("ROOT");
			doc.AppendChild(root);
			foreach(DataRow row in rows)
			{
				XmlElement elmt = doc.CreateElement(paramName);
				foreach(string name in columnName)
				{
					XmlElement param = doc.CreateElement(name);
					param.AppendChild(doc.CreateTextNode(row[name].ToString()));
					elmt.AppendChild(param);
					
				}
				root.AppendChild(elmt);
			}

			return doc;

		}


		#region HasAtLeastOneRow
		public static bool HasAtLeastOneRow(DataSet ds)
		{
			return (ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0);
		}

		#endregion




	}
}
