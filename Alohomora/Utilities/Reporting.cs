using Alohomora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Utilities
{
    public static class Reporting
    {
        public static void PrintReport(List<UsernameLinkModel> targetLinks)
        {
            Console.WriteLine("--------------REPORT-----------------");
            Console.WriteLine("Date:" + DateTime.Now);
            Console.WriteLine("");
            foreach (UsernameLinkModel link in targetLinks)
            {
                Console.WriteLine("Id: " + link.id);
                Console.WriteLine("Name: " + link.name);
                foreach (LinkModel uid in link.usernames)
                {
                    Console.WriteLine("Username: " + uid.ProfileLink);
                }
            }
            Console.WriteLine("");
        }

        public static void PrintTargetLink(UsernameLinkModel targetLink)
        {
            var localTargetLink = targetLink;
            //Console.WriteLine("");
            //Console.WriteLine(targetLink.name);
            foreach(LinkModel link in localTargetLink.usernames)
            {
                //Console.WriteLine(string.Format("Username: {0} Confidence: {1}", link.ProfileLink, link.ConfidenceScore));
                Console.WriteLine("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",targetLink.id, targetLink.name, link.ProfileLink,link.ConfidenceScore);
            }
        }
    }
}
