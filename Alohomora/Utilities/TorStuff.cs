using System;
using Knapcode.TorSharp;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alohomora.Utilities
{
    /// <summary>
    /// This tor class works so long as tor is running on your machine. 
    /// </summary>
    public static class TorStuff
    {
        private static TorSharpSettings _torSettings;
        public static async void InitializeTor()
        {
            _torSettings = new TorSharpSettings
            {
                ZippedToolsDirectory = Path.Combine(Path.GetTempPath(), "TorZipped"),
                ExtractedToolsDirectory = Path.Combine(Path.GetTempPath(), "TorExtracted"),
                PrivoxyPort = 1337,
                TorSocksPort = 1338,
                TorControlPort = 1339,
                TorControlPassword = "foobar",
                ToolRunnerType = ToolRunnerType.Simple
            };

            // download tools
            await new TorSharpToolFetcher(_torSettings, new HttpClient()).FetchAsync();
        }

        public static async Task<string> MakeTorRequest(string url)
        {
            string result = String.Empty;
            
            using (var proxy = new TorSharpProxy(_torSettings))
            using (var handler = proxy.CreateHttpClientHandler())
            using (var httpClient = new HttpClient(handler))
            {
                await proxy.ConfigureAndStartAsync();
                result = await httpClient.GetStringAsync(url);
            }

            return result;
        }
    }
}
