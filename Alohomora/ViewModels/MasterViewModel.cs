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

            Content = new TargetProfileDashboard();

            Window _cachedWindow = Application.Current.MainWindow;

            OnPropertyChanged("Content");
            Modules.Add(new Module()
            {
                IconPath = "/Images/home_icon.png",
                NavCommand = new ButtonCommand(new Predicate<object>(obj => { return true; }), 
                new Action<object>(obj => 
                {
                    Window _window = new Window();
                    _window.Content = new BrowserControl();
                    _window.Owner = _cachedWindow;
                    _window.MinHeight = 500;
                    _window.MinWidth = 500;
                    _window.SizeToContent = SizeToContent.WidthAndHeight;
                    _window.Show();
                }))
            });
            

            List<Type> modules = new List<Type>(GetTypesWith<ModuleEntryPoint>(true));

            foreach(Type module in modules)
            {
                string iconPath = module.CustomAttributes
                    .FirstOrDefault().NamedArguments.Where(n => n.MemberName.Contains("ModuleIconPath"))
                    .FirstOrDefault().TypedValue.Value.ToString();

                DependencyObject _content = Activator.CreateInstance(module) as DependencyObject;
                Module _module = new Module()
                {
                    IconPath = iconPath,
                    Content = _content,
                    NavCommand = new ButtonCommand(
                        new Predicate<object>(obj => 
                                                { return true; }), 
                                                new Action<object>(obj => 
                                                                    {
                                                                        Window _window = new Window();
                                                                        _window.Content = _content;
                                                                        _window.Owner = _cachedWindow;
                                                                        _window.MinHeight = 500;
                                                                        _window.MinWidth = 500;
                                                                        _window.SizeToContent = SizeToContent.WidthAndHeight;
                                                                        _window.Show();
                                                                    }))
                };
                Modules.Add(_module);
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
