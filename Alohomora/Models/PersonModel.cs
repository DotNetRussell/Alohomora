using Alohomora.Utilities;
using Alohomora.ViewModels;
using System;
using System.Collections.Generic;
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

        private List<string> _jobs;

        private List<string> _schools;

        private List<string> _details;

        private List<string> _addresses;

        private List<string> _imageUrls;

        private List<string> _names;

        private List<string> _phoneNumbers;

        private List<string> _dobs;

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

        public List<string> Names
        {
            get { return _names; }
            set { _names = value; }
        }

        public List<string> Dobs
        {
            get { return _dobs; }
            set { _dobs = value; }
        }

        public List<string> PhoneNumbers
        {
            get { return _phoneNumbers; }
            set { _phoneNumbers = value; }
        }

        public List<string> ImageUrls
        {
            get { return _imageUrls; }
            set { _imageUrls = value; }
        }

        public List<string> Addresses
        {
            get { return _addresses; }
            set { _addresses = value; }
        }

        public List<string> Details
        {
            get { return _details; }
            set { _details = value; }
        }

        public List<string> Schools
        {
            get { return _schools; }
            set { _schools = value; }
        }

        public List<string> Jobs
        {
            get { return _jobs; }
            set { _jobs = value; }
        }

        private List<string> _notes;

        public List<string> Notes
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
        
        [ScriptIgnore]
        public ICommand DeleteItemCommand { get; set; }

        public PersonModel()
        {
            _jobs = new List<string>();
            _schools = new List<string>();
            _details = new List<string>();
            _addresses = new List<string>();
            _imageUrls = new List<string>();
            _names = new List<string>();
            _dobs = new List<string>();
            _phoneNumbers = new List<string>();
            _notes = new List<string>();

            DeleteItemCommand = new ButtonCommand(CanDeleteItem, DeleteItemExecuted);
        }

        public bool CanDeleteItem(Object args)
        {
            return true;
        }

        public void DeleteItemExecuted(Object args)
        {
            PersonModel personModel = args as PersonModel;

            if (personModel != null)
            {
                MessageBoxResult result = MessageBox.Show("Delete Target Profile?", "Are you sure you'd like to delete this target profile?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if(result == MessageBoxResult.Yes)
                {
                    MasterTargetListViewModel.RemoveTarget(personModel);
                }
            }
        }

    }
}
