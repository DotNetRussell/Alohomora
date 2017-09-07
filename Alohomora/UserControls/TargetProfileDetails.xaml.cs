using Alohomora.Models;
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

namespace Alohomora.UserControls
{
    public partial class TargetProfileDetails : UserControl
    {
        /// <summary>
        /// Initializes a new instance of Target Profile Details
        /// </summary>
        /// <param name="targetProfile">The target profile to be displayed</param>
        public TargetProfileDetails(PersonModel targetProfile)
        {
            InitializeComponent();
            this.DataContext = new TargetProfileViewModel(targetProfile);
        }
    }
}
