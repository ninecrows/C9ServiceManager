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
        /// <summary>
        /// Milliseconds per tick for making adjustments.
        /// </summary>
        private int runRate = 20000;

        public ServiceManagerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Service Manager Service Starting Up...");

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = runRate;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);

            EventLog.WriteEntry($"Timer intialized at {runRate / 1000} Seconds");
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Service Manager Service Stopping...");
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            EventLog.WriteEntry("Ping");
        }
    }
}
