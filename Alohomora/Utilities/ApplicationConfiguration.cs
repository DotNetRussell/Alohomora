using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Utilities
{
    /// <summary>
    /// This static class handles all application configuration operations
    /// </summary>
    public static class ApplicationConfiguration
    {
        public static string PiplApiKey = String.Empty;
        public static string ConnectionString = String.Empty;
        public static string DatabaseState = String.Empty;
        public static string DatabaseTableName = String.Empty;

        /// <summary>
        /// Loads the current application config file into memory for use
        /// </summary>
        public static void LoadConfig()
        {
            if (File.Exists("config.json"))
            {
                string json = File.ReadAllText("config.json");
                dynamic jobject = JsonConvert.DeserializeObject<dynamic>(json);
                PiplApiKey = jobject.config.pipl_apikey;
                ConnectionString = jobject.config.connection_string;
                DatabaseState = jobject.config.state;
                DatabaseTableName = jobject.config.db_table_name;
            }
        }

        /// <summary>
        /// Saves the passed in values to the application config file
        /// </summary>
        /// <param name="piplkey"></param>
        /// <param name="constring"></param>
        /// <param name="dbstate"></param>
        /// <param name="dbtablename"></param>
        public static void SaveConfig(string piplkey, string constring, string dbstate, string dbtablename) 
        {
            string json = String.Format( "{{ \"config\": {{ \"pipl_apikey\": \"{0}\",\"connection_string\": \"{1}\",\"state\":\"{2}\",\"db_table_name\":\"{3}\"}} }}",piplkey,constring,dbstate,dbtablename);
            File.Delete("config.json");
            File.AppendAllText("config.json", json);
            LoadConfig();
        }
    }
}
