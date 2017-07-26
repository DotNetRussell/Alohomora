using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Models.piplModels
{
    public class RootObject
    {
        public int __invalid_name__http_status_code { get; set; }
        public int __invalid_name__visible_sources { get; set; }
        public int __invalid_name__available_sources { get; set; }
        public string __invalid_name__search_id { get; set; }
        public Query query { get; set; }
        public Person person { get; set; }
    }
}
