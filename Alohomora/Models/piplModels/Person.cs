using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Models.piplModels
{
    public class Person
    {
        public string id { get; set; }
        public List<Name> names { get; set; }
        public List<Email> emails { get; set; }
        public List<Phone> phones { get; set; }
        public Gender gender { get; set; }
        public Dob dob { get; set; }
        public List<Address> addresses { get; set; }
        public List<Job> jobs { get; set; }
        public List<Education> educations { get; set; }
        public List<Image> images { get; set; }
        public List<Url> urls { get; set; }
        public bool Selected { get; set; }

        public Person()
        {
            names = new List<Name>();
            emails = new List<Email>();
            phones = new List<Phone>();
            addresses = new List<Address>();
            jobs = new List<Job>();
            educations = new List<Education>();
            images = new List<Image>();
            urls = new List<Url>();
        }
    }
}
