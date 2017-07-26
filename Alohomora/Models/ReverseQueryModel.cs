using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Models
{
    public class ReverseQueryModel : ModelBase
    {
        private string _query;

        public string Query
        {
            get { return _query; }
            set { _query = value; OnPropertyChanged("Query"); }
        }

        private string _location;

        public string Location
        {
            get { return _location; }
            set { _location = value; OnPropertyChanged("Location"); }
        }
        
    }
}
