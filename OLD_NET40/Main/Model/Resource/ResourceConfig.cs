using System;
using System.Xml;
using System.Collections.Specialized;
using System.Configuration;

namespace Model.Resource
{
	/// <summary>
	/// Summary description for DBConfig.
	/// </summary>
	public class ResourceConfig
	{
		private static ResourceConfig _resourceConfig;

		static ResourceConfig()
		{
			//Config.StartUp();
		}

		private string _homePage;

		private ResourceConfig(XmlNode node)
		{
			//
			// TODO: Add constructor logic here
			//
			if(null!=node)
				initialize(node);
		}

		private void initialize(XmlNode node)
		{
			XmlNode valueNode ;
			valueNode = node.SelectSingleNode("homePage/text()");
			if(valueNode!=null)
				_homePage = valueNode.Value;

            //XmlNodeList nodeList = node.SelectNodes("lcUploadStatus/item");
            //if(nodeList.Count>0)
            //{
            //    _lcUploadStatus = new string[nodeList.Count];
            //    for(int i=0;i<nodeList.Count;i++)
            //    {
            //        string xpath = String.Format("lcUploadStatus/item[@no={0}]/text()",i);
            //        valueNode = node.SelectSingleNode(xpath);
            //        if(valueNode!=null)
            //        {
            //            _lcUploadStatus[i] = valueNode.Value;
            //        }
            //    }
            //}

            //nodeList = node.SelectNodes("draftUploadStatus/item");
            //if(nodeList.Count>0)
            //{
            //    _draftUploadStatus = new string[nodeList.Count];
            //    for(int i=0;i<nodeList.Count;i++)
            //    {
            //        string xpath = String.Format("draftUploadStatus/item[@no={0}]/text()",i);
            //        valueNode = node.SelectSingleNode(xpath);
            //        if(valueNode!=null)
            //        {
            //            _draftUploadStatus[i] = valueNode.Value;
            //        }
            //    }
            //}

            //nodeList = node.SelectNodes("invoiceUploadStatus/item");
            //if(nodeList.Count>0)
            //{
            //    _invoiceUploadStatus = new string[nodeList.Count];
            //    for(int i=0;i<nodeList.Count;i++)
            //    {
            //        string xpath = String.Format("invoiceUploadStatus/item[@no={0}]/text()",i);
            //        valueNode = node.SelectSingleNode(xpath);
            //        if(valueNode!=null)
            //        {
            //            _invoiceUploadStatus[i] = valueNode.Value;
            //        }
            //    }
            //}


		}

        //public static string[] InvoiceUploadStatus
        //{
        //    get
        //    {
        //        return _resourceConfig._invoiceUploadStatus;
        //    }
        //}

        //public static string[] DraftUploadStatus
        //{
        //    get
        //    {
        //        return _resourceConfig._draftUploadStatus;
        //    }
        //}

        //public static string[] LCUploadStatus
        //{
        //    get
        //    {
        //        return _resourceConfig._lcUploadStatus;
        //    }
        //}

		public static void CreateInstance(XmlNode node)
		{
			_resourceConfig = new ResourceConfig(node);
		}

		public static string HomePage
		{
			get
			{
				return _resourceConfig._homePage;
			}
		}


	}
}
