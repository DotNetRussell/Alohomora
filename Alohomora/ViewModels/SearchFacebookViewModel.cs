using Alohomora.Models;
using Alohomora.Models.piplModels;
using Alohomora.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Alohomora.ViewModels
{
    public class SearchFacebookViewModel : ViewModelBase
    {
        #region Private backers

        private string _searchCriteria;
        private string _searchLocation;
        private Visibility _isLoadingVisibility;
        private FacebookLinkModel _selectedFacebookLinkModel;

        #endregion

        #region Properties

        /// <summary>
        /// Displays the wait indicator 
        /// </summary>
        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            set { _isLoadingVisibility = value; OnPropertyChanged("IsLoadingVisibility"); }
        }

        /// <summary>
        /// Used as the private backer for IsLoadingVisibility 
        /// </summary>
        private bool IsLoading
        {
            set { IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Used as the data context for the main content container
        /// </summary>
        public FacebookLinkModel SelectedFacebookLinkModel
        {
            get { return _selectedFacebookLinkModel; }
            set { _selectedFacebookLinkModel = value; OnPropertyChanged("SelectedFacebookLinkModel"); }
        }

        /// <summary>
        /// This collection is the users scraped from facebook
        /// </summary>
        public ObservableCollection<FacebookLinkModel> FacebookUsers { get; set; }

        #endregion

        #region Command Properties

        public ICommand OpenFacebookUrlCommand { get; set; }
        public ICommand SearchVoterDBCommand { get; set; }
        public ICommand ScrapeFacebookCommand { get; set; }
        public ICommand ModifiedVoterDBSearchCommand { get; set; }
        public ICommand ModifiedVoterDBSearchSpouseCommand { get; set; }
        public ICommand SearchPiplAPICommand { get; set; }

        #endregion

        #region Constructor

        public SearchFacebookViewModel(IEnumerable<FacebookLinkModel> facebookUsers)
        {
            IsLoading = false;
            FacebookUsers = new ObservableCollection<FacebookLinkModel>(facebookUsers);
            OpenFacebookUrlCommand = new ButtonCommand(CanExecuteOpenFacebookUrlCommand, OpenFacebookUrlCommandExecuted);
            SearchVoterDBCommand = new ButtonCommand(CanExecuteSearchVoterDBCommand, SearchVoterDBCommandExecuted);
            ScrapeFacebookCommand = new ButtonCommand(CanExecuteFacebookScrapeCommand, FacebookScrapeExecuted);
            SearchPiplAPICommand = new ButtonCommand(CanExecutePiplApiSearch, PiplApiSearchExecuted);
            ModifiedVoterDBSearchCommand = new ButtonCommand(CanExecuteModifiedSearchVoterDBCommand, ExecutedModifiedSearchVoterDBCommand);
        }

        #endregion

        #region Command Handlers

        public bool CanExecutePiplApiSearch(object args)
        {
            return true;
        }

        public void PiplApiSearchExecuted(object args)
        {
            FacebookLinkModel facebookLinkModel = SelectedFacebookLinkModel;

            if (SelectedFacebookLinkModel==null 
                || String.IsNullOrEmpty(facebookLinkModel.CurrentCity)
                || String.IsNullOrEmpty(facebookLinkModel.CurrentCity))
            {
                MessageBox.Show("You must first scrape facebook before doing sub operations");
                return;
            }

            IsLoading = true;

            if (facebookLinkModel != null && !String.IsNullOrEmpty(facebookLinkModel.CurrentCity))
            {
                PiplStuff.RetrievePiplData(facebookLinkModel.FirstName,
                    facebookLinkModel.LastName, facebookLinkModel.CurrentCity,
                    facebookLinkModel.CurrentState, (personArray) =>
                    {
                        facebookLinkModel.PiplLinks = personArray;
                        IsLoading = false;
                    });
            }
        }

        public bool CanExecuteSpouseModifiedSearchVoterDBCommand(object args)
        {
            FacebookLinkModel facebookLinkModel = SelectedFacebookLinkModel;
            if (facebookLinkModel != null && !String.IsNullOrEmpty(facebookLinkModel.MarriedTo))
            {
                return !String.IsNullOrEmpty(facebookLinkModel.MarriedTo);
            }
            else
            {
                return false;
            }
        }

        public async void ExecutedSpouseModifiedSearchVoterDBCommand(object args)
        {
            IsLoading = true;

            FacebookLinkModel facebookLinkModel = SelectedFacebookLinkModel;
            RunSpouseQuery(facebookLinkModel);

            IsLoading = false;
        }

        public bool CanExecuteModifiedSearchVoterDBCommand(object args)
        {
            FacebookLinkModel facebookLinkModel = SelectedFacebookLinkModel;
            if (facebookLinkModel != null && !String.IsNullOrEmpty(facebookLinkModel.CurrentCity))
            {
                return !String.IsNullOrEmpty(facebookLinkModel.CurrentCity);
            }
            else
            {
                return false;
            }
        }

        public void ExecutedModifiedSearchVoterDBCommand(object args)
        {
            FacebookLinkModel facebookLinkModel = SelectedFacebookLinkModel;
            SearchVoterDBCommandExecuted(facebookLinkModel);
        }

        public bool CanExecuteSearchVoterDBCommand(object args)
        {
            return true;
        }

        public async void SearchVoterDBCommandExecuted(object args)
        {
            FacebookLinkModel facebookLinkModel = args as FacebookLinkModel;
            
            if(facebookLinkModel == null 
                || String.IsNullOrEmpty(facebookLinkModel.CurrentCity)                 
                || String.IsNullOrEmpty(facebookLinkModel.CurrentCity))
            {
                MessageBox.Show("You must first scrape facebook before doing sub operations");
                return;
            }

            IsLoading = true;

            SelectedFacebookLinkModel = facebookLinkModel;
            facebookLinkModel.PossibleLinks =
                await DatabaseStuff.RunQuery(facebookLinkModel.FirstName,
                facebookLinkModel.LastName, facebookLinkModel.CurrentCity);

            RunSpouseQuery(facebookLinkModel);

            if (facebookLinkModel.PossibleSpouseLinks != null && facebookLinkModel.PossibleSpouseLinks != null && facebookLinkModel.PossibleLinks.Count > 0 && facebookLinkModel.PossibleSpouseLinks.Count > 0)
            {
                bool matched = false;
                foreach (PersonModel possibleLink in facebookLinkModel.PossibleLinks)
                {
                    if (matched)
                    {
                        break;
                    }

                    foreach (PersonModel possibleSpouseLink in facebookLinkModel.PossibleSpouseLinks)
                    {
                        if (possibleLink.address == possibleSpouseLink.address)
                        {
                            facebookLinkModel.VoterDBMatch = possibleLink;
                            facebookLinkModel.SouseVoterDBMatch = possibleSpouseLink;
                            matched = true;
                        }
                    }
                }
            }
            IsLoading = false;
        }

        public bool CanExecuteFacebookScrapeCommand(object args)
        {
            return true;
        }

        public async void FacebookScrapeExecuted(object args)
        {
            // Display the loading screen
            IsLoading = true;

            // Get the LinkModel that we're tring to scrape details for
            FacebookLinkModel facebookLinkModel = args as FacebookLinkModel;
            SelectedFacebookLinkModel = facebookLinkModel;

            // Make a call to the target profile url and get the source
            var source = await ExternalBrowser.CallExternalBrowser(facebookLinkModel.TargetUrl);

            // Get the real name from the profile 
            string realName = FacebookStuff.GetRealNameFromProfilePage(source);

            // Get the profile image from the profile
            facebookLinkModel.ProfileImage = FacebookStuff.GetProfilePhotoFromProfilePage(source);

            // Get all of the details from the profile page for parsing
            List<string> details = FacebookStuff.GetIntroFromAuthenticatedProfilePage(source);
            facebookLinkModel.TargetDetails = details;

            facebookLinkModel.ParsedDetails = new List<string>();
            foreach (string detail in facebookLinkModel.TargetDetails)
            {
                string text = FacebookStuff.GetTextFromSingleDetailAuthenticated(detail);
                if (!String.IsNullOrEmpty(text) && !facebookLinkModel.ParsedDetails.Contains(FacebookStuff.GetTextFromSingleDetailAuthenticated(detail)))
                {
                    facebookLinkModel.ParsedDetails.Add(FacebookStuff.GetTextFromSingleDetailAuthenticated(detail));
                }
            }

            // If the name contains a space there's more than one name listed
            if (realName.Contains(' '))
            {
                // If the name has two items in it then set index 0 to first name and index 1 to last name
                if (realName.Split(' ').Count() == 2)
                {
                    facebookLinkModel.FirstName = realName.Split(' ')[0];
                    facebookLinkModel.LastName = realName.Split(' ')[1];
                }
                // Otherwise they list a middle name or initial and set index 0 as first name and index 2 as last name
                else
                {
                    facebookLinkModel.FirstName = realName.Split(' ')[0];
                    facebookLinkModel.LastName = realName.Split(' ')[2];
                }
            }
            else
            {
                facebookLinkModel.FirstName = realName;
            }

            if (realName.Contains(','))
            {
                facebookLinkModel.FirstName = realName.Split(',')[1];
                facebookLinkModel.LastName = realName.Split(',')[0];
            }

            foreach (string detail in facebookLinkModel.TargetDetails)
            {
                // lower the detail so we can identify it easier 
                string lowerDetail = detail.ToLower();

                // If the detail contains "lives in" or "current city" then we know this is the current city detail
                if (lowerDetail.Contains("lives in") || lowerDetail.Contains("current city"))
                {
                    string cityState = FacebookStuff.GetTextFromSingleDetailAuthenticated(detail);

                    if (cityState.Contains(','))
                    {
                        facebookLinkModel.CurrentCity = cityState.Split(',')[0];
                        facebookLinkModel.CurrentState = cityState.Split(',')[1];
                    }
                    else
                    {
                        facebookLinkModel.CurrentCity = cityState;
                    }

                }

                // If the detail contains "studied" then it's the college detail
                if (lowerDetail.Contains("studied"))
                {
                    facebookLinkModel.College = FacebookStuff.GetTextFromSingleDetailAuthenticated(detail);
                }

                // If the detail contains "went to" then it's the high school detail
                if (lowerDetail.Contains("went to"))
                {
                    facebookLinkModel.HighSchool = FacebookStuff.GetTextFromSingleDetailAuthenticated(detail);
                }

                // If the detail contains "from" then this detail is for the origin city and state
                if (lowerDetail.Contains("from"))
                {
                    string cityState = FacebookStuff.GetTextFromSingleDetailAuthenticated(detail);

                    // If there's a comma then split on it and set index 0 as the city and index 1 as the state
                    if (cityState.Contains(','))
                    {
                        facebookLinkModel.OriginCity = cityState.Split(',')[0];
                        facebookLinkModel.OriginState = cityState.Split(',')[1];
                    }
                    else
                    {
                        facebookLinkModel.OriginCity = cityState;
                    }
                }

                // If the detail contains mairried to then it will list a spouse
                if (lowerDetail.Contains("married to"))
                {
                    string spouseName = FacebookStuff.GetTextFromSingleDetailAuthenticated(detail);
                    facebookLinkModel.MarriedTo = spouseName;

                    // If the name contains a space there's more than one name listed
                    if (spouseName.Contains(' '))
                    {
                        // If the name has two items in it then set index 0 to first name and index 1 to last name
                        if(spouseName.Split(' ').Count() == 2)
                        {
                            facebookLinkModel.SpouseFirstName = spouseName.Split(' ')[0];
                            facebookLinkModel.SpouseLastName = spouseName.Split(' ')[1];
                        }
                        // Otherwise they list a middle name or initial and set index 0 as first name and index 2 as last name
                        else
                        {
                            facebookLinkModel.SpouseFirstName = spouseName.Split(' ')[0];
                            facebookLinkModel.SpouseLastName = spouseName.Split(' ')[2];
                        }
                    }
                    else if (spouseName.Contains(','))
                    {
                        facebookLinkModel.SpouseFirstName = spouseName.Split(',')[1];
                        facebookLinkModel.SpouseLastName = spouseName.Split(',')[0];
                    }
                }

                // This is the magic string for the little suitcase image next to the job detail text
                // It's a bad identifier and will likely break
                if (lowerDetail.Contains("sx_9deefd"))
                {
                    if (facebookLinkModel.Jobs == null)
                    {
                        facebookLinkModel.Jobs = new List<string>();
                    }

                    facebookLinkModel.Jobs.Add(FacebookStuff.GetTextFromSingleDetailAuthenticated(detail));
                }

            }
            IsLoading = false;
        }

        public bool CanExecuteOpenFacebookUrlCommand(object args)
        {
            return true;
        }

        public void OpenFacebookUrlCommandExecuted(object args)
        {
            Process.Start(new ProcessStartInfo(args.ToString()));
        }

        #endregion

        #region Private Functions

        private async void RunSpouseQuery(FacebookLinkModel facebookLinkModel)
        {
            if (!String.IsNullOrEmpty(facebookLinkModel.MarriedTo))
            {
                facebookLinkModel.PossibleSpouseLinks =
                    await DatabaseStuff.RunQuery(facebookLinkModel.SpouseFirstName,
                    facebookLinkModel.SpouseLastName, facebookLinkModel.CurrentCity);
            }
        }

        #endregion
    }
}
