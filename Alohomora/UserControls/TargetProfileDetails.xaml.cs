using Alohomora.Models;
using Alohomora.ViewModels;
using System;
using System.Collections.Generic;
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

namespace Alohomora.UserControls
{
    public partial class TargetProfileDetails : UserControl
    {
        /// <summary>
        /// Initializes a new instance of Target Profile Details
        /// </summary>
        /// <param name="targetProfile">The target profile to be displayed</param>
        public TargetProfileDetails(PersonModel targetProfile)
        {
            InitializeComponent();
            this.DataContext = new TargetProfileViewModel(targetProfile);
            this.Loaded += TargetProfileDetails_Loaded;
        }

        private void TargetProfileDetails_Loaded(object sender, RoutedEventArgs e)
        {
            CheckFirstAddress();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button != null)
            {
                TargetProfileViewModel.SetMapLocation(button.Content as string);
            }
        }

        private void CheckFirstAddress()
        {
            var radios = FindVisualChildren<RadioButton>(AddressListControl);
            if (radios.Count() > 0)
            {
                radios.FirstOrDefault().IsChecked = true;
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
