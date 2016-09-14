using System.Xml;
using System;
namespace InvoiceClient.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    public sealed partial class Settings {

        private XmlDocument _config;
        
        public Settings() {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            this.SettingChanging += this.SettingChangingEventHandler;

            this.SettingsSaving += this.SettingsSavingEventHandler;
            
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Add code to handle the SettingChangingEvent event here.

        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Add code to handle the SettingsSaving event here.
            if (_config != null)
            {
                _config.Save(AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString());
            }
        }

        public override object this[string propertyName]
        {
            get
            {
                return base[propertyName];
            }
            set
            {
                base[propertyName] = value;

                if (_config == null)
                {
                    _config = new XmlDocument();
                    _config.Load(AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString());
                }
                var property = _config.SelectSingleNode(String.Format("//setting[@name='{0}']", propertyName));
                if (property != null)
                {
                    var valueNode = property["value"];
                    if (valueNode == null)
                    {
                        valueNode = _config.CreateElement("value");
                        property.AppendChild(valueNode);
                    }
                    if (value != null)
                        valueNode.InnerText = value.ToString();
                }
            }
        }
    }
}
