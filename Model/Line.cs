using GalaSoft.MvvmLight;
using System.Windows;

namespace TestApp.Model {
    public class Line : ViewModelBase {

        public ClassBox fromBox;
        public ClassBox toBox;
        public int cF;
        public int cT;

        public Point ConnectFrom {
            get { return fromBox.getPoint(cF); }

        }

        public Point ConnectTo {
            get { return toBox.getPoint(cT); }
        }

        public void raisePropertyChanged() {
            RaisePropertyChanged(nameof(ConnectFrom));
            RaisePropertyChanged(nameof(ConnectTo));
        }
    }

}
