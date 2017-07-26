using Alohomora.Models.piplModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Models
{
    public class FacebookLinkModel : ModelBase
    {
        private Guid _TargetId;

        public Guid TargetId
        {
            get { return _TargetId; }
            set { _TargetId = value; OnPropertyChanged("TargetId"); }
        }

        private string _profileImage;

        public string ProfileImage
        {
            get { return _profileImage; }
            set { _profileImage = value; OnPropertyChanged("ProfileImage"); }
        }

        private string _highschool;

        public string HighSchool
        {
            get { return _highschool; }
            set { _highschool = value; OnPropertyChanged("HighSchool"); }
        }

        private string _college;

        public string College
        {
            get { return _college; }
            set { _college = value; OnPropertyChanged("College"); }
        }

        private string _currentCity;

        public string CurrentCity
        {
            get { return _currentCity; }
            set { _currentCity = value; OnPropertyChanged("CurrentCity"); }
        }

        private string _currentState;

        public string CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; OnPropertyChanged("CurrentState"); }
        }

        private string _oroginCity;

        public string OriginCity
        {
            get { return _oroginCity; }
            set { _oroginCity = value; OnPropertyChanged("OriginCity"); }
        }

        private string _originState;

        public string OriginState
        {
            get { return _originState; }
            set { _originState = value; OnPropertyChanged("OriginState"); }
        }

        private string _targetUrl;

        public string TargetUrl
        {
            get { return _targetUrl; }
            set { _targetUrl = value; OnPropertyChanged("TargetUrl"); }
        }

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; OnPropertyChanged("DisplayName"); }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged("FirstName"); }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged("LastName"); }
        }

        private string _marriedTo;

        public string MarriedTo
        {
            get { return _marriedTo; }
            set { _marriedTo = value; OnPropertyChanged("MarriedTo"); }
        }

        private string _spouseFirstName;

        public string SpouseFirstName
        {
            get { return _spouseFirstName; }
            set { _spouseFirstName = value; OnPropertyChanged("SpouseFirstName"); }
        }

        private string _spouseLastName;

        public string SpouseLastName
        {
            get
            { return _spouseLastName; }
            set { _spouseLastName = value; OnPropertyChanged("SpouseLastName"); }
        }


        private PersonModel _voterDBMatch;

        public PersonModel VoterDBMatch
        {
            get { return _voterDBMatch; }
            set { _voterDBMatch = value; OnPropertyChanged("VoterDBMatch"); }
        }

        private PersonModel _spouseVoterDBMatch;

        public PersonModel SouseVoterDBMatch
        {
            get { return _spouseVoterDBMatch; }
            set { _spouseVoterDBMatch = value; OnPropertyChanged("SpouseVoterDBMatch"); }
        }
        
        private List<PersonModel> _possibleLinks;

        public List<PersonModel> PossibleLinks
        {
            get { return _possibleLinks; }
            set { _possibleLinks = value; OnPropertyChanged("PossibleLinks"); }
        }

        private List<PersonModel> _possibleSpouseLinks;

        public List<PersonModel> PossibleSpouseLinks
        {
            get { return _possibleSpouseLinks; }
            set { _possibleSpouseLinks = value; OnPropertyChanged("PossibleSpouseLinks"); }
        }
        
        private string _targetSource;

        public string TargetSource
        {
            get { return _targetSource; }
            set { _targetSource = value; OnPropertyChanged("TargetSource"); }
        }

        private List<String> _jobs;

        public List<String> Jobs
        {
            get { return _jobs; }
            set { _jobs = value; OnPropertyChanged("Jobs"); }
        }

        private List<String> _targetDetails;

        public List<String> TargetDetails
        {
            get { return _targetDetails; }
            set { _targetDetails = value; OnPropertyChanged("TargetDetails"); }
        }

        private List<String> _parsedDetails;

        public List<String> ParsedDetails
        {
            get { return _parsedDetails; }
            set { _parsedDetails = value; OnPropertyChanged("ParsedDetails"); }
        }

        private Person[] _piplLinks;

        public Person[] PiplLinks
        {
            get { return _piplLinks; }
            set { _piplLinks = value; OnPropertyChanged("PiplLinks"); }
        }


        public FacebookLinkModel()
        {
            TargetId = Guid.NewGuid();
        }
    }
}
