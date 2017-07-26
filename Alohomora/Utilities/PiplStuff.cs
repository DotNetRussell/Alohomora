using Alohomora.Models.piplModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Alohomora.Utilities
{
    public static class PiplStuff
    {
        private class PiplWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest webRequest = base.GetWebRequest(address);
                webRequest.Timeout = 20 * 60 * 1000;
                return base.GetWebRequest(address);
            }
        }

        private static string GetProxyUrl(string url)
        {
            WebClient webclient = new WebClient();
            WebProxy proxy = new WebProxy("77.174.47.101", 80);
            webclient.Proxy = proxy;
            string response = webclient.DownloadString(url);

            return response;
        }

        private static string BuildURL(string firstname, string lastname, string city, string state, string apikey)
        {
            return String.Format("https://api.pipl.com/search/?first_name={0}&last_name={1}&state={3}&city={2}&key={4}", firstname, lastname, city, state, apikey);
        }

        private static async Task<string> ExecutePiplRequest(string url, Action<string> callback)
        {
            PiplWebClient webclient = new PiplWebClient();
            string response = String.Empty;
            try
            {
                //Random web proxies are disabled atm....
                //They're fucking broke... 

                //RandomWebProxy randomProxy = new RandomWebProxy();
                //randomProxy.NewRandomProxy((ip,port)=> 
                //{
                //WebProxy proxy = new WebProxy(randomProxy.IpAddress, randomProxy.Port);
                //proxy.UseDefaultCredentials = true;
                //webclient.Proxy = proxy;
                //making post requests
                //https://stackoverflow.com/questions/4015324/http-request-with-post/4015346#4015346
                //response = await TorStuff.MakeTorRequest(url);
                webclient.DownloadStringAsync(new Uri(url));
                webclient.DownloadStringCompleted += (s, a) =>
                {
                    try
                    {
                        callback(a.Result);
                    }
                    catch (Exception e)
                    {
                        ExecutePiplRequest(url, callback);
                    }

                };

                //});

            }
            catch (Exception err) { Console.WriteLine(err.Message); /**Nom nom nom**/ }
            return response;
        }

        private static Person[] ParsePiplJson(string json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return new Person[] { };
            }

            dynamic jobject = JsonConvert.DeserializeObject<dynamic>(json);

            dynamic people = jobject.possible_persons;
            dynamic person = jobject.person;

            if (people == null && person == null)
            {
                return new Person[] { };
            }
            return person != null ? new Person[] { JsonConvert.DeserializeObject<Person>(person.ToString()) } : JsonConvert.DeserializeObject<Person[]>(people.ToString());
        }

        public static async void RetrievePiplData(string firstname, string lastname, string city, string state, Action<Person[]> callback)
        {
            string url = BuildURL(firstname, lastname, city, state, ApplicationConfiguration.PiplApiKey);

            await ExecutePiplRequest(url, (result) =>
            {
                callback(ParsePiplJson(result));
            });
        }
    }
}
