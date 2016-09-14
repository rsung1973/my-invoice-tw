using System;
using System.Xml;
using System.Collections.Specialized;
using System.Configuration;

namespace Config
{
    /// <summary>
    /// Summary description for DBConfig.
    /// </summary>
    public class BasicConfiguration
    {
        protected static BasicConfiguration _configuration = new BasicConfiguration();

        protected NameValueCollection _values;

        protected BasicConfiguration()
        {
            //
            // TODO: Add constructor logic here
            //
            initializeNameValueSettings();
        }

        private void initializeNameValueSettings()
        {
//            _values = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection("appParams");
            _values = ConfigurationManager.AppSettings;

        }


        public static BasicConfiguration Values
        {
            get
            {
                return _configuration;
            }
        }

        public string this[int index]
        {
            get
            {
                return _values[index];
            }
        }

        public string this[string name]
        {
            get
            {
                return _values[name];
            }
            //set
            //{
            //    _values.Add(name, value);
            //}
        }


    }
}
