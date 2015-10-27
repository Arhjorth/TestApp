using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TestApp.Model;

namespace TestApp.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {   
        public ObservableCollection<Class> Classes { get; set; }
        public ICommand CommandAddClass { get; }


        public MainViewModel() {
            Classes = new ObservableCollection<Class>();
            CommandAddClass = new RelayCommand(AddClass);
        }

        public void AddClass() {
            Classes.Add(new Class());
        }
    }
}