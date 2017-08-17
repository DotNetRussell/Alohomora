using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using mshtml;
using System.Data.SqlClient;
using System.Windows.Threading;
using System.Net;
using Alohomora.Models;
using Alohomora.Utilities;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;
using System.Diagnostics;
using Alohomora.ViewModels;
using static Alohomora.ViewModels.MasterViewModel;
using Alohomora.UserControls;
using Alohomora.Models.piplModels;

namespace Alohomora
{
    public partial class MainWindow : Window
    {
        //private BrowserControl _welcomePage = new BrowserControl();
        //private FacebookLogin _facebookLogin = new FacebookLogin();
        //private SearchVoterDBControl _searchVoterDBControl = new SearchVoterDBControl();
        //private SearchFacebookControl _searchFacebookControl = new SearchFacebookControl();
        //private RegexTweeker _regexTweeker = new RegexTweeker();
        //private ConfigurationControl _configurationControl = new ConfigurationControl();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationConfiguration.LoadConfig();
            // Need to wait for the window to load to initialize the view model otherwise the browser shits the bed.
            MasterViewModel viewmodel = new MasterViewModel();
            this.DataContext = viewmodel;
        }

      
    }
}
