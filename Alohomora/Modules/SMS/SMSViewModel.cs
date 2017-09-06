using Alohomora.Models;
using Alohomora.Utilities;
using Alohomora.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Alohomora.Modules.SMS
{
    public class SMSViewModel : ViewModelBase
    {
        public TextBeltViewModel TextBeltVM { get; set; }
        public ObservableCollection<PersonModel> TargetProfiles { get;set; }
        public ICommand RefreshContactsCommand { get; set; }

        public SMSViewModel()
        {
            TextBeltVM = new TextBeltViewModel();
            TargetProfiles = new ObservableCollection<PersonModel>(MasterTargetListViewModel.GetTargetProfiles());
            RefreshContactsCommand = new ButtonCommand(CanExecuteRefresh, RefreshExecuted);
        }

        public bool CanExecuteRefresh(object args)
        {
            return true;
        }

        public void RefreshExecuted(object args)
        {
            TargetProfiles = new ObservableCollection<PersonModel>(MasterTargetListViewModel.GetTargetProfiles());
            OnPropertyChanged("TargetProfiles");
        }
    }
}
