using GalaSoft.MvvmLight;
using System.Windows;
using TestApp.Model;

namespace TestApp.ViewModel {
    public class LineViewModel : ViewModelBase {

        public Line Line { get; set; }

        public LineViewModel() {
            Line = new Line();
        }
        public LineViewModel(Line _line) : base() {
            Line = _line;
        }

        public Point ConnectFrom {
            get { return Line.fromBox.getPoint(Line.cF); }

        }

        public Point ConnectTo {
            get { return Line.toBox.getPoint(Line.cT); }
        }

        public int cF {
            get { return Line.cF; }
            set { Line.cF = value; }
        }
        public int cT {
            get { return Line.cT; }
            set { Line.cT = value; }
        }

        public ClassBoxViewModel fromBox {
            get { return new ClassBoxViewModel(Line.fromBox); }
            set { Line.fromBox = value.ClassBox; }
        }

        public ClassBoxViewModel toBox {
            get { return new ClassBoxViewModel(Line.toBox); }
            set { Line.toBox = value.ClassBox; }
        }

        public void raisePropertyChanged() {
            RaisePropertyChanged(nameof(ConnectFrom));
            RaisePropertyChanged(nameof(ConnectTo));
        }

    }
}
