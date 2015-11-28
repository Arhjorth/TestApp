using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using TestApp.Model;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using TestApp.UndoRedo;
using TestApp.View;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace TestApp.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {   
        public static ObservableCollection<ClassBoxViewModel> ClassBoxes { get; set; }
        public static ObservableCollection<LineViewModel> Lines { get; set; }

        public ICommand CommandAddClassBox { get; }
        public ICommand CommandAddLine { get; }
        public ICommand CommandMouseUpClassBox { get; }
        public ICommand CommandMouseDownClassBox { get; } 
        public ICommand CommandMouseMoveClassBox { get; }
        public ICommand MouseMoveShapeCommand { get; }
        public ICommand CommandUndo { get; }
        public ICommand CommandRedo { get; }
        public ICommand CommandLoad { get; }
        public ICommand CommandSave { get; }
        public ICommand CommandNew { get; }
        public ICommand CommandAddMethod { get; }

        public static UndoRedoController undoRedoController = UndoRedoController.Instance;

        // Saves the initial point that the mouse has during a move operation.
        private Point initialMousePosition;
        // Saves the initial point that the shape has during a move operation.
        private Point initialClassBoxPosition;

        private bool isAddingLine;

        private ClassBoxViewModel addingLineFrom;

        public DialogViews dialogs { get; set; }

        public MainViewModel()
        {
            ClassBoxes = new ObservableCollection<ClassBoxViewModel>();
            Lines = new ObservableCollection<LineViewModel>();

            CommandAddClassBox = new RelayCommand(AddClassBox);
            CommandAddLine = new RelayCommand(AddLine);
            CommandMouseDownClassBox = new RelayCommand<MouseButtonEventArgs>(MouseDownClassBox);
            CommandMouseMoveClassBox = new RelayCommand<MouseEventArgs>(MouseMoveClassBox);
            CommandMouseUpClassBox = new RelayCommand<MouseButtonEventArgs>(MouseUpClassBox);
            CommandSave = new RelayCommand(SaveDiagram);
            CommandLoad = new RelayCommand(LoadDiagram);
            CommandNew = new RelayCommand(NewDiagram);

            CommandUndo = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            CommandRedo = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

          //  CommandMouseMoveClassbox = new RelayCommand<MouseButtonEventArgs>(MousemMoveClassBox);

        }

        private void AddClassBox() {
            undoRedoController.AddAndExecute(new CommandAddClassBox(new ClassBoxViewModel() , ClassBoxes ));
        }

        private void AddLine() {
            isAddingLine = true;

        }
        
        // private class MousemMoveClassBox(MouseButtonEventArgs e)

            
        private void MouseDownClassBox(MouseButtonEventArgs e) {
            if (!isAddingLine) { 
            ClassBoxViewModel selectedBox = (ClassBoxViewModel)((FrameworkElement)e.MouseDevice.Target).DataContext;

            var mousePosition = RelativeMousePosition(e);

            initialMousePosition = mousePosition;
            initialClassBoxPosition = new Point(selectedBox.PosX, selectedBox.PosY);

            // The mouse is captured, so the current shape will always be the target of the mouse events, 
            //  even if the mouse is outside the application window.
            e.MouseDevice.Target.CaptureMouse();
            }
        }

        private void MouseMoveClassBox(MouseEventArgs e) {
            if (Mouse.Captured != null) {
                var target = ((FrameworkElement)e.MouseDevice.Target).DataContext;

                if (target is ClassBoxViewModel) {
                    ClassBoxViewModel selectedBox = (ClassBoxViewModel)target;

                    var mousePosition = RelativeMousePosition(e);

                    selectedBox.PosX = initialClassBoxPosition.X + (mousePosition.X - initialMousePosition.X);
                    selectedBox.PosY = initialClassBoxPosition.Y + (mousePosition.Y - initialMousePosition.Y);
                }
            }
        }

        private void MouseUpClassBox(MouseButtonEventArgs e) {

            // Used for adding a Line.
            if (isAddingLine) {
                ClassBoxViewModel selectedBox = (ClassBoxViewModel)((FrameworkElement)e.MouseDevice.Target).DataContext;

                if (addingLineFrom == null) {
                    addingLineFrom = selectedBox;
                    addingLineFrom.IsSelected = true;
                } else if (addingLineFrom != selectedBox) {

                    int[] connectionPoints = calculateConnectionPoints(addingLineFrom, selectedBox);
                    LineViewModel newLine = new LineViewModel() { fromBox = addingLineFrom, cF = connectionPoints[0], toBox = selectedBox, cT = connectionPoints[1] };
                    undoRedoController.AddAndExecute(new CommandAddLine(newLine, Lines));

                    //Adding line to classbox arraylist of lines
                    addingLineFrom.LineList.Add(newLine);
                    selectedBox.LineList.Add(newLine);

                    addingLineFrom.IsSelected = false;

                    isAddingLine = false;
                    addingLineFrom = null;


                }
            } else {
                var target = ((FrameworkElement)e.MouseDevice.Target).DataContext;

                if (target is ClassBoxViewModel) {
                    ClassBoxViewModel selectedBox = (ClassBoxViewModel) target;
                    var mousePosition = RelativeMousePosition(e);
                    undoRedoController.Add(new CommandMoveClassBox(selectedBox, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));
                    e.MouseDevice.Target.ReleaseMouseCapture();

                    foreach (LineViewModel line in selectedBox.LineList) {
                        int[] connectionsPoints = calculateConnectionPoints(line.fromBox, line.toBox);
                        line.cF = connectionsPoints[0];
                        line.cT = connectionsPoints[1];
                        line.raisePropertyChanged();
                    }
               }
            }
        }

        private int[] calculateConnectionPoints(ClassBoxViewModel addingLineFrom, ClassBoxViewModel selectedBox) {
            Point[] potList = new Point[] { addingLineFrom.ConnectTop, addingLineFrom.ConnectRight, addingLineFrom.ConnectBottom, addingLineFrom.ConnectLeft };
            Point[] potList0 = new Point[] { selectedBox.ConnectTop, selectedBox.ConnectRight, selectedBox.ConnectBottom, selectedBox.ConnectLeft };

            double dis = double.MaxValue;
            int[] points = new int[2];
            int list1 = 0;
            int list2 = 0;

            foreach (Point pot in potList) {
                list2 = 0;
                foreach (Point pot0 in potList0) {

                    if (distance(pot.X, pot.Y, pot0.X, pot0.Y) < dis) {
                        points[0] = list1;
                        points[1] = list2;
                        dis = distance(pot.X, pot.Y, pot0.X, pot0.Y);
                    }
                    list2++;
                }
                list1++;
            }
            return points;
        }

        private double distance(double x1, double y1, double x2, double y2) {
            return Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
        }

        // Gets the mouse position relative to the canvas.
        private Point RelativeMousePosition(MouseEventArgs e) {
            // Here the visual element that the mouse is captured by is retrieved.
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // The canvas holding the shapes visual element, is found by searching up the tree of visual elements.
            var canvas = FindParentOfType<Canvas>(shapeVisualElement);
            // The mouse position relative to the canvas is gotten here.
            return Mouse.GetPosition(canvas);
        }

        private static T FindParentOfType<T>(DependencyObject o) {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }

       SaveFileDialog saveDialog = 
            new SaveFileDialog() { Title = "Save Diagram", Filter = "XML Document (.xml)|*.xml", DefaultExt = "xml", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };
       OpenFileDialog openDialog = 
            new OpenFileDialog() { Title = "Open Diagram", Filter = "XML Document (.xml)|*.xml", DefaultExt = "xml", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), CheckFileExists = true };

        private void NewDiagram() {
            ClassBoxes.Clear();
            Lines.Clear();
        }

        private void SaveDiagram() {
            string path = null;
            if (saveDialog.ShowDialog() == true) {
                path = saveDialog.FileName;
            }

            if (path != null) {
                Diagram diagram = new Diagram() { ClassBoxes = ClassBoxes.Select(x => x.ClassBox).ToList(), Lines = Lines.Select(x => x.Line).ToList() };
                ControllerLoadSave.Instance.Save(diagram, path);
            }
        }

        private async void LoadDiagram() {
            string path = null;

            if (openDialog.ShowDialog() == true) {
                path = openDialog.FileName;
            }

            if (path != null) {
                Diagram diagram = await ControllerLoadSave.Instance.load(path);

                ClassBoxes.Clear();
                Lines.Clear();
                
                foreach(ClassBox box in diagram.ClassBoxes) {
                    ClassBoxes.Add( new ClassBoxViewModel(box));
                }
                foreach(Line line in diagram.Lines) {
                    Lines.Add(new LineViewModel(line));
                } 

                foreach (LineViewModel line in Lines) {
                    foreach(ClassBoxViewModel box in ClassBoxes) { // Setting the actual ClassBox objects for each line. Fixing problem of only reading coordinates of ealier ClassBoxes. 
                        if (box.equals(line.fromBox)) {
                            line.fromBox = box;
                        }
                        else if(box.equals(line.toBox)) {
                            line.toBox = box;
                        }
                    }


                    line.fromBox.LineList.Add(line);
                    line.toBox.LineList.Add(line);
                }
            }
        }

    }
}