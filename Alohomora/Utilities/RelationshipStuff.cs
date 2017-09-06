using Alohomora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alohomora.Utilities
{
    public static class RelationshipStuff
    {
        public static double GetConfidenceLevel(DBLinkModel targetModel, string source)
        {
            double confidenceLevel = 0.0;
            string realName =  FacebookStuff.GetRealNameFromProfilePage(source);            
            List<string> details = FacebookStuff.GetIntroFromAuthenticatedProfilePage(source);
            List<string> texts = FacebookStuff.GetTextFromDetailsAuthenticated(details);
            confidenceLevel += ScoreRealName(targetModel, realName);
            confidenceLevel += ScoreCityState(targetModel, texts);

            return confidenceLevel;
        }
        
        private static double ScoreCityState(DBLinkModel personModel, List<string> details)
        {
            double returnScore = 0.0;

            string personState = personModel.state.ToLower();
            string personCity = personModel.city.ToLower();

            bool cityFound = false;
            bool stateFound = false;

            int stateKeywordCount = 0;
            int cityKeywordCount = 0;

            foreach(string detail in details)
            {
                if (detail.ToLower().Contains(personState))
                {
                    stateFound = true;
                    stateKeywordCount++;
                }
            }

            foreach (string detail in details)
            {
                if (detail.ToLower().Contains(personCity))
                {
                    cityFound = true;
                    stateKeywordCount++;
                }
            }

            if(cityFound && stateFound)
            {
                returnScore += .7;
            }
            else if (cityFound && !stateFound)
            {
                returnScore += .4;
            }
            else if(stateFound && !cityFound)
            {
                returnScore += .1;
            }

            returnScore += (stateKeywordCount * .01) + (cityKeywordCount*2);
            return returnScore;
        }        

        private static double ScoreRealName(DBLinkModel personModel, string realname)
        {
            double returnScore = 0.0;

            string fname = personModel.firstname.ToLower();
            string lname = personModel.lastname.ToLower();

            string[] names = realname.Split(' ');
            foreach(string name in names)
            {
                if (name.ToLower().Contains(fname))
                {
                    returnScore += .15;
                }
                if (name.ToLower().Contains(lname))
                {
                    returnScore += .15;
                }
            }

            return returnScore;
        }
    }
}
