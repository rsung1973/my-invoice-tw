using System;
using System.Configuration.Provider;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Xml;

namespace Uxnet.Com.Providers
{

    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class UxnetSiteMapProvider : SiteMapProvider
    {
        private SiteMapProvider _parentSiteMapProvider = null;
        private string _providerName = null;
        private string _sourceFilename = null;
        private SiteMapNode _rootNode = null;
        private IDictionary _nodeKey = null;
        private XmlDocument _menuDoc = null;


        // A default constructor. The Name property is initialized in the
        // Initialize method.
        public UxnetSiteMapProvider()
        {
        }
        // Implement the CurrentNode property.
        public override SiteMapNode CurrentNode
        {
            get
            {
                return FindSiteMapNode(HttpContext.Current.Request.RawUrl);
            }
        }

        // Implement the RootNode property.
        public override SiteMapNode RootNode
        {
            get
            {
                return _rootNode;
            }
        }
        // Implement the ParentProvider property.
        public override SiteMapProvider ParentProvider
        {
            get
            {
                return _parentSiteMapProvider;
            }
            set
            {
                _parentSiteMapProvider = value;
            }
        }

        // Implement the RootProvider property.
        public override SiteMapProvider RootProvider
        {
            get
            {
                // If the current instance belongs to a provider hierarchy, it
                // cannot be the RootProvider. Rely on the ParentProvider.
                if (this.ParentProvider != null)
                {
                    return ParentProvider.RootProvider;
                }
                // If the current instance does not have a ParentProvider, it is
                // not a child in a hierarchy, and can be the RootProvider.
                else
                {
                    return this;
                }
            }
        }
        // Implement the FindSiteMapNode method.
        public override SiteMapNode FindSiteMapNode(string rawUrl)
        {

            // Does the root node match the URL?
            if (!String.IsNullOrEmpty(rawUrl))
            {
                string url = rawUrl.Split('?')[0];
                if (_nodeKey.Contains(url))
                {
                    return createSiteMapNode((XmlElement)_nodeKey[url]);
                }
            }
            return null;
        }

        // Implement the GetChildNodes method.
        public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
        {
            if (_nodeKey.Contains(node.Key))
            {
                XmlElement element = (XmlElement)_nodeKey[node.Key];

                if (element.HasChildNodes)
                {
                    SiteMapNodeCollection collection = new SiteMapNodeCollection();

                    foreach (XmlNode itemNode in element.ChildNodes)
                    {
                        if (itemNode is XmlElement)
                        {
                            XmlElement nodeElement = (XmlElement)itemNode;
                            if("menuItem".Equals(nodeElement.Name))
                                collection.Add(createSiteMapNode(nodeElement));
                        }
                    }

                    return collection;
                }
            }

            return null;

        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return _rootNode;
        }
        // Implement the GetParentNode method.
        public override SiteMapNode GetParentNode(SiteMapNode node)
        {
            return node.ParentNode;
        }

        // Implement the ProviderBase.Initialize property.
        // Initialize is used to initialize the state that the Provider holds, but
        // not actually build the site map.
        public override void Initialize(string name, NameValueCollection attributes)
        {

            lock (this)
            {
                if (_rootNode == null)
                {
                    base.Initialize(name, attributes);

                    _providerName = name;
                    _sourceFilename = attributes["siteMapFile"];
                    _nodeKey = new Hashtable();
                    _menuDoc = new XmlDocument();

                    // Build the site map in memory.
                    LoadSiteMapFromStore();
                }
            }
        }

        protected virtual void LoadSiteMapFromStore()
        {
            string pathToOpen;

            lock (this)
            {
                // If a root node exists, LoadSiteMapFromStore has already
                // been called, and the method can return.
                if (_rootNode != null)
                {
                    return;
                }
                else
                {
                    pathToOpen = HttpContext.Current.Server.MapPath(_sourceFilename);

                    if (File.Exists(pathToOpen))
                    {
                        // Open the file to read from.
                        _menuDoc.Load(pathToOpen);
                        _rootNode = createSiteMapNode(_menuDoc.DocumentElement);

                    }
                    else
                    {
                        throw new Exception("找不到功能選單設定檔:"+pathToOpen);
                    }
                }
            }
        }

        private SiteMapNode createSiteMapNode(XmlElement xmlElement)
        {
            String url = (xmlElement.Attributes["url"]!=null) ? xmlElement.Attributes["url"].Value : null;

            if (String.IsNullOrEmpty(url))
            {
                url = Guid.NewGuid().ToString();
            }
            else
            {
                if (url.StartsWith("~/"))
                {
                    url = HttpRuntime.AppDomainAppVirtualPath + url.Substring(1);
                }
            }

            _nodeKey[url] = xmlElement;

            SiteMapNode siteMapNode = new SiteMapNode(this, url,
                                        xmlElement.Attributes["url"].Value,
                                        xmlElement.Attributes["value"].Value);

            return siteMapNode;
        }
    }

}