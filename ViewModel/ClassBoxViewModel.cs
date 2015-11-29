using GalaSoft.MvvmLight;
using TestApp.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections;
using TestApp.UndoRedo;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TestApp.ViewModel {
    public class ClassBoxViewModel : ViewModelBase {

        public ICommand RemoveCommand { get; }
        public ICommand CommandAddMethod { get; }
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
            CommandAddMethod = new RelayCommand(AddMethod);
            ClassBoxes = MainViewModel.ClassBoxes;
            Lines = MainViewModel.Lines;

        }
        public ClassBoxViewModel(ClassBox _box) {
            ClassBox = _box;
            RemoveCommand = new RelayCommand(Remove);
            CommandAddMethod = new RelayCommand(AddMethod);
            ClassBoxes = MainViewModel.ClassBoxes;
            Lines = MainViewModel.Lines;
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

        private bool isSelected;
        public bool IsSelected {
            get { return isSelected; }
            set { isSelected = value; RaisePropertyChanged(() => SelectedColor); } }

        private bool isMoveSelected;
        public bool IsMoveSelected {
            get { return isMoveSelected; }
            set { isMoveSelected = value; RaisePropertyChanged(() => SelectedColor); } }

        private bool isAbstract;
        public bool IsAbstract {
            get { return isAbstract; }
            set { isAbstract = value; RaisePropertyChanged(); RaisePropertyChanged(() => SelectedColor); } }
        public Brush SelectedColor => IsSelected ? Brushes.Orange : IsAbstract ? (IsMoveSelected ? Brushes.DodgerBlue : Brushes.MediumBlue) : (IsMoveSelected ? Brushes.SkyBlue : Brushes.Teal);

        public ArrayList LineList { get { return ClassBox.LineList; } }

        public ObservableCollection<String> Methods {
            get { return new ObservableCollection<string>( ClassBox.Methods ); }
            set { ClassBox.Methods = new List<String>(value); }
        }

        private void AddMethod() {
            ClassBox.Height += 20;
            ClassBox.Methods.Add("new method");
            RaisePropertyChanged(nameof(Methods));
            RaisePropertyChanged(nameof(Height));
        }

        public void RaisePropertyMethods() {
            RaisePropertyChanged(nameof(Methods));
        }
    }
}
