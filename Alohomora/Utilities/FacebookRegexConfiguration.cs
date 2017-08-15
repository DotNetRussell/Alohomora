using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora.Utilities
{
    public static class FacebookRegexConfiguration
    {
        public static string FacebookQueryURL { get; set; }
        public static string ExtractUserNamesFromProfilePageRegex { get; set; }
        public static string GetRealNameFromProfilePageRegex { get; set; }
        public static string GetProfilePhotoFromProfilePageRegex { get; set; }
        public static string GetProfilePhotoFromProfilePageSecondPassRegex { get; set; }
        public static string GetProfileIntroCardFromProfilePageRegex { get; set; }
        public static string GetDetailsFromIntroCardRegex { get; set; }
        public static string GetDetailsFromIntroCardSecondPassRegex { get; set; }
        public static string GetLinksFromFindFriendsPageRegex { get; set; }

        private static string _facebookURLDefault = "https://www.facebook.com/search/people/?q=";
        private static string _extractUsernamesDefault = "(?<=_ajx\"><div><a href=\").*?(?=\" data-testid=\"serp_result)";
        private static string _getRealNameDefault = "(?<=profile_name_in_profile_page\">).*?(?=</span)";
        private static string _getProfilePhotoDefault = @"(?<=profilePicThumb"">).*?(?="">)";
        private static string _getProfilePhotoSecondPassDefault = @"(?<=src="").*";
        private static string _getProfileIntroCardDefault = "(?<=data-profile-intro-car).*?(?=</div>)";
        private static string _getDetailsFromIntroCardDefault = "(?<=data-hovercard-prefer-more-content-show=\").*?(?=</a>)";
        private static string _getDetailsFromIntroCardSecondPassDefault = "(?<=>).*";
        private static string _getLinksFromFindFriendsDefault = "(?<=friendBrowserNameTitle fsl fwb fcb).*?(?=friendBrowserMarginTopMini)";

        public static void LoadDefaults()
        {
            FacebookQueryURL = _facebookURLDefault;
            ExtractUserNamesFromProfilePageRegex = _extractUsernamesDefault;
            GetRealNameFromProfilePageRegex = _getRealNameDefault;
            GetProfilePhotoFromProfilePageRegex = _getProfilePhotoDefault;
            GetProfilePhotoFromProfilePageSecondPassRegex = _getProfilePhotoSecondPassDefault;
            GetProfileIntroCardFromProfilePageRegex = _getProfileIntroCardDefault;
            GetDetailsFromIntroCardRegex = _getDetailsFromIntroCardDefault;
            GetDetailsFromIntroCardSecondPassRegex = _getDetailsFromIntroCardSecondPassDefault;
            GetLinksFromFindFriendsPageRegex = _getLinksFromFindFriendsDefault;
        }

        public static bool LoadCustomRegexes()
        {
            if (File.Exists("fb_regex_config.json"))
            {
                string json = File.ReadAllText("fb_regex_config.json");
                dynamic jobject = JsonConvert.DeserializeObject<dynamic>(json);
                FacebookQueryURL = ((string)jobject.config.facebook_url).Replace("\\\"", "\"");
                ExtractUserNamesFromProfilePageRegex = ((string)jobject.config.extract_usernames_profilepage).Replace("\\\"", "\"");
                GetRealNameFromProfilePageRegex = ((string)jobject.config.get_realname_profilepage).Replace("\\\"", "\"");
                GetProfilePhotoFromProfilePageRegex = ((string)jobject.config.get_profile_photo_one).Replace("\\\"", "\"");
                GetProfilePhotoFromProfilePageSecondPassRegex = ((string)jobject.config.get_profile_photo_two).Replace("\\\"", "\"");
                GetProfileIntroCardFromProfilePageRegex = ((string)jobject.config.get_profile_introcard).Replace("\\\"", "\"");
                GetDetailsFromIntroCardRegex = ((string)jobject.config.get_details_one).Replace("\\\"", "\"");
                GetDetailsFromIntroCardSecondPassRegex = ((string)jobject.config.get_details_two).Replace("\\\"", "\"");
                GetLinksFromFindFriendsPageRegex = ((string)jobject.config.get_links).Replace("\\\"", "\"");

                return true;
            }
            else
            {
                return false;
            }
        }

        public static void SaveCustomRegexes(string fburl, string extractUserNames, string getRealNames, string getProfilePhotoOne,
                                                string getProfilePhotoTwo, string getProfileIntro, string getDetailsFromIntroOne,
                                                string getDetailsFromIntroTwo, string getLinksFromFindFriends)
        {
            string json = String.Format("{{ \"config\": {{ \"facebook_url\": \"{0}\",\"extract_usernames_profilepage\": \"{1}\",\"get_realname_profilepage\":\"{2}\",\"get_profile_photo_one\":\"{3}\",\"get_profile_photo_two\":\"{4}\",\"get_profile_introcard\":\"{5}\",\"get_details_one\":\"{6}\",\"get_details_two\":\"{7}\",\"get_links\":\"{8}\"}} }}", fburl.Replace("\"","\\\""), extractUserNames.Replace("\"", "\\\""), getRealNames.Replace("\"", "\\\""), getProfilePhotoOne.Replace("\"", "\\\""), getProfilePhotoTwo.Replace("\"", "\\\""), getProfileIntro.Replace("\"", "\\\""), getDetailsFromIntroOne.Replace("\"", "\\\""), getDetailsFromIntroTwo.Replace("\"", "\\\""), getLinksFromFindFriends.Replace("\"", "\\\""));
            File.Delete("fb_regex_config.json");
            File.AppendAllText("fb_regex_config.json", json);
            LoadCustomRegexes();
        }
    }
}
