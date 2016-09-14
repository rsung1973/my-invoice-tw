using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Model.Resource
{
	/// <summary>
	/// Summary description for ResourceConfigHandler.
	/// </summary>
	public class ResourceConfigHandler : System.Configuration.IConfigurationSectionHandler
	{
		public ResourceConfigHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region IConfigurationSectionHandler Members

		public object Create(object parent, object configContext, XmlNode section)
		{
			// TODO:  Add ResourceConfigHandler.Create implementation
			XmlDocument xmlDoc = new XmlDocument();
			
			XmlNode node = xmlDoc.ImportNode(section,true);
			xmlDoc.AppendChild(node);
			return xmlDoc;
		}

		#endregion

		public void test()
		{
		}
	}
}
