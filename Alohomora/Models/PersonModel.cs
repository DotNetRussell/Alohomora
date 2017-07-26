using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Models
{
    public class PersonModel : ModelBase
    {
        private Guid _Id;

        public Guid Id
        {
            get { return _Id; }
            set { _Id = value; OnPropertyChanged("Id"); }
        }

        private string _firstName;

        public string firstname
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged("firstname"); }
        }

        private string _lastname;

        public string lastname
        {
            get { return _lastname; }
            set { _lastname = value; OnPropertyChanged("lastname"); }
        }

        private string _address;

        public string address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged("address"); }
        }

        private string _city;

        public string city
        {
            get { return _city; }
            set { _city = value; OnPropertyChanged("city"); }
        }

        private string _state;

        public string state
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged("state"); }
        }

        private string _dob;

        public string dob
        {
            get { return _dob; }
            set { _dob = value; OnPropertyChanged("dob"); }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); }
        }

        public string FormatedName
        {
            get
            {
                return string.Format("{0} {1}", firstname, lastname);
            }
        }


    }
}
