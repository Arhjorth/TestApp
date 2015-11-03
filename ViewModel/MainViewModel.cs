using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using TestApp.Model;
using System.Windows.Media;
using System.Windows.Controls;
using System;

namespace TestApp.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {   
        public ObservableCollection<ClassBox> ClassBoxes { get; set; }
        public ICommand CommandAddClassBox { get; }
        public ICommand CommandMouseUpClassBox { get; }
        public ICommand CommandMouseDownClassBox { get; }
        public ICommand CommandMouseMoveClassBox { get; }

        // Saves the initial point that the mouse has during a move operation.
        private Point initialMousePosition;
        // Saves the initial point that the shape has during a move operation.
        private Point initialClassBoxPosition;

        public MainViewModel()
        {
            ClassBoxes = new ObservableCollection<ClassBox>();

            CommandAddClassBox = new RelayCommand(AddClassBox);
            CommandMouseDownClassBox = new RelayCommand<MouseButtonEventArgs>(MouseDownClassBox);
            CommandMouseMoveClassBox = new RelayCommand<MouseEventArgs>(MouseMoveClassBox);
            CommandMouseUpClassBox = new RelayCommand<MouseButtonEventArgs>(MouseUpClassBox);

        }

        private void AddClassBox() {
            ClassBoxes.Add(new ClassBox());
        }

        private void MouseDownClassBox(MouseButtonEventArgs e) {

            ClassBox selectedBox = (ClassBox)((FrameworkElement)e.MouseDevice.Target).DataContext;

            var mousePosition = RelativeMousePosition(e);

            initialMousePosition = mousePosition;
            initialClassBoxPosition = new Point(selectedBox.PosX, selectedBox.PosY);

            // The mouse is captured, so the current shape will always be the target of the mouse events, 
            //  even if the mouse is outside the application window.
            e.MouseDevice.Target.CaptureMouse();
        }

        private void MouseMoveClassBox(MouseEventArgs e) {
            if (Mouse.Captured != null) {

                ClassBox selectedBox = (ClassBox)((FrameworkElement)e.MouseDevice.Target).DataContext;


                var mousePosition = RelativeMousePosition(e);

                selectedBox.PosX = initialClassBoxPosition.X + (mousePosition.X - initialMousePosition.X);
                selectedBox.PosY = initialClassBoxPosition.Y + (mousePosition.Y - initialMousePosition.Y);
            }
        }

        private void MouseUpClassBox(MouseButtonEventArgs e) {

            ClassBox selectedBox = (ClassBox)((FrameworkElement)e.MouseDevice.Target).DataContext;

            var mousePosition = RelativeMousePosition(e);

            selectedBox.PosX = initialClassBoxPosition.X;
            selectedBox.PosY = initialClassBoxPosition.Y;

            e.MouseDevice.Target.ReleaseMouseCapture();
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