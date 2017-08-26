using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace C9ServiceManagerService
{
    public partial class ServiceManagerService : ServiceBase
    {
        /// <summary>
        /// Milliseconds per tick for making adjustments.
        /// </summary>
        private int runRate = 20000;

        System.Timers.Timer timer = new System.Timers.Timer();

        public ServiceManagerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Service Manager Service Starting Up...");

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\" +
                    "ServiceManagerService",
                    RegistryKeyPermissionCheck.ReadWriteSubTree,
                    System.Security.AccessControl.RegistryRights.FullControl))
                {
                    if (key != null)
                    {
                        EventLog.WriteEntry("Opened service key");

                        Object o = key.GetValue("PollingInterval");
                        if (o == null)
                        {
                            EventLog.WriteEntry("Rate not set, defaulting");
                            key.SetValue("PollingInterval", runRate);
                        }
                        else
                        {
                            int value = (o as int?) ?? 20000;
                            EventLog.WriteEntry($"Set rate to {value}");
                        }
                    }
                    else
                    {
                        EventLog.WriteEntry("Failed to open service key");
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry($"Failed to read registry {e}");
            }

            // Set the run rate and handler.
            timer.Interval = runRate;
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            EventLog.WriteEntry($"Timer intialized at {runRate / 1000.0} Seconds");
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Service Manager Service Stopping...");

            timer.Stop();
            //timer.Close();
            timer.Dispose();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            EventLog.WriteEntry("Ping");
        }
    }
}
