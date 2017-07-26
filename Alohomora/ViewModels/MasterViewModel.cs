using Alohomora.Models;
using Alohomora.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alohomora.ViewModels
{
    public class MasterViewModel : ViewModelBase
    {
        private int _selectedSearchIndex;
        public event RoutedEventHandler OnMainColumnChanged;
        public delegate void MainColumnChanged(object sender, RoutedEventArgs e);

        public class MainColumnChangedEventArgs : RoutedEventArgs
        {
            public int MainColumnIndex { get; set; }
        }

        /// <summary>
        /// I did this stupid thing with the navigation panel because I was to lazy to do it the correct way.
        /// Now it looks like garbage and it took me just as long to do if not longer....
        /// Don't do this... Don't be Anthony...
        /// </summary>
        public int SelectedSearchIndex
        {
            get { return _selectedSearchIndex; }
            set {
                    _selectedSearchIndex = value;
                    OnPropertyChanged("SelectedSearchIndex");
                    OnMainColumnChanged(this, (new MainColumnChangedEventArgs() { MainColumnIndex = value }));
                }
        }
    }
}
