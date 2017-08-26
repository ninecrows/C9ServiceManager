using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace C9ServiceManagerService
{
    public partial class ServiceManagerService : ServiceBase
    {
        public ServiceManagerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Service Manager Service Starting Up...");
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Service Manager Service Stopping...");
        }
    }
}
