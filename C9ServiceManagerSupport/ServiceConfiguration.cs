using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C9ServiceManagerSupport
{
    /// <summary>
    /// Container for the service manager service configuration for a single service.
    /// </summary>
    public class ServiceConfiguration
    {
        public ServiceConfiguration(string serviceName) 
        {
            this.name = serviceName;
        }

        /// <summary>
        /// Name of the associated service.
        /// </summary>
        public string name { get; }

        /// <summary>
        /// Time of day (in minutes from start of day) that we begin managing this service.
        /// </summary>
        public int startTime { get; set; }

        /// <summary>
        /// Time if day (in minutes from start of day) that we stop managing this service.
        /// </summary>
        public int endTime { get; set; }

        /// <summary>
        /// Do we make sure the service is running or stopped during active times?
        /// </summary>
        public bool runService { get; set; } 

        /// <summary>
        /// Do we manage the service state or simply watch and record changes?
        /// </summary>
        public bool monitorOnly { get; set; }
    }
}
