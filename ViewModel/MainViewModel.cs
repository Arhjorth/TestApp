using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using TestApp.Model;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using TestApp.UndoRedo;

namespace TestApp.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {   
        public ObservableCollection<ClassBox> ClassBoxes { get; set; }
        public ObservableCollection<Line> Lines { get; set; }

        public ICommand CommandAddClassBox { get; }
        public ICommand CommandAddLine { get; }
        public ICommand CommandMouseUpClassBox { get; }
        public ICommand CommandMouseDownClassBox { get; }
        public ICommand CommandMouseMoveClassBox { get; }
        public ICommand MouseMoveShapeCommand { get; }
        public ICommand CommandUndo { get; }
        public ICommand CommandRedo { get; }

        public UndoRedoController undoRedoController = UndoRedoController.Instance;

        // Saves the initial point that the mouse has during a move operation.
        private Point initialMousePosition;
        // Saves the initial point that the shape has during a move operation.
        private Point initialClassBoxPosition;

        private bool isAddingLine;

        private ClassBox addingLineFrom;

        public MainViewModel()
        {
            ClassBoxes = new ObservableCollection<ClassBox>();
            Lines = new ObservableCollection<Line>();

            CommandAddClassBox = new RelayCommand(AddClassBox);
            CommandAddLine = new RelayCommand(AddLine);
            CommandMouseDownClassBox = new RelayCommand<MouseButtonEventArgs>(MouseDownClassBox);
            CommandMouseMoveClassBox = new RelayCommand<MouseEventArgs>(MouseMoveClassBox);
            CommandMouseUpClassBox = new RelayCommand<MouseButtonEventArgs>(MouseUpClassBox);

            CommandUndo = new RelayCommand(undoRedoController.Undo/*, undoRedoController.CanUndo*/); //TODO: Fix dis!
            CommandRedo = new RelayCommand(undoRedoController.Redo/*, undoRedoController.CanRedo*/);

          //  CommandMouseMoveClassbox = new RelayCommand<MouseButtonEventArgs>(MousemMoveClassBox);

        }

        private void AddClassBox() {
            undoRedoController.AddAndExecute(new CommandAddClassBox(new ClassBox() , ClassBoxes ));
            Console.WriteLine(undoRedoController.CanUndo());
        }

        private void AddLine() {
            isAddingLine = true;

        }
        
        // private class MousemMoveClassBox(MouseButtonEventArgs e)

            
        private void MouseDownClassBox(MouseButtonEventArgs e) {
            if (!isAddingLine) { 
            Console.WriteLine("this");
            ClassBox selectedBox = (ClassBox)((FrameworkElement)e.MouseDevice.Target).DataContext;

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
                Console.WriteLine("is");
                ClassBox selectedBox = (ClassBox)((FrameworkElement)e.MouseDevice.Target).DataContext;


                var mousePosition = RelativeMousePosition(e);

                selectedBox.PosX = initialClassBoxPosition.X + (mousePosition.X - initialMousePosition.X);
                selectedBox.PosY = initialClassBoxPosition.Y + (mousePosition.Y - initialMousePosition.Y);
                
                Console.WriteLine(selectedBox.PosX+" "+selectedBox.PosY);
            }
        }

        private void MouseUpClassBox(MouseButtonEventArgs e) {

            // Used for adding a Line.
            if (isAddingLine) {
                ClassBox selectedBox = (ClassBox)((FrameworkElement)e.MouseDevice.Target).DataContext;

                if (addingLineFrom == null) {
                    addingLineFrom = selectedBox;
                    addingLineFrom.IsSelected = true;
                }
                else if (addingLineFrom != selectedBox) {

                    Point[] connectionPoints =  calculateConnectionPoints(addingLineFrom, selectedBox);
                    undoRedoController.AddAndExecute(new CommandAddLine(new Line(connectionPoints[0], connectionPoints[1]), Lines));
                    Console.WriteLine(" LINES : " + Lines.Count);
                    addingLineFrom.IsSelected = false;

                    isAddingLine = false;
                    addingLineFrom = null;
                    
                    
                }
            } else{

            ClassBox selectedBox = (ClassBox)((FrameworkElement)e.MouseDevice.Target).DataContext;
            var mousePosition = RelativeMousePosition(e);
            undoRedoController.Add(new CommandMoveClassBox(selectedBox, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));
            e.MouseDevice.Target.ReleaseMouseCapture();
         }
       }

        private Point[] calculateConnectionPoints(ClassBox addingLineFrom, ClassBox selectedBox) {
            Point[] potList = new Point[] { addingLineFrom.ConnectTop, addingLineFrom.ConnectRight, addingLineFrom.ConnectBottom, addingLineFrom.ConnectLeft };
            Point[] potList0 = new Point[] { selectedBox.ConnectTop, selectedBox.ConnectRight, selectedBox.ConnectBottom, selectedBox.ConnectLeft };

            Point pota = new Point();
            Point potb = new Point();
            
            double dis = 999999.0;
            
            foreach (Point pot in potList) {
                foreach (Point pot0 in potList0) {
                    Console.WriteLine("I MADE CALC.     #############3");
                    if (distance(pot.X, pot.Y, pot0.X, pot0.Y) < dis) {
                        pota.X = pot.X;
                        pota.Y = pot.Y;
                        potb.X = pot0.X;
                        potb.Y = pot0.Y;

                        dis = distance(pot.X, pot.Y, pot0.X, pot0.Y);
                        Console.WriteLine("DISTANCE :" + dis);

                    }
                }
            }
            Point[] points = new Point[] { pota, potb };
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



    }
}