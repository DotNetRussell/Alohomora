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
using System.ComponentModel;

namespace Alohomora
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationConfiguration.LoadConfig();

            // Need to wait for the window to load to initialize 
            // the view model otherwise the browser shits the bed.
            this.DataContext = new MasterViewModel(); 
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            // Make sure to cache the profiles before we close
            MasterTargetListViewModel.SaveTargetProfiles();

            Window _window = Window.GetWindow(this);
            foreach(Window win in _window.OwnedWindows)
            {
                win.Close();
            }            
        }
    }
}
