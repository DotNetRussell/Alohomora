
using Alohomora.Models;
using Alohomora.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace Alohomora.Modules.VoterDatabase
{
    [ModuleEntryPoint(ModuleIconPath="/Modules/VoterDatabase/Images/module_icon.png")]
    public partial class SearchVoterDBControl : UserControl
    {
        public SearchVoterDBControl()
        {
            InitializeComponent();
            this.DataContext = new SearchVoterDatabaseViewModel();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void ProfileExpanderButton_Expanded(object sender, RoutedEventArgs e)
        {
            var listBoxItem = GetParent<ListBoxItem>((DependencyObject)sender) as ListBoxItem;

            if (listBoxItem != null)
            {
                LinkModel linkModel = listBoxItem.DataContext as LinkModel;

                if(linkModel != null)
                {
                    UsernameLinkModel user = SearchVoterDatabaseViewModel.GetUsernameLinkModel(this.DataContext, linkModel.UserModelLinkId);
                    Expander expander = sender as Expander;

                    if(expander != null)
                    {
                        ListBox innerListbox = expander.Content as ListBox;
                        innerListbox.ItemsSource = user.usernames;
                    }
                }
            }
        }

        private T GetParent<T>(DependencyObject root)
        {
            object retObj = null;
           
            var parent = VisualTreeHelper.GetParent((DependencyObject)root);

            while(parent != null)
            {               
                if (parent is T)
                {
                    retObj = parent;
                    break;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent((DependencyObject)parent);
                }
            }
            return (T)retObj;
        }
    }
}
