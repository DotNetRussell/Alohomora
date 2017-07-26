using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Utilities
{
    /// <summary>
    /// This class is still experimental.... don't use yet
    /// </summary>
    public class RandomWebProxy
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }

        private void TestProxy(string ip, int port, Action<bool> callback)
        {
            WebClient client = new WebClient();
            WebProxy proxy = new WebProxy(ip, port);
            client.Proxy = proxy;

            try
            {
                client.DownloadStringCompleted += (s, a) => 
                {
                    callback(true);
                };

                client.DownloadStringAsync(new Uri("https://pipl.com"));

            }
            catch
            {
                callback(false);
            }

        }

        public void NewRandomProxy(Action<string,int> callback)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, a) =>
            {
                dynamic jobject = JsonConvert.DeserializeObject<dynamic>(a.Result);
                IpAddress = jobject.ip;
                Port = jobject.port;

                //TestProxy(IpAddress, Port, (isConfigured) => 
                //{
                //    if (isConfigured)
                //    {
                        callback(IpAddress, Port);
                //    }
                //    else
                //    {
                //        NewRandomProxy(callback);
                //    }
                //});
            };
            client.DownloadStringAsync(new Uri("http://gimmeproxy.com/api/getProxy?post=true&supportsHttps=true&maxCheckPeriod=3600"));
        }
    }
}
