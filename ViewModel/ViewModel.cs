using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using TestApp.Model;

namespace TestApp.ViewModel {

    class ViewModel : ViewModelBase {

        public ObservableCollection<Class> Classes { get; set; }

        public ViewModel() {
            Classes = new ObservableCollection<Class>();
        }

        private void addClass(){
            Classes.Add(new Class());
        }
       
    }
}