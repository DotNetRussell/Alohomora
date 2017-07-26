using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Models.piplModels
{
    public class Job
    {
        public string title { get; set; }
        public string organization { get; set; }
        public string industry { get; set; }
        public DateRange date_range { get; set; }
        public string display { get; set; }
    }
}
