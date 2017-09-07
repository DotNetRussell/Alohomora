using Alohomora.Models;
using Alohomora.UserControls;
using Alohomora.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Alohomora.ViewModels
{
    public class MasterTargetListViewModel : ViewModelBase
    {
        public ObservableCollection<PersonModel> TargetProfiles { get; private set; }
        
        public ICommand DeleteItemCommand { get; set; }
        public ICommand TargetDetailsCommand { get; set; }

        public MasterTargetListViewModel()
        {
            TargetProfiles = new ObservableCollection<PersonModel>();
            AlohomoraServices.RegisterService("targetProfileViewModel", this);
            DeleteItemCommand = new ButtonCommand(CanDeleteItem, DeleteItemExecuted);
            TargetDetailsCommand = new ButtonCommand(CanDisplayTargetDetails, DisplayTargetDetails);
            GetPersonModels();
        }

        public bool CanDisplayTargetDetails(Object args)
        {
            return true;
        }

        public void DisplayTargetDetails(Object args)
        {
            Window _window = new Window();
            _window.Content = new TargetProfileDetails(args as PersonModel);
            _window.Owner = Application.Current.MainWindow;
            _window.MinHeight = 500;
            _window.MinWidth = 500;
            _window.SizeToContent = SizeToContent.WidthAndHeight;
            _window.Show();
        }

        public bool CanDeleteItem(Object args)
        {
            return true;
        }

        public void DeleteItemExecuted(Object args)
        {
            PersonModel personModel = args as PersonModel;

            if (personModel != null)
            {
                MessageBoxResult result = MessageBox.Show("Delete Target Profile?", "Are you sure you'd like to delete this target profile?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result == MessageBoxResult.Yes)
                {
                    MasterTargetListViewModel.RemoveTarget(personModel);
                }
            }
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
