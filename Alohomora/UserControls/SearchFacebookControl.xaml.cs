﻿using Alohomora.Models;
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
            List<FacebookLinkModel> _facebookUsers = new List<FacebookLinkModel>();

            FacebookButton.Visibility = Visibility.Collapsed;
            FaceBookBrowser.Visibility = Visibility.Collapsed;
            FacebookListBox.Visibility = Visibility.Visible;
            DetailsContainer.Visibility = Visibility.Visible;

            dynamic document = FaceBookBrowser.Document;
            var source = document.documentElement.InnerHtml;

            MatchCollection matches = Regex.Matches(source, "(?<=friendBrowserNameTitle fsl fwb fcb).*?(?=friendBrowserMarginTopMini)");

            foreach(Match blob in matches)
            {
                FacebookLinkModel facebookLinkModel = new FacebookLinkModel();

                facebookLinkModel.TargetSource = blob.Value;

                Match url = Regex.Match(blob.Value, "(?<=href=\").*?(?=\")");

                if (url != null)
                {
                    facebookLinkModel.TargetUrl = url.Value;
                }

                Match displayName = Regex.Match(blob.Value, "(?<=/ajax/hovercard/user.php).*?(?=</a)");

                if (displayName != null)
                {
                    Match innerMatch = Regex.Match(displayName.Value, "(?<=\">).*");
                    facebookLinkModel.DisplayName = innerMatch.Value;

                    if (facebookLinkModel.DisplayName.Contains("<"))
                    {
                        int index = facebookLinkModel.DisplayName.IndexOf("<");
                        facebookLinkModel.DisplayName = facebookLinkModel.DisplayName.Substring(0, index);
                    }
                }
                _facebookUsers.Add(facebookLinkModel);
            }
            
            this.DataContext = new SearchFacebookViewModel(_facebookUsers);
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
