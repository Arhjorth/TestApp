using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections;
using TestApp.UndoRedo;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TestApp.Model;
using System.Collections.ObjectModel;

namespace TestApp.ViewModel {
    public class ClassBoxViewModel : ViewModelBase {

        public ICommand RemoveCommand { get; }

        public ObservableCollection<ClassBoxViewModel> ClassBoxes { get; set; }
        public ObservableCollection<LineViewModel> Lines { get; set; }

        public UndoRedoController undoRedoController = MainViewModel.undoRedoController;

        private void Remove() {
            undoRedoController.AddAndExecute(new CommandDeleteClassBox(this,ClassBoxes ,Lines));
        }

        public ClassBox ClassBox { get; set; }

        public ClassBoxViewModel() {
            ClassBox = new ClassBox();
            RemoveCommand = new RelayCommand(Remove);
            ClassBoxes = MainViewModel.ClassBoxes;
            Lines = MainViewModel.Lines;

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
