using Alohomora.ViewModels;
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

namespace Alohomora.Modules.RegexTweeker
{
    [ModuleEntryPoint(ModuleIconPath = "/Modules/RegexTweeker/Images/module_icon.png")]
    public partial class RegexTweeker : UserControl
    {
        public RegexTweeker()
        {
            InitializeComponent();
            this.DataContext = new RegexTweekerViewModel();
        }
    }
}
