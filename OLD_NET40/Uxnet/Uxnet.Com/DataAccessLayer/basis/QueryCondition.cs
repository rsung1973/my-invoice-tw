using System;
using System.Xml.Xsl;
using Uxnet.Com.DataAccessLayer.basis;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;
using System.Data;
using System.IO;

namespace DataAccessLayer.basis
{
	/// <summary>
	/// QueryCondition 的摘要描述。
	/// </summary>
	public class QueryCondition
	{
        private static XslCompiledTransform _xslt;

		private string _operator;
		private object _left;
		private object _right;

        public QueryCondition(string xsltQueryCondition)
        {
            if (_xslt == null)
            {
                lock (typeof(QueryCondition))
                {
                    if (_xslt == null)
                    {
                        _xslt = new XslCompiledTransform();
                        _xslt.Load(xsltQueryCondition, XsltSettings.TrustedXslt, null);
                    }
                }
            }
        }

		public QueryCondition()
		{}

		public QueryCondition(string operatorStr,object leftOprand,object rightOperand)
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
			_operator = operatorStr;
			_left = leftOprand;
			_right = rightOperand;
		}

        public string PrepareQueryCondition(dsQueryCondition dsQuery)
        {

            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);

            XmlDataDocument document = new XmlDataDocument(dsQuery.Copy());

            _xslt.Transform(document.CreateNavigator(), xw);

            xw.Flush();
            xw.Close();

            //DataSet ds = new DataSet();
            //StringReader sr = new StringReader(sb.ToString());

            //ds.ReadXml(sr);
            //sr.Close();

            //QueryCondition queryCond = new QueryCondition();

            //if (ds.Tables.Count > 0)
            //{

            //    foreach (DataRow row in ds.Tables[0].Rows)
            //    {
            //        queryCond = queryCond.Add("And", row[0]);
            //    }
            //}

            Regex reg = new Regex("(?<tag><Condition>(?<value>[^<>]*)</Condition>)", RegexOptions.Singleline);
            Match m = reg.Match(sb.ToString());

            QueryCondition queryCond = new QueryCondition();

            while (m.Success)
            {
                queryCond = queryCond.Add("And", m.Groups["value"].Value);
                m = m.NextMatch();
            }

            return queryCond.ToString();

        }


		public QueryCondition Add(string operatorStr,object operand)
		{
			if(null==_left)
			{
				_left = operand;
				return this;
			}
			else if(null==_right)
			{
				_operator = operatorStr;
				_right = operand;
				return this;
			}
			else
			{
				QueryCondition cond = new QueryCondition(operatorStr,this,operand);
				return cond;
			}

		}

		public string OperatorString
		{
			get
			{
				return _operator;
			}
			set
			{
				_operator = value;
			}
		}

		public object LeftOperand
		{
			get
			{
				return _left;
			}
			set
			{
				_left = value;
			}

		}

		public object RightOperand
		{
			get
			{
				return _right;
			}
			set
			{
				_right = value;
			}
		}
		public override string ToString()
		{
			string l_str = (null!=_left)?_left.ToString():null;
			string r_str = (null!=_right)?_right.ToString():null;

			if(null!=r_str)
			{
				return new System.Text.StringBuilder(" ( ").Append(l_str).Append(" ")
					.Append(_operator).Append(" ").Append(r_str).Append(" ) ").ToString();
			}
			else if(null!=l_str)
			{
				return new System.Text.StringBuilder(" ").Append(l_str).Append(" ").ToString();

			}
			else
				return null;
		}


	}
}
