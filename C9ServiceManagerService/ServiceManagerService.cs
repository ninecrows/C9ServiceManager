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

        // Version display on startup...there must be a better way to
        // handle this, but I'm not going looking at the moment. 
        private int version = 0;
        private int minorVersion = 1;
        private int build = 5;

        // Store the timer that we use for our wake-ups.
        System.Timers.Timer timer = new System.Timers.Timer();

        public ServiceManagerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry($"Service Manager Service Starting Up @ {version}.{minorVersion}.{build}");

            // Fetch configuration information from the registry. This should be factored but at the moment I've got other fish to fry.
            try
            {
                EventLog.WriteEntry($"Reading configuration for {this.ServiceName}");

                string keyPath = "SYSTEM\\CurrentControlSet\\Services\\" + ServiceName;

                // Get our service key so that we can retrieve configuration information stored beneath it.
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath,
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

                    // Look for the services key and if it is present we'll enumerate the subkeys. Each subkey corresponds to a service that is to be monitored or managed.
                    using (RegistryKey servicesKey = key.OpenSubKey("Services"))
                    {
                        if (servicesKey != null)
                        {
                            string[] targets = servicesKey.GetSubKeyNames();

                            string logme = "";
                            foreach (string target in targets)
                            {
                                string serviceState = "Does not exist";
                                try
                                {
                                   using (ServiceController thisService = new ServiceController(target))
                                    {
                                        if (thisService != null)
                                        {
                                            // Yes, ToString handles running as well, but in the longer term I expect Running to be a particularly notable state.
                                            if (thisService.Status == ServiceControllerStatus.Running)
                                            {
                                                serviceState = ">Running<";
                                            }
                                            else
                                            {
                                                serviceState = thisService.Status.ToString();
                                            }
                                        }
                                        else
                                        {
                                            // Actually handled by the default string for the moment.
                                        }
                                    }
                                }

                                // In this case we really don't want to do anything if this fails...its just a state probe. 
                                catch (Exception e)
                                {
                                    serviceState = "error probing " + e.ToString();
                                }

                                    logme += $"{target}: {serviceState}\n";
                            }

                            EventLog.WriteEntry($"Probe Services\n{logme}");
                        }
                        else
                        {
                            EventLog.WriteEntry("No Services key found.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry($"Failed to read registry {e}");
            }

            ServiceController[] services = ServiceController.GetServices();
            EventLog.WriteEntry($"Found {services.Length} services");

            string results = "";
            foreach (ServiceController service in services)
            {
                results += $"[{service.ServiceName}] -> \"{service.DisplayName}\"\n";
            }

            EventLog.WriteEntry(results);

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
