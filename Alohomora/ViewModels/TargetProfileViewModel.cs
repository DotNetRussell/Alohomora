using Alohomora.Models;
using Alohomora.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Controls;
using System.Windows.Input;

namespace Alohomora.ViewModels
{
    public class TargetProfileViewModel : ViewModelBase
    {
        private string MapAPIKey = "AIzaSyCsizleFLLXn4cimI2OEua_prM8Kl9ojzQ";
        private string MapStaticAPIKey = "AIzaSyA5xv6682cpFYYdTgGb7fZcRqhn9T_MbYw";
        private string LatLong { get; set; }
        private string TargetAddress { get; set; }
        public PersonModel TargetProfile { get; private set; }
        public ICommand AddNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public string NoteText { get; set; }

        private WebBrowser _mapControl = new WebBrowser();
        public WebBrowser MapControl { get { return _mapControl; } set { _mapControl = value; } }
        public string MapUrl
        {
            get
            {
                return "https://maps.googleapis.com/maps/api/staticmap?maptype=hybrid&markers=color:blue%7Clabel:S%7C" + LatLong + "&center=" + TargetAddress.Replace(" ", "+") + "&zoom=18&size=450x450&key=" + MapStaticAPIKey;
            }
        }

        public TargetProfileViewModel(PersonModel targetModel)
        {
            if (targetModel == null)
            {
                throw new Exception("Target Profile cannot be null");
            }
            else
            {
                TargetProfile = targetModel;
                OnPropertyChanged("TargetProfile");
            }

            AddNoteCommand = new ButtonCommand(CanExecuteAddNoteText, AddNoteTextExecuted);
            DeleteNoteCommand = new ButtonCommand(CanExecuteDeleteNoteText, DeleteNoteTextExecuted);

            AlohomoraServices.RegisterService("TargetProfileViewModel", this);
        }

        private void GetLatLong(string address)
        {
            WebClient webClient = new WebClient();

            webClient.DownloadStringCompleted += (sender, args) =>
            {
                if (args.Error == null)
                {
                    dynamic locationInfo = JsonConvert.DeserializeObject(args.Result);
                    LatLong = locationInfo.results[0].geometry.location.lat + "," + locationInfo.results[0].geometry.location.lng;
                    TargetAddress = address;
                    MapControl.Source = new Uri(MapUrl);
                    OnPropertyChanged("MapControl");
                }
            };
            webClient.DownloadStringAsync(new Uri("https://maps.googleapis.com/maps/api/geocode/json?address=" + address.Replace(" ", "+") + "&key=" + MapAPIKey));

        }

        public bool CanExecuteAddNoteText(object args)
        {
            return true;
        }

        public void AddNoteTextExecuted(object args)
        {
            TargetProfile.Notes.Add(NoteText);
        }

        public bool CanExecuteDeleteNoteText(object args)
        {
            return true;
        }

        public void DeleteNoteTextExecuted(object args)
        {
            TargetProfile.Notes.Remove(args as string);
        }

        public static void SetMapLocation(string address)
        {
            TargetProfileViewModel viewModelInstance = AlohomoraServices.GetService("TargetProfileViewModel") as TargetProfileViewModel;
            if (viewModelInstance != null)
            {
                viewModelInstance.GetLatLong(address);
            }
        }
    }
}
