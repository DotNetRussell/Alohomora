using Alohomora.Models;
using Alohomora.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Alohomora.ViewModels
{
    public class TargetProfileViewModel : ViewModelBase
    {
        public PersonModel TargetProfile { get; private set; }
        public ICommand AddNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public string NoteText { get; set; }

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

            AddNoteCommand = new ButtonCommand(CanExecuteAddNoteText,AddNoteTextExecuted);
            DeleteNoteCommand = new ButtonCommand(CanExecuteDeleteNoteText, DeleteNoteTextExecuted);
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
    }
}
