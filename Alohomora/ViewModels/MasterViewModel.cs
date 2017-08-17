using Alohomora.Models;
using Alohomora.UserControls;
using Alohomora.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Alohomora.ViewModels
{
    public class MasterViewModel : ViewModelBase
    {
        public ObservableCollection<Module> Modules { get; set; }
        public DependencyObject Content  { get; set; }


        private int _selectedSearchIndex;
        public event RoutedEventHandler OnMainColumnChanged;
        public delegate void MainColumnChanged(object sender, RoutedEventArgs e);

        public class MainColumnChangedEventArgs : RoutedEventArgs
        {
            public int MainColumnIndex { get; set; }
        }

        IEnumerable<Type> GetTypesWith<ITypeAttribute>(bool inherit)
                              where ITypeAttribute : System.Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   where t.IsDefined(typeof(ITypeAttribute), inherit)
                   select t;
        }

        public MasterViewModel()
        {
            Modules = new ObservableCollection<Module>();

            Content = new BrowserControl();
            OnPropertyChanged("Content");
            Modules.Add(new Module()
            {
                IconPath = "/Images/home_icon.png",
                NavCommand = new ButtonCommand(new Predicate<object>(obj => { return true; }), new Action<object>(obj => { this.Content = new BrowserControl(); OnPropertyChanged("Content"); }))
            });
            

            List<Type> modules = new List<Type>(GetTypesWith<ModuleEntryPoint>(true));

            foreach(Type module in modules)
            {
                string iconPath = module.CustomAttributes
                    .FirstOrDefault().NamedArguments.Where(n => n.MemberName.Contains("ModuleIconPath"))
                    .FirstOrDefault().TypedValue.Value.ToString();

                var _content = Activator.CreateInstance(module) as DependencyObject;
                Module _module = new Module()
                {
                    IconPath = iconPath,
                    Content = _content,
                    NavCommand = new ButtonCommand(new Predicate<object>(obj => { return true; }), new Action<object>(obj => { this.Content = _content; OnPropertyChanged("Content"); }))
                };
                Modules.Add(_module);
            }
        }

        /// <summary>
        /// I did this stupid thing with the navigation panel because I was to lazy to do it the correct way.
        /// Now it looks like garbage and it took me just as long to do if not longer....
        /// Don't do this... Don't be Anthony...
        /// </summary>
        public int SelectedSearchIndex
        {
            get { return _selectedSearchIndex; }
            set {
                    _selectedSearchIndex = value;
                    OnPropertyChanged("SelectedSearchIndex");
                    OnMainColumnChanged(this, (new MainColumnChangedEventArgs() { MainColumnIndex = value }));
                }
        }
    }

    public class Module
    {
        private string _iconPath;
        public string IconPath {
            get
            {
                return _iconPath;
            }
            set
            {
                _iconPath = value;
                SetImageData(File.ReadAllBytes(Directory.GetCurrentDirectory() + _iconPath));

            }
        }
        public ImageSource Source { get; set; }
        public ICommand NavCommand { get; set; }
        public DependencyObject Content { get; set; }

        public void SetImageData(byte[] data)
        {
            var source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(data);
            source.EndInit();

            Source = source;
        }

    }
}
