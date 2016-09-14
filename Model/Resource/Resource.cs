using System;
using System.IO;
using System.Reflection;

namespace Model.Resource
{
	/// <summary>
	/// Summary description for Resource.
	/// </summary>
	public class Resource
	{
		private static Resource _resource = new Resource();

		private Resource()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public Stream GetResourceAsStream(string name)
		{
//			string []str = Assembly.GetExecutingAssembly().GetManifestResourceNames(); 
//			System.Reflection.ManifestResourceInfo info =  Assembly.GetExecutingAssembly().GetManifestResourceInfo("Model.Resource.xsd.Inbox.xsd");
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType().Namespace + ".xsd." + name); 
		}

		public static Resource Instance
		{
			get
			{
				return _resource;
			}
		}
	}
}
