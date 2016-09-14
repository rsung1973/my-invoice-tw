using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Collections;

namespace Utility
{
	/// <summary>
	/// Summary description for LCFileParser.
	/// </summary>
	public class TextToXml
	{
		private DataSet _dsFormat;
		private string _formatFile;
		private string[] _tableName;
		private ArrayList fieldChain;
		private XmlDocument xmlUpload;
		private XmlElement xmlRoot;
		private int _fieldIndex;

		public TextToXml(string formatFile,string tableName) : this(formatFile,new string[]{tableName})
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public TextToXml(string formatFile,params string[] tableName)
		{
			//
			// TODO: Add constructor logic here
			//
			_formatFile = formatFile;
			_tableName = tableName;

			initialize();
		}


		private void initialize()
		{
			_dsFormat = new DataSet();
			for(int i=0;i<_tableName.Length;i++)
			{
				DataTable table = _dsFormat.Tables.Add(_tableName[i]);
				table.Columns.Add("columnName",typeof(String));
				table.Columns.Add("columnLength",typeof(int));
				DataColumn column = new DataColumn("repeat",typeof(bool));
				column.DefaultValue = false;
				column.AllowDBNull = false;
				table.Columns.Add(column);
			}

			_dsFormat.ReadXml(_formatFile);

			fieldChain = new ArrayList();
			initializeFieldChain(_dsFormat.Tables[0]);

			xmlUpload = new XmlDocument();
			xmlRoot = xmlUpload.CreateElement("Root");
			xmlUpload.AppendChild(xmlRoot);

		}

		private void initializeFieldChain(DataTable table)
		{
			foreach(DataRow row in table.Rows)
			{
				fieldChain.Add(row);
				if(_dsFormat.Tables.Contains((string)row["columnName"]))
				{
					initializeFieldChain(_dsFormat.Tables[(string)row["columnName"]]);
				}
			}
		}

		private bool parse(DataTable table,XmlNode currentNode,byte[] buf,ref int startIndex)
		{

			System.Text.Encoding encoding = System.Text.Encoding.Default;
			bool isNewItem = true;

			if(startIndex<buf.Length)
			{
				XmlNode itemNode = xmlUpload.CreateElement(table.TableName);

				while(_fieldIndex<fieldChain.Count && isNewItem)
				{
					DataRow row = (DataRow)fieldChain[_fieldIndex];

					if(row.Table==table)
					{
						_fieldIndex++;
					}
					else
					{
						//同一table的資料到此結束了.
						break;
					}

					string columnName = (string)row["columnName"];
					int columnLength = (int)row["columnLength"];


					//檢查是否為巢狀子table
					if(_dsFormat.Tables.Contains(columnName))
					{
						//上傳檔format中定義此欄位為另一個巢狀子table,
						//繼續剖析接下來的子table.
						isNewItem = parse(_dsFormat.Tables[columnName],itemNode,buf,ref startIndex);
					}
					else
					{
						//單純的column資料
						if(!(bool)row["repeat"])
						{
							if(startIndex+columnLength<=buf.Length)
							{
								XmlNode uploadNode = xmlUpload.CreateElement(columnName);
								itemNode.AppendChild(uploadNode);

								XmlNode valueNode = xmlUpload.CreateTextNode(
									new String(encoding.GetChars(buf,startIndex,columnLength)).Trim());
								startIndex += columnLength;
								uploadNode.AppendChild(valueNode);
							}
							else
							{
								//資料長度錯誤,資料與格式不符
								startIndex += columnLength;
								isNewItem = false;
								break;
							}
						}
						else
						{
							while(startIndex+columnLength<=buf.Length)
							{
								XmlNode uploadNode = xmlUpload.CreateElement(columnName);
								itemNode.AppendChild(uploadNode);

								XmlNode valueNode = xmlUpload.CreateTextNode(
									new String(encoding.GetChars(buf,startIndex,columnLength)).Trim());
								startIndex += columnLength;
								uploadNode.AppendChild(valueNode);

								currentNode.AppendChild(itemNode);
								itemNode = xmlUpload.CreateElement(table.TableName);
							}

							itemNode = null;
						}
					}

				}

				if(isNewItem && itemNode!=null)
				{
					currentNode.AppendChild(itemNode);
				}
			}

			return isNewItem;

		}

		public static byte[] nextLine(Stream stream)
		{
			int byteRead = stream.ReadByte();

			//end of file 回傳null
			if(byteRead==-1)
			{
				return null;
			}

            using (MemoryStream ms = new MemoryStream())
            {
                while (byteRead != -1)
                {
                    if (byteRead == 0x0A)
                    {
                        break;
                    }
                    else if (byteRead == 0x0D)
                    {
                        //skip
                    }
                    else if (byteRead == 0x00)
                    {
                        ms.WriteByte(0x20);
                    }
                    else
                    {
                        ms.WriteByte((byte)byteRead);
                    }
                    byteRead = stream.ReadByte();
                }
                return ms.ToArray();
            }
		}

		public string parse(Stream stream)
		{

			byte[] buf=nextLine(stream);
			while(buf!=null && buf.Length>0)
			{
				int startIndex = 0;
				_fieldIndex = 0;
				parse(_dsFormat.Tables[0],xmlRoot,buf,ref startIndex);
				buf=nextLine(stream);
			}


            string xmlFile = String.Format("{0}\\{1}.xml", CurrentStorePath, System.Guid.NewGuid().ToString());
			xmlUpload.Save(xmlFile);
			return xmlFile;
		}


		public string CurrentStorePath
		{
			get
			{
                string xmlPath = Path.Combine(UtilityConfig.LogPath, DateTime.Now.ToString("yyyy/MM/dd").Replace('/', '\\'));
                if (!Directory.Exists(xmlPath))
				{
					Directory.CreateDirectory(xmlPath);
				}
				return xmlPath;
			}
		}
	}

			
}
