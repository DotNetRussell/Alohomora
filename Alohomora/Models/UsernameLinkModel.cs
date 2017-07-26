using Alohomora.Models.piplModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Models
{
    public class UsernameLinkModel : ModelBase
    {
        private Guid _id;

        public Guid id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("id"); }
        }

        private string _name;

        public string name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("name"); }
        }

        private List<LinkModel> _usernames;

        public List<LinkModel> usernames
        {
            get { return _usernames; }
            set { _usernames = value; OnPropertyChanged("usernames"); }
        }

        private double _ConfidenceLevel;

        public double ConfidenceLevel
        {
            get { return _ConfidenceLevel; }
            set { _ConfidenceLevel = value; OnPropertyChanged("ConfidenceLevel"); }
        }

    }

    public class LinkModel : ModelBase
    {
        private string _ProfileLink;

        public string ProfileLink
        {
            get { return _ProfileLink; }
            set { _ProfileLink = value; OnPropertyChanged("ProfileLink"); }
        }

        private double _ConfidenceScore;

        public double ConfidenceScore
        {
            get { return _ConfidenceScore; }
            set { _ConfidenceScore = value; OnPropertyChanged("ConfidenceScore"); }
        }

        private Guid _UserModelLinkId;

        public Guid UserModelLinkId
        {
            get { return _UserModelLinkId; }
            set { _UserModelLinkId = value; OnPropertyChanged("UserModelLinkId"); }
        }

        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged("FullName"); }
        }

        private Person[] _piplLinks;

        public Person[] PiplLinks
        {
            get { return _piplLinks; }
            set { _piplLinks = value; OnPropertyChanged("PiplLinks"); }
        }

    }
}
