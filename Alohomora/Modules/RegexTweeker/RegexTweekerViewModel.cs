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
    public class RegexTweekerViewModel : ViewModelBase
    {
        public ICommand UpdateFacebookConfigCommand { get; set; }
        public ICommand LoadDefaultsCommand { get; set; }

        private string _facebookQueryUrl;

        public string FacebookQueryUrl
        {
            get { return _facebookQueryUrl; }
            set { _facebookQueryUrl = value; OnPropertyChanged("FacebookQueryUrl"); }
        }

        private string _extractUsernamesFromProfilePage;

        public string ExtractUsernamesFromProfilePage
        {
            get { return _extractUsernamesFromProfilePage; }
            set { _extractUsernamesFromProfilePage = value; OnPropertyChanged("ExtractUsernamesFromProfilePage"); }
        }

        private string _getRealNameFromProfilePage;

        public string GetRealNameFromProfilePage
        {
            get { return _getRealNameFromProfilePage; }
            set { _getRealNameFromProfilePage = value; OnPropertyChanged("GetRealNameFromProfilePage"); }
        }

        private string _getProfilePhotoFromProfilePage;

        public string GetProfilePhotoFromProfilePage
        {
            get { return _getProfilePhotoFromProfilePage; }
            set { _getProfilePhotoFromProfilePage = value; OnPropertyChanged("GetProfilePhotoFromProfilePage"); }
        }

        private string _getProfilePhotoFromProfilePageSecondPass;

        public string GetProfilePhotoFromProfilePageSecondPass
        {
            get { return _getProfilePhotoFromProfilePageSecondPass; }
            set { _getProfilePhotoFromProfilePageSecondPass = value; OnPropertyChanged("GetProfilePhotoFromProfilePageSecondPass"); }
        }

        private string _getProfileIntroCardsFromProfilePage;

        public string GetProfileIntroCardsFromProfilePage
        {
            get { return _getProfileIntroCardsFromProfilePage; }
            set { _getProfileIntroCardsFromProfilePage = value; OnPropertyChanged("GetProfileIntroCardsFromProfilePage"); }
        }

        private string _getDetailsFromIntroCards;

        public string GetDetailsFromIntroCards
        {
            get { return _getDetailsFromIntroCards; }
            set { _getDetailsFromIntroCards = value; OnPropertyChanged("GetDetailsFromIntroCards"); }
        }

        private string _getDetailsFromIntroCardSecondPass;

        public string GetDetailsFromIntroCardsSecondPass
        {
            get { return _getDetailsFromIntroCardSecondPass; }
            set { _getDetailsFromIntroCardSecondPass = value; OnPropertyChanged("GetDetailsFromIntroCardsSecondPass"); }
        }

        private string _getLinksFromFindFriendsPage;

        public string GetLinksFromFindFriendsPage
        {
            get { return _getLinksFromFindFriendsPage; }
            set { _getLinksFromFindFriendsPage = value; OnPropertyChanged("GetLinksFromFindFriendsPage"); }
        }
        
        public RegexTweekerViewModel()
        {
            UpdateFacebookConfigCommand = new ButtonCommand(CanUpdateFacebookConfig, UpdateFacebookConfigExecuted);
            LoadDefaultsCommand = new ButtonCommand(CanLoadDefaults, LoadDefaultsExecuted);
            LoadCustomConfiguration();
        }

        private void LoadCustomConfiguration()
        {
            if (!FacebookRegexConfiguration.LoadCustomRegexes())
            {
                FacebookRegexConfiguration.LoadDefaults();
            }

            FacebookQueryUrl = FacebookRegexConfiguration.FacebookQueryURL;
            ExtractUsernamesFromProfilePage = FacebookRegexConfiguration.ExtractUserNamesFromProfilePageRegex;
            GetRealNameFromProfilePage = FacebookRegexConfiguration.GetRealNameFromProfilePageRegex;
            GetProfilePhotoFromProfilePage = FacebookRegexConfiguration.GetProfilePhotoFromProfilePageRegex;
            GetProfilePhotoFromProfilePageSecondPass = FacebookRegexConfiguration.GetProfilePhotoFromProfilePageSecondPassRegex;
            GetProfileIntroCardsFromProfilePage = FacebookRegexConfiguration.GetProfileIntroCardFromProfilePageRegex;
            GetDetailsFromIntroCards = FacebookRegexConfiguration.GetDetailsFromIntroCardRegex;
            GetDetailsFromIntroCardsSecondPass = FacebookRegexConfiguration.GetDetailsFromIntroCardSecondPassRegex;
            GetLinksFromFindFriendsPage = FacebookRegexConfiguration.GetLinksFromFindFriendsPageRegex;
        }

        public bool CanLoadDefaults(object args)
        {
            return true;
        }

        public void LoadDefaultsExecuted(object args)
        {
            FacebookRegexConfiguration.LoadDefaults();

            FacebookQueryUrl = FacebookRegexConfiguration.FacebookQueryURL;
            ExtractUsernamesFromProfilePage = FacebookRegexConfiguration.ExtractUserNamesFromProfilePageRegex;
            GetRealNameFromProfilePage = FacebookRegexConfiguration.GetRealNameFromProfilePageRegex;
            GetProfilePhotoFromProfilePage = FacebookRegexConfiguration.GetProfilePhotoFromProfilePageRegex;
            GetProfilePhotoFromProfilePageSecondPass = FacebookRegexConfiguration.GetProfilePhotoFromProfilePageSecondPassRegex;
            GetProfileIntroCardsFromProfilePage = FacebookRegexConfiguration.GetProfileIntroCardFromProfilePageRegex;
            GetDetailsFromIntroCards = FacebookRegexConfiguration.GetDetailsFromIntroCardRegex;
            GetDetailsFromIntroCardsSecondPass = FacebookRegexConfiguration.GetDetailsFromIntroCardSecondPassRegex;
            GetLinksFromFindFriendsPage = FacebookRegexConfiguration.GetLinksFromFindFriendsPageRegex;
            MessageBox.Show("Defaults Loaded");
        }

        public bool CanUpdateFacebookConfig(object args)
        {
            return true;
        }
        public void UpdateFacebookConfigExecuted(object args)
        {
            FacebookRegexConfiguration.SaveCustomRegexes(FacebookQueryUrl, ExtractUsernamesFromProfilePage, GetRealNameFromProfilePage, GetProfilePhotoFromProfilePage, GetProfilePhotoFromProfilePageSecondPass, GetProfileIntroCardsFromProfilePage, GetDetailsFromIntroCards, GetDetailsFromIntroCardsSecondPass, GetLinksFromFindFriendsPage);
            MessageBox.Show("Your facebook regexes have been updated.");
        }
    }
}
