using Alohomora.Models;
using Alohomora.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Alohomora.ViewModels
{
    /// <summary>
    /// The SearchVoterDatabase viewmodel handles all logic for searching the voter database
    /// </summary>
    public class SearchVoterDatabaseViewModel : ViewModelBase
    {
        #region Private Variables
        private bool _searchingDb = false;
        private string _firstName = String.Empty;
        private string _lastName = String.Empty;
        private bool _UseDateOfBirth;
        private DateTime _dateOfBirth;
        private string _streetAddress = String.Empty;
        private string _city = String.Empty;
        private string _zipCode = String.Empty;
        private bool _dobEqual = true;
        private bool _dobAfter;
        private bool _dobBefore;
        #endregion

        /// <summary>
        /// A collection of rows returned from a database query
        /// </summary>
        public ObservableCollection<DBLinkModel> DatabaseSearchResults { get; set; }

        /// <summary>
        /// A collection of targets returned from scraping facebook
        /// </summary>
        public ObservableCollection<UsernameLinkModel> TargetLinks { get; set; }

        /// <summary>
        /// A collection of the top targets returned from scraping facebook
        /// </summary>
        public ObservableCollection<LinkModel> TopTargets { get; set; }

        /// <summary>
        /// Command for commiting a database search
        /// </summary>
        public ICommand SearchDatabaseCommand { get; set; }

        /// <summary>
        /// Command for commiting a facebook scrape session
        /// </summary>
        public ICommand ScanFacebookCommand { get; set; }

        /// <summary>
        /// First name in the DB search field
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged("FirstName"); }
        }

        /// <summary>
        /// Last name in the DB search field
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged("LastName"); }
        }

        /// <summary>
        /// Use DOB flag
        /// </summary>
        public bool UseDateOfBirth
        {
            get { return _UseDateOfBirth; }
            set { _UseDateOfBirth = value; OnPropertyChanged("UseDateOfBirth"); }
        }

        /// <summary>
        /// DOB in the DB search field
        /// </summary>
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; OnPropertyChanged("DateOfBirth"); }
        }

        /// <summary>
        /// Street Address in the DB search field
        /// </summary>
        public string StreetAddress
        {
            get { return _streetAddress; }
            set { _streetAddress = value; OnPropertyChanged("StreetAddress"); }
        }

        /// <summary>
        /// City in the DB search field
        /// </summary>
        public string City
        {
            get { return _city; OnPropertyChanged("City"); }
            set { _city = value; }
        }

        /// <summary>
        /// Zipcode in the DB search field
        /// 
        /// NOTE NOT ENABLED....
        /// </summary>
        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; OnPropertyChanged("ZipCode"); }
        }

        /// <summary>
        /// DOB Equal flag in DB search field
        /// </summary>
        public bool DobEqual
        {
            get { return _dobEqual; }
            set { _dobEqual = value; OnPropertyChanged("DobEqual"); }
        }

        /// <summary>
        /// DOB After flag in DB search field
        /// </summary>
        public bool DobAfter
        {
            get { return _dobAfter; }
            set { _dobAfter = value; OnPropertyChanged("DobAfter"); }
        }

        /// <summary>
        /// DOB Before flag in the DB search field
        /// </summary>
        public bool DobBefore
        {
            get { return _dobBefore; }
            set { _dobBefore = value; OnPropertyChanged("DobBefore"); }
        }

        /// <summary>
        /// Selected Person Model in 
        /// </summary>
        public DBLinkModel SelectedPerson { get; set; }

        public SearchVoterDatabaseViewModel()
        {
            DateOfBirth = DateTime.Now;
            DatabaseSearchResults = new ObservableCollection<DBLinkModel>();
            TopTargets = new ObservableCollection<LinkModel>();
            TargetLinks = new ObservableCollection<UsernameLinkModel>();
            SearchDatabaseCommand = new ButtonCommand(CanExecuteSearchCommand, SearchCommandExecuted);
            ScanFacebookCommand = new ButtonCommand(CanExecuteScanFacebook, ScanFacebookExecuted);
        }

        public bool CanExecuteSearchCommand(object param)
        {
            return !_searchingDb;
        }

        public async void SearchCommandExecuted(object param)
        {
            _searchingDb = true;
            DatabaseSearchResults.Clear();

            if (String.IsNullOrEmpty(FirstName.Trim()))
            {
                FirstName = String.Empty;
            }
            if (String.IsNullOrEmpty(LastName.Trim()))
            {
                FirstName = String.Empty;
            }
            if (String.IsNullOrEmpty(StreetAddress.Trim()))
            {
                StreetAddress = String.Empty;
            }
            if (String.IsNullOrEmpty(City.Trim()))
            {
                City = String.Empty;
            }
            if (String.IsNullOrEmpty(ZipCode.Trim()))
            {
                ZipCode = String.Empty;
            }

            Parallel.ForEach((await DatabaseStuff.RunQuery(FirstName, LastName,
               (UseDateOfBirth ? DateOfBirth : DateTime.MinValue), DobEqual, DobAfter, DobBefore, StreetAddress, City, ZipCode)), personModel =>
                   Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                       DatabaseSearchResults.Add(personModel)
                       ))
                );
            _searchingDb = false;
        }

        public bool CanExecuteScanFacebook(object param)
        {
            return DatabaseSearchResults.Where(pm => pm.IsSelected).Count() > 0; //SelectedPerson != null;
        }

        public static UsernameLinkModel GetUsernameLinkModel(Object datacontext, Guid id)
        {
            UsernameLinkModel ret = null;
            SearchVoterDatabaseViewModel vm = datacontext as SearchVoterDatabaseViewModel;

            if (vm != null)
            {
                ret = vm.TargetLinks.Where(t => t.id == id).FirstOrDefault();
            }

            return ret;
        }
        
        public void ScanFacebookExecuted(object param)
        {

            // Okay I apologize in advance for this shit.... Read on if you dare...
            // 
            // Purpose of this block of code: 
            // To scrape facebook as fast as possible, asynchronously.
            //
            // How it's achieved: 
            // Parallel foreach loops iterating over urls and async web requests
            //
            // Why this is garbage:
            // Parallel foreach is used to iterate over the urls as fast as possible. However, I'm using
            // async web requests inside of the parallel loops. Unfortunately this breaks C# apparently. 
            // When you await an async request inside of a parallel foreach, the parallel loop is no longer
            // blocking and exits immediately. This means that that part at the bottom, the part that updates the
            // UI, will run prior to the results being returned.... Yeah... fml
            //
            // How I worked around this:
            // I create a List<Object>. When a request goes out we add one to the list. When a response comes back
            // we remove one from the list. At the base of the function we have an infinite non-blocking loop that
            // waits for the requests count to equal zero. When it's zero it sorts our list based on confidence score
            // and updates the UI.
            //
            // Problems with this method:
            // Currently I'm not accounting for failed requests.... I have no idea what would happen. Probably nothing good.
            // Like seriously... I don't expect the application to suddenly get better if a request fails... Fix this... 

            List<Object> requests = new List<Object>();

            List<UsernameLinkModel> tempLinks = new List<UsernameLinkModel>();
            try
            {
                IEnumerable<DBLinkModel> tmpCache = DatabaseSearchResults.Where(pm => pm.IsSelected);
                foreach (var item in tmpCache)
                {
                    var temp = TargetLinks.Where(tl => tl.id == item.Id && tl.usernames.Count > 0);
                    if (temp.Count() > 0)
                    {
                        tempLinks.Add(temp.FirstOrDefault());
                        tmpCache = tmpCache.Where(tl => tl.Id != item.Id);
                    }
                }

                TargetLinks.Clear();
                TopTargets.Clear();

                object outerLock = new object();
                object innerLock = new object();

                // First we run over our list of voter db targets
                Parallel.ForEach(tmpCache, async pm =>
                {
                    DBLinkModel target = pm;

                    Console.WriteLine("Running search on target: " + target.firstname + " " + target.lastname);

                    lock (outerLock)
                    {
                        requests.Add(new Object());
                    }

                    // foreach voter db target, try and see if there are any facebook users in ohio with that name
                    string source = await ExternalBrowser.CallExternalBrowser(FacebookStuff.FormatQueryURL(target), true);

                    // lock adding and removing requests, because you know, threads, shared resources, fml
                    lock (outerLock)
                    {
                        requests.RemoveAt(requests.Count - 1);
                    }

                    // Link made
                    TargetLinks.Add(FacebookStuff.ExtractUsernamesFromSource(target, source));
                    
                    if (requests.Count == 0)
                    {
                        // For each link made
                        Parallel.ForEach(TargetLinks, targetLink =>
                        {
                            // Run over every possible user and score it
                            Parallel.ForEach(targetLink.usernames, async username =>
                            {
                                lock (innerLock)
                                {
                                    requests.Add(new Object());
                                }

                                // Get the source for this possible facebook match
                                string profileSource = await ExternalBrowser.CallExternalBrowser(username.ProfileLink);

                                lock (innerLock)
                                {
                                    requests.RemoveAt(requests.Count - 1);
                                }

                                // Parse and score the source
                                username.ConfidenceScore = RelationshipStuff.GetConfidenceLevel(SelectedPerson, profileSource);
                                username.UserModelLinkId = targetLink.id;
                                username.FullName = targetLink.name;

                                // wait until alllllllll the requests are done. If you're curious why in 2017 I need to do this, read the
                                // above giant comment block
                                while (requests.Count > 0)
                                {
                                    System.Windows.Forms.Application.DoEvents();
                                }

                                Console.WriteLine("SCAN COMPLETED AT: " + DateTime.Now.ToString());
                                Sort();
                            });

                        });
                    }
                });


            }
            catch (Exception err) { Console.WriteLine(err.Message); /*Nom nom nom*/ }


        }

        /// <summary>
        /// This function will sort the targets by confidence score
        /// </summary>
        private void Sort()
        {
            foreach (var _targetLink in TargetLinks)
            {
                //If our targetlink has no usernames then skip
                if (_targetLink.usernames.Count != 0)
                {
                    //Find the top scored target
                    LinkModel topTarget = null;

                    foreach (LinkModel username in _targetLink.usernames)
                    {
                        if (topTarget == null)
                        {
                            topTarget = username;
                        }
                        else if (topTarget.ConfidenceScore < username.ConfidenceScore)
                        {
                            topTarget = username;
                        }
                    }

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        LinkModel currentTopTarget = TopTargets.Where(tt => tt.UserModelLinkId == topTarget.UserModelLinkId).FirstOrDefault();

                        if (currentTopTarget != null)
                        {
                            TopTargets.Remove(currentTopTarget);
                        }

                        TopTargets.Add(topTarget);
                    }));

                }
            }
        }

    }
}
