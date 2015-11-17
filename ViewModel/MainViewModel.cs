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

                    undoRedoController.AddAndExecute(new CommandAddLine(new Line(addingLineFrom, selectedBox), Lines));

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