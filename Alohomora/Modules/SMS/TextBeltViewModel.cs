using Alohomora.Utilities;
using Alohomora.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Alohomora.Modules.SMS
{
    public class TextBeltViewModel : ViewModelBase
    {
        private string _phoneNumber;

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged("PhoneNumber"); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged("Message"); }
        }

        private string _apiKey;

        public string APIKey
        {
            get { return _apiKey; }
            set { _apiKey = value; OnPropertyChanged("APIKey"); }
        }

        public ICommand SendMessageCommand { get; set; }

        public TextBeltViewModel()
        {
            SendMessageCommand = new ButtonCommand(CanExecuteSendMessage, SendMessageExecuted);
        }

        public bool CanExecuteSendMessage(object args)
        {
            return !String.IsNullOrEmpty(PhoneNumber) 
                && !String.IsNullOrEmpty(Message) 
                && !String.IsNullOrEmpty(APIKey);
        }

        public void SendMessageExecuted(object args)
        {
            TextBeltStuff.SendText(PhoneNumber, Message, APIKey);
        }
    }
}
