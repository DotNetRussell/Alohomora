using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Utilities
{
    public static class AlohomoraServices
    {
        private static Dictionary<string, object> Services = new Dictionary<string, object>();

        public static void RegisterService(string serviceName, object serviceInstance)
        {
            try
            {
                if(Services.ContainsKey(serviceName))
                {
                    Services.Remove(serviceName);
                }

                Services.Add(serviceName, serviceInstance);
            }
            catch(Exception error)
            {
                throw new Exception("There was an error while registering the service. " + error.Message);
            }
        }

        public static object GetService(string serviceName)
        {
            return Services[serviceName];
        }
    }
}
