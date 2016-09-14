using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using InvoiceClient.Properties;


namespace InvoiceClient
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.serviceInstaller1.ServiceName = Settings.Default.ServiceName;
            this.serviceInstaller1.DisplayName = Settings.Default.ServiceName;
        }
    }
}
