using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;

namespace Uxnet.Com.Providers
{
    public class UserMenuManager
    {
        private string _storedPath;
        private Dictionary<string, XmlDocument> _cachedMenu;


        public UserMenuManager(string storedPath)
        {
            _storedPath = storedPath;
            _cachedMenu = new Dictionary<string, XmlDocument>();
        }

        public XmlDocument GetMenuDocument(string menuPath)
        {
            if (!_cachedMenu.ContainsKey(menuPath))
            {
                XmlDocument menuDoc = new XmlDocument();
                menuDoc.Load(Path.Combine(_storedPath, menuPath));
                _cachedMenu.Add(menuPath, menuDoc);
                return menuDoc;
            }

            return _cachedMenu[menuPath];
        }

        public void Reload(string menuPath)
        {
            XmlDocument menuDoc = new XmlDocument();
            menuDoc.Load(Path.Combine(_storedPath, menuPath));
            _cachedMenu[menuPath] = menuDoc;
        }

        public void Save(string menuPath, XmlDocument menuDoc)
        {
            menuDoc.Save(Path.Combine(_storedPath, menuPath));
            if (!_cachedMenu.ContainsKey(menuPath))
            {
                _cachedMenu.Add(menuPath, menuDoc);
            }
            else
            {
                _cachedMenu[menuPath] = menuDoc;
            }
        }

        public void Save(string menuPath, string xmlData)
        {
            XmlDocument menuDoc = new XmlDocument();
            menuDoc.LoadXml(xmlData);
            Save(menuPath, menuDoc);
        }

        public void Save(XmlDocument doc)
        {
            var keyValue = _cachedMenu.Where(p => p.Value == doc).FirstOrDefault();
            if (!default(KeyValuePair<String,XmlDocument>).Equals(keyValue))
            {
                Save(keyValue.Key, keyValue.Value);
            }
        }

        public String StoredPath
        {
            get
            {
                return _storedPath;
            }
        }

    }
}
