using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TestApp.Model;

namespace TestApp.ViewModel {
    public class ClassBoxViewModel : ViewModelBase {

        public ClassBox ClassBox { get; set; }

        public ClassBoxViewModel() {
            ClassBox = new ClassBox();
        }
        public ClassBoxViewModel(ClassBox _box)  : base(){
            ClassBox = _box;
        }

        public double PosX {
            get { return ClassBox.PosX; }
            set {
                ClassBox.PosX = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanvasCenterX));
                RaisePropertyChanged(nameof(ConnectTop));
                RaisePropertyChanged(nameof(ConnectBottom));
                RaisePropertyChanged(nameof(ConnectLeft));
                RaisePropertyChanged(nameof(ConnectRight));
                raiseLinePropertyChanged();
            }
        }

        public double PosY {
            get { return ClassBox.PosY; }
            set {
                ClassBox.PosY = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanvasCenterY));
                RaisePropertyChanged(nameof(ConnectTop));
                RaisePropertyChanged(nameof(ConnectBottom));
                RaisePropertyChanged(nameof(ConnectLeft));
                RaisePropertyChanged(nameof(ConnectRight));
                raiseLinePropertyChanged();
            }
        }

        public double Height { get { return ClassBox.Height; } }
        public double Width  { get { return ClassBox.Width; } }

        public double CanvasCenterX {
            get { return PosX + ClassBox.Height / 2; }
        }

        public double CanvasCenterY {
            get { return PosY + ClassBox.Width / 2; }
        }

        public Point ConnectTop {
            get {
                ClassBox.ConnectTop = new Point(PosX + ClassBox.Width / 2 , PosY);
                RaisePropertyChanged();
                return ClassBox.ConnectTop;
            }
        }

        public Point ConnectLeft {
            get {
                ClassBox.ConnectLeft = new Point (PosX , PosY + ClassBox.Height / 2);
                RaisePropertyChanged();
                return ClassBox.ConnectLeft;
            }
        }

        public Point ConnectRight {
            get {
                ClassBox.ConnectRight = new Point (PosX + ClassBox.Width , PosY + ClassBox.Height / 2);
                RaisePropertyChanged();
                return ClassBox.ConnectRight;
            }
        }
        public Point ConnectBottom {
            get {
                ClassBox.ConnectBottom = new Point (PosX + ClassBox.Width / 2 , PosY + ClassBox.Height);
                RaisePropertyChanged();
                return ClassBox.ConnectBottom;
            }
        }

        public void raiseLinePropertyChanged() {
            foreach (LineViewModel line in ClassBox.LineList) {
                line.raisePropertyChanged();
            }
        }

        public bool equals(ClassBoxViewModel fromBox) {
            return (this.PosX == fromBox.PosX) && (this.PosY == fromBox.PosY);
        }

        public Brush SelectedColor => IsSelected ? Brushes.Orchid : Brushes.White;

        public bool IsSelected { get; set; }

        public ArrayList LineList { get { return ClassBox.LineList; } }

    }
}
