using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C9ServiceManagerSupport
{
    /// <summary>
    /// Interface that provides access to service configuration information for the service manager service.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Interval between checks in seconds.
        /// </summary>
        /// <returns></returns>
        int checkInterval(); 

        /// <summary>
        /// Interval between checks in seconds
        /// </summary>
        /// <param name="newInterval"></param>
        void checkInterval(int newInterval);
     
        /// <summary>
        /// Return a list of services being managed and how they are to be handled.
        /// </summary>
        /// <returns></returns>
        ServiceConfiguration[] getServices();
    }
}
