using Alohomora.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Alohomora.ViewModels
{
    /// <summary>
    /// Used to populate / save and load the json config file
    /// </summary>
    public class ConfigurationViewModel : ViewModelBase
    {
        private string _piplApiKey;

        /// <summary>
        /// Used for all pipl activity
        /// </summary>
        public string PiplApiKey
        {
            get { return _piplApiKey; }
            set { _piplApiKey = value; OnPropertyChanged("PiplApiKey"); }
        }

        private string _connectionString;

        /// <summary>
        /// Connects to the voter database
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; OnPropertyChanged("ConnectionString"); }
        }

        private string _dbState;

        /// <summary>
        /// Used for searching the voter database
        /// 
        /// TODO: Make it so you can enable multiple states....
        /// </summary>
        public string DatabaseState
        {
            get { return _dbState; }
            set { _dbState = value; OnPropertyChanged("DatabaseState"); }
        }

        private string _dbTablename;

        /// <summary>
        /// Voter Database table name. Used in the select queries
        /// </summary>
        public string DatabaseTableName
        {
            get { return _dbTablename; }
            set { _dbTablename = value; OnPropertyChanged("DatabaseTableName"); }
        }

        /// <summary>
        /// Command bound to the save button
        /// </summary>
        public ICommand SaveConfigurationCommand { get; set; }

        /// <summary>
        /// Constructs a new instance of the ConfigurationViewModel
        /// </summary>
        public ConfigurationViewModel()
        {
            ApplicationConfiguration.LoadConfig();
            PiplApiKey = ApplicationConfiguration.PiplApiKey;
            ConnectionString = ApplicationConfiguration.ConnectionString;
            DatabaseState = ApplicationConfiguration.DatabaseState;
            DatabaseTableName = ApplicationConfiguration.DatabaseTableName;

            SaveConfigurationCommand = new ButtonCommand(CanExecuteSaveConfiguration, SaveConfigurationExecuted);
        }

        /// <summary>
        /// Can execute a save operation
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Returns true if save can be executed</returns>
        public bool CanExecuteSaveConfiguration(object args)
        {
            return true;
        }

        /// <summary>
        /// Save Executed
        /// </summary>
        /// <param name="args"></param>
        public void SaveConfigurationExecuted(object args)
        {
            ApplicationConfiguration.SaveConfig(PiplApiKey, ConnectionString, DatabaseState, DatabaseTableName);

            //TODO get this out of the view model.... wtf anthony...
            MessageBox.Show("Your config has been saved...");
        }
    }
}
