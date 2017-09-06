using Alohomora.Models;
using Alohomora.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Alohomora.ViewModels
{
    public class MasterTargetListViewModel : ViewModelBase
    {
        public ObservableCollection<PersonModel> TargetProfiles { get; private set; }

        public MasterTargetListViewModel()
        {
            TargetProfiles = new ObservableCollection<PersonModel>();
            AlohomoraServices.RegisterService("targetProfileViewModel", this);
            GetPersonModels();
        }

        public static IEnumerable<PersonModel> GetTargetProfiles()
        {
            MasterTargetListViewModel masterTargetListViewModel = AlohomoraServices.GetService("targetProfileViewModel") as MasterTargetListViewModel;
            if (masterTargetListViewModel != null)
            {
                return masterTargetListViewModel.TargetProfiles;
            }
            else
            {
                return null;
            }
        }

        public static void AddTarget(PersonModel model)
        {
            MasterTargetListViewModel masterTargetListViewModel = AlohomoraServices.GetService("targetProfileViewModel") as MasterTargetListViewModel;
            if (masterTargetListViewModel != null)
            {
                bool targetExists = masterTargetListViewModel.TargetProfiles.Where(profile => profile.PersonId == model.PersonId).Count() > 0;
                if (!targetExists)
                {
                    masterTargetListViewModel.TargetProfiles.Add(model);
                    masterTargetListViewModel.OnPropertyChanged("TargetProfiles");
                }
            }
        }

        public static void RemoveTarget(PersonModel model)
        {
            MasterTargetListViewModel masterTargetListViewModel = AlohomoraServices.GetService("targetProfileViewModel") as MasterTargetListViewModel;
            if (masterTargetListViewModel != null)
            {
                bool targetExists = masterTargetListViewModel.TargetProfiles.Where(profile => profile.PersonId == model.PersonId).Count() > 0;
                if (targetExists)
                {
                    masterTargetListViewModel.TargetProfiles.Remove(model);
                    masterTargetListViewModel.OnPropertyChanged("TargetProfiles");
                }
            }
        }

        public static void SaveTargetProfiles()
        {
            MasterTargetListViewModel masterTargetListViewModel = AlohomoraServices.GetService("targetProfileViewModel") as MasterTargetListViewModel;

            string json = Serializer.PersonModelsToJson(masterTargetListViewModel.TargetProfiles);
            if (File.Exists("./cachedTargets.alohomora"))
            {
                File.Delete("./cachedTargets.alohomora");
            }
            File.AppendAllText("./cachedTargets.alohomora",json);
            File.Encrypt("./cachedTargets.alohomora");
        }

        private void GetPersonModels()
        {
            if (File.Exists("./cachedTargets.alohomora"))
            {
                File.Decrypt("./cachedTargets.alohomora");
                string json = File.ReadAllText("./cachedTargets.alohomora");
                foreach (PersonModel model in Serializer.GetPersonModels(json))
                {
                    AddTarget(model);
                }
            }
        }
    }
}
