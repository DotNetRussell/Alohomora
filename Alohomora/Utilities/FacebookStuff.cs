using Alohomora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Alohomora.Utilities
{
    public static class FacebookStuff
    {
        public static string FormatQueryURL(PersonModel target)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(FacebookRegexConfiguration.FacebookQueryURL + target.firstname + "+" + target.lastname + "+" + target.state);
            return builder.ToString();
        }

        public static UsernameLinkModel ExtractUsernamesFromSource(PersonModel target, string source)
        {
            //Unauthenticated pattern
            //string pattern = @"(?<=<a class=""_42ft _4jy0 _4jy3 _517h _51sy"" role=""button"" href=""https:\/\/www.facebook.com\/).*?(?=\/photos"")";

            //Authenticated pattern
            string pattern = FacebookRegexConfiguration.ExtractUserNamesFromProfilePageRegex;
            string pattern2 = "(?<=class=\"_2ial).*(https:// www.facebook.*)(?=\" data-testid=\"serp_result)";
            List<LinkModel> usernames = new List<LinkModel>();
            MatchCollection matches = Regex.Matches(source, pattern);

            if (matches.Count == 0)
            {
                matches = Regex.Matches(source, pattern2);
            }

            foreach (Match m in matches)
            {
                usernames.Add(new LinkModel() { ProfileLink = m.Value, ConfidenceScore = 0 });
            }

            UsernameLinkModel linkModel = new UsernameLinkModel()
            {
                id = target.Id,
                name = target.firstname + " " + target.lastname,
                usernames = usernames
            };

            Console.WriteLine("Links Made: " + usernames.Count);
            return linkModel;
        }

        public static string GetRealNameFromProfilePage(string source)
        {
            string realNameRegex = FacebookRegexConfiguration.GetRealNameFromProfilePageRegex;
            string realName = String.Empty;
            var matches = Regex.Matches(source, realNameRegex);
            if (matches.Count > 0)
            {
                string temp = matches[0].Value;
                if (temp.Contains("<"))
                {
                    string[] split = temp.Split('<');
                    realName = split[0];
                }
                else
                {
                    realName = temp;
                }
            }

            return realName;
        }

        public static string GetProfilePhotoFromProfilePage(string source)
        {
            string profilePhotoRegex = FacebookRegexConfiguration.GetProfilePhotoFromProfilePageRegex;
            string photoUrl = String.Empty;
            var matches = Regex.Matches(source, profilePhotoRegex);
            if (matches.Count > 0)
            {
                string subPattern = FacebookRegexConfiguration.GetProfilePhotoFromProfilePageSecondPassRegex;
                var subMatch = Regex.Match(matches[0].Value, subPattern);
                photoUrl = subMatch.Value;

            }
            else
            {
                string secondTryRegex = "(?<=Profile Photo).*?(?=\"\\>)";
                var matchesSecondTry = Regex.Matches(source, secondTryRegex);
                if (matchesSecondTry.Count > 0)
                {
                    string subPattern = FacebookRegexConfiguration.GetProfilePhotoFromProfilePageSecondPassRegex;
                    var subMatch = Regex.Match(matchesSecondTry[0].Value, subPattern);
                    photoUrl = subMatch.Value;
                }
            }

            photoUrl = photoUrl.Replace("&amp;", "&");
            photoUrl = HttpUtility.HtmlDecode(photoUrl);
            return photoUrl;

        }

        public static List<string> GetIntroFromAuthenticatedProfilePage(string source)
        {
            string pattern = FacebookRegexConfiguration.GetProfileIntroCardFromProfilePageRegex;
            List<string> details = new List<string>();
            var matches = Regex.Matches(source, pattern);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    details.Add(HttpUtility.HtmlDecode(match.Value));
                }
            }
            return details;
        }

        public static List<string> GetTextFromDetailsAuthenticated(List<string> source)
        {
            string detailTextRegex = FacebookRegexConfiguration.GetDetailsFromIntroCardRegex;
            string innerTextRegex = FacebookRegexConfiguration.GetDetailsFromIntroCardSecondPassRegex;

            List<string> text = new List<string>();

            foreach (string detailItem in source)
            {
                var match = Regex.Match(detailItem, detailTextRegex);
                if (match != null)
                {
                    Match result = Regex.Match(match.Value, innerTextRegex);
                    text.Add(result.Value);
                }

            }

            return text;
        }

        public static string GetTextFromSingleDetailAuthenticated(string source)
        {
            string detailTextRegex = FacebookRegexConfiguration.GetDetailsFromIntroCardRegex;
            string innerTextRegex = FacebookRegexConfiguration.GetDetailsFromIntroCardSecondPassRegex;

            string text = String.Empty;

            var match = Regex.Match(source, detailTextRegex);
            if (match != null)
            {
                Match result = Regex.Match(match.Value, innerTextRegex);
                text = result.Value;
            }
            return text;
        }

        public static List<FacebookLinkModel> GetLinksFromFindFriends(string source)
        {
            List<FacebookLinkModel> _facebookUsers = new List<FacebookLinkModel>();
            MatchCollection matches = Regex.Matches(source, FacebookRegexConfiguration.GetLinksFromFindFriendsPageRegex);

            foreach (Match blob in matches)
            {
                FacebookLinkModel facebookLinkModel = new FacebookLinkModel();

                facebookLinkModel.TargetSource = blob.Value;

                Match url = Regex.Match(blob.Value, "(?<=href=\").*?(?=\")");

                if (url != null)
                {
                    facebookLinkModel.TargetUrl = url.Value;
                }

                Match displayName = Regex.Match(blob.Value, "(?<=/ajax/hovercard/user.php).*?(?=</a)");

                if (displayName != null)
                {
                    Match innerMatch = Regex.Match(displayName.Value, "(?<=\">).*");
                    facebookLinkModel.DisplayName = innerMatch.Value;

                    if (facebookLinkModel.DisplayName.Contains("<"))
                    {
                        int index = facebookLinkModel.DisplayName.IndexOf("<");
                        facebookLinkModel.DisplayName = facebookLinkModel.DisplayName.Substring(0, index);
                    }
                }
                _facebookUsers.Add(facebookLinkModel);
            }
            return _facebookUsers;
        }

    }
}
