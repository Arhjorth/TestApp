using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections;
using TestApp.UndoRedo;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TestApp.Model;
using System.Collections.ObjectModel;
using System;

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
        public ClassBoxViewModel(ClassBox _box) : base() {
            ClassBox = _box;
        }

        public double PosX {
            get { return ClassBox.PosX; }
            set {
                ClassBox.PosX = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanvasCenterX));
                ConnectTop = ConnectTop;
                ConnectBottom = ConnectBottom;
                ConnectLeft = ConnectLeft;
                ConnectRight = ConnectRight;
                raiseLinePropertyChanged();
            }
        }

        public double PosY {
            get { return ClassBox.PosY; }
            set {
                ClassBox.PosY = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanvasCenterY));
                ConnectTop = ConnectTop;
                ConnectBottom = ConnectBottom;
                ConnectLeft = ConnectLeft;
                ConnectRight = ConnectRight;
                raiseLinePropertyChanged();
            }
        }

        public double Height { get { return ClassBox.Height; } }
        public double Width { get { return ClassBox.Width; } }

        public double CanvasCenterX {
            get { return PosX + ClassBox.Height / 2; }
        }

        public double CanvasCenterY {
            get { return PosY + ClassBox.Width / 2; }
        }

        public Point ConnectTop {
            get {
                ClassBox.ConnectTop = new Point(PosX + Width / 2, PosY);
                return ClassBox.ConnectTop;
            }
            set { ClassBox.ConnectTop = value; }
        }

        public Point ConnectLeft {
            get {
                ClassBox.ConnectLeft = new Point(PosX, PosY + Height / 2);
                return ClassBox.ConnectLeft;
            }
            set { ClassBox.ConnectLeft = value; }
        }

        public Point ConnectRight {
            get {
                ClassBox.ConnectRight = new Point(PosX + Width, PosY + Height / 2);
                return ClassBox.ConnectRight;
            }
            set { ClassBox.ConnectRight = value; }
        }
        public Point ConnectBottom {
            get {
                ClassBox.ConnectBottom = new Point(PosX + Width / 2, PosY + Height);
                return ClassBox.ConnectBottom;
            }
            set { ClassBox.ConnectBottom = value; }
        }

        public String Name {
            get { return ClassBox.Name; }
            set { ClassBox.Name = value; }
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

        public ObservableCollection<String> Methods { get; set; } = new ObservableCollection<String>();
        public void AddMethod(String str) {
            ClassBox.Methods.Add(str);
            Methods.Add(str);
            RaisePropertyChanged(nameof(Methods));
        }

        public void RaisePropertyMethods() {
            RaisePropertyChanged(nameof(Methods));
        }

    }
}
