using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Utilities
{
    public static class ExternalBrowser
    {
        /// <summary>
        /// Calls the SimpleBrowser.exe and passes the URL as the target. The browser then returns the source 
        /// </summary>
        /// <param name="url">Target url for scraping</param>
        /// <param name="doscan">This needs to be true if your scraping facebook user names from the facebook query users call</param>
        /// <returns></returns>
        public static async Task<string> CallExternalBrowser(string url, bool doscan = false)
        {
            string html = "";
            try
            {

                string args = String.Empty;
                if (doscan)
                {
                    args = "\"" + url + "\" \"" + doscan + "\"";
                }
                else
                {
                    args = url;
                }

                Process process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.FileName = "SimpleBrowser.exe";

                process.StartInfo.Arguments = args;
                process.Start();
                html = await process.StandardOutput.ReadToEndAsync();
            }
            catch (Exception err) { /*Nom nom nom*/ }
            return html;
        }
    }
}
