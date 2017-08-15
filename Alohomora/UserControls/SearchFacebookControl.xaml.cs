using Alohomora.Models;
using Alohomora.Utilities;
using Alohomora.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Alohomora.UserControls
{
    /// <summary>
    /// Interaction logic for SearchFacebookControl.xaml
    /// </summary>
    public partial class SearchFacebookControl : UserControl
    {

        public SearchFacebookControl()
        {
            InitializeComponent();
        }

        private void FacebookButton_Click(object sender, RoutedEventArgs e)
        {
            FacebookButton.Visibility = Visibility.Collapsed;
            FaceBookBrowser.Visibility = Visibility.Collapsed;
            FacebookListBox.Visibility = Visibility.Visible;
            DetailsContainer.Visibility = Visibility.Visible;

            dynamic document = FaceBookBrowser.Document;
            string source = document.documentElement.InnerHtml;
            
            this.DataContext = new SearchFacebookViewModel(FacebookStuff.GetLinksFromFindFriends(source));
        }

        private void BackToSearchClicked(object sender, RoutedEventArgs e)
        {
            FacebookButton.Visibility = Visibility.Visible;
            FaceBookBrowser.Visibility = Visibility.Visible;
            FacebookListBox.Visibility = Visibility.Collapsed;
            DetailsContainer.Visibility = Visibility.Collapsed;
        }
    }
}
