using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace C9ServiceManagerSupport
{
    /// <summary>
    /// Base implementation of service configuration management for the service manager service.
    /// 
    /// Back end into the registry for persistence.
    /// </summary>
    public class Configuration : IConfiguration
    {
        private string keyPathBase = "SYSTEM\\CurrentControlSet\\Services\\";

        /// <summary>
        /// Construct an load a service manager service configuration object.
        /// </summary>
        /// <param name="myServiceName">Base name of this service under the services key</param>
        public Configuration(string myServiceName)
        {
            this.serviceName = myServiceName;

            string keyPath = keyPathBase + myServiceName;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath,
                                                                      RegistryKeyPermissionCheck.ReadSubTree))
            {
                if (key != null)
                {
                    Object o = key.GetValue("PollingInterval");

                    // Value item is null if we failed to read or if the value read isn't an int.
                    int? valueWeRead = o != null ? o as int? : null;

                    if (o == null)
                    {
                        defaultedInterval = true;
                    }
                    else
                    {
                        // Grab the check interval or default to 20 Seconds if not present.
                        int value = (o as int?) ?? 20000;
                    }
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
                                using (System.ServiceProcess.ServiceController thisService = new System.ServiceProcess.ServiceController(target))
                                {
                                    if (thisService != null)
                                    {
                                        // Yes, ToString handles running as well, but in the longer term I expect Running to be a particularly notable state.
                                        if (thisService.Status == System.ServiceProcess.ServiceControllerStatus.Running)
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

                       // EventLog.WriteEntry($"Probe Services\n{logme}");
                    }
                    else
                    {
                        //EventLog.WriteEntry("No Services key found.");
                    }
                }
            }
        }

        private int storedInterval = 20000;
        private bool defaultedInterval = false;

        public string serviceName { get; }

        public int checkInterval()
        {
            return (0);
        }

        public void checkInterval(int newInterval)
        {

        }

        public ServiceConfiguration[] getServices()
        {
            int itemCount = 0;

            return new ServiceConfiguration[itemCount];
        }
    }
}
