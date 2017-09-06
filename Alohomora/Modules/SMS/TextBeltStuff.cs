using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Modules.SMS
{
    class TextBeltStuff
    {
        /// <summary>
        /// You must first have a textbelt api key prior to using this service. You can get one at https://textbelt.com/
        /// </summary>
        /// <param name="phoneNumber">Target Phone Number</param>
        /// <param name="message">Your SMS Message Body</param>
        /// <param name="key">TextBelt API Key</param>
        /// <param name="callback">If you wish to get a JSON response, wire the callback up</param>
        public static void SendText(string phoneNumber, string message, string key, Action<string> callback = null)
        {
            using (WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues("http://textbelt.com/text", 
                    new NameValueCollection() {
                                                { "phone", phoneNumber },
                                                { "message", message },
                                                { "key", key },
                                              });

                if(callback != null)
                {
                    string result = System.Text.Encoding.UTF8.GetString(response);
                    callback(result);
                }
            }
        }
    }
}
