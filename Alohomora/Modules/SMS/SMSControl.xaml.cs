using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alohomora.Modules.SMS
{
    [ModuleEntryPoint(ModuleIconPath="/Modules/SMS/Images/module_icon.png")]
    public partial class SMSControl : UserControl
    {
        public SMSControl()
        {
            InitializeComponent();
            this.DataContext = new SMSViewModel();
        }
    }
}
