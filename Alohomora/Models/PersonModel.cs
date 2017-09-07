using Alohomora.Utilities;
using Alohomora.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Input;

namespace Alohomora.Models
{
    public class PersonModel : ModelBase
    {
        private Guid _personId = Guid.NewGuid();
        private string _fullName;
        private string _imageUrl;
        private ObservableCollection<string> _jobs;
        private ObservableCollection<string> _schools;
        private ObservableCollection<string> _details;
        private ObservableCollection<string> _addresses;
        private ObservableCollection<string> _imageUrls;
        private ObservableCollection<string> _names;
        private ObservableCollection<string> _phoneNumbers;
        private ObservableCollection<string> _dobs;
        private ObservableCollection<string> _notes;
        public string Name
        {
            get { return _names.FirstOrDefault(); }
        }
        public string ImageUrl
        {
            get { return _imageUrls.FirstOrDefault(); }
        }
        public string Dob
        {
            get { return _dobs.FirstOrDefault(); }
        }

        public ObservableCollection<string> Names
        {
            get { return _names; }
            set { _names = value; }
        }

        public ObservableCollection<string> Dobs
        {
            get { return _dobs; }
            set { _dobs = value; }
        }

        public ObservableCollection<string> PhoneNumbers
        {
            get { return _phoneNumbers; }
            set { _phoneNumbers = value; }
        }

        public ObservableCollection<string> ImageUrls
        {
            get { return _imageUrls; }
            set { _imageUrls = value; }
        }

        public ObservableCollection<string> Addresses
        {
            get { return _addresses; }
            set { _addresses = value; }
        }

        public ObservableCollection<string> Details
        {
            get { return _details; }
            set { _details = value; }
        }

        public ObservableCollection<string> Schools
        {
            get { return _schools; }
            set { _schools = value; }
        }

        public ObservableCollection<string> Jobs
        {
            get { return _jobs; }
            set { _jobs = value; }
        }

        public ObservableCollection<string> Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        public Guid PersonId
        {
            get { return _personId; }
        }

        public int JobsCount
        {
            get
            {
                return _jobs.Count;
            }
        }

        public int SchoolsCount
        {
            get
            {
                return _schools.Count;
            }
        }

        public int AddressesCount
        {
            get
            {
                return _addresses.Count;
            }
        }

        public int PhoneNumbersCount
        {
            get
            {
                return _phoneNumbers.Count;
            }
        }        

        public PersonModel()
        {
            _jobs = new ObservableCollection<string>();
            _schools = new ObservableCollection<string>();
            _details = new ObservableCollection<string>();
            _addresses = new ObservableCollection<string>();
            _imageUrls = new ObservableCollection<string>();
            _names = new ObservableCollection<string>();
            _dobs = new ObservableCollection<string>();
            _phoneNumbers = new ObservableCollection<string>();
            _notes = new ObservableCollection<string>();

        }        
    }
}
