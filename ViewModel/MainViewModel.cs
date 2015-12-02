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
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using System.Net.Mime;
using System.Text;

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
        public ICommand CommandMouseDownCanvas { get; }
        public ICommand CommandMouseMoveCanvas { get; }
        public ICommand CommandMouseUpCanvas { get; }
        public ICommand CommandMouseMoveShapeCommand { get; }
        public ICommand CommandUndo { get; }
        public ICommand CommandRedo { get; }
        public ICommand CommandLoad { get; }
        public ICommand CommandSave { get; }
        public ICommand CommandNew { get; }
        public ICommand CommandAddMethod { get; }
        public ICommand CommandSendEmail { get; }
        public ICommand CommandOnSendEmail { get; }
        public ICommand CommandCut { get; }
        public ICommand CommandCopy { get; }
        public ICommand CommandPaste { get; }

        public static UndoRedoController undoRedoController = UndoRedoController.Instance;

        // Saves the initial point that the mouse has during a move operation.
        private Point initialMousePosition;
        // Saves the initial point that the shape has during a move operation.
        private Dictionary<ClassBoxViewModel,Point> initialClassBoxPositions = new Dictionary<ClassBoxViewModel,Point>();

        private Point SelectionBoxStart;
        public double SelectionBoxX { get; set; }
        public double SelectionBoxY { get; set; }
        public double SelectionBoxWidth { get; set; }
        public double SelectionBoxHeight { get; set; }

        // ####### EMAIL ########
        public Visibility OnEmailSend { get; set; } = Visibility.Hidden;

        public string UserEmail { get; set; } = "Please enter your email";


        // ##############
        private bool isAddingLine;
        private bool canPaste = false;

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
            CommandMouseDownCanvas = new RelayCommand<MouseButtonEventArgs>(MouseDownCanvas);
            CommandMouseMoveCanvas = new RelayCommand<MouseEventArgs>(MouseMoveCanvas);
            CommandMouseUpCanvas = new RelayCommand<MouseButtonEventArgs>(MouseUpCanvas);
            CommandSave = new RelayCommand(SaveDiagram);
            CommandLoad = new RelayCommand(LoadDiagram);
            CommandNew = new RelayCommand(NewDiagram);
            CommandSendEmail = new RelayCommand(SendXmlAsEmail);
            CommandOnSendEmail = new RelayCommand(OnSendEmail);
            CommandCut = new RelayCommand(Cut);
            CommandCopy = new RelayCommand(Copy);
            CommandPaste = new RelayCommand(Paste,CanPaste);
            CommandUndo = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            CommandRedo = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

          //  CommandMouseMoveClassbox = new RelayCommand<MouseButtonEventArgs>(MousemMoveClassBox);

        }


        //########## EMAIL #######
        private void OnSendEmail() {
            if(OnEmailSend == Visibility.Hidden || OnEmailSend == Visibility.Collapsed) {
                OnEmailSend = Visibility.Visible;
                RaisePropertyChanged(nameof(OnEmailSend));
            }
            else {
                OnEmailSend = Visibility.Hidden;
                RaisePropertyChanged(nameof(OnEmailSend));
            }
        }
        
        private void SendXmlAsEmail() {
            OnSendEmail();
            Task.Run(() => SendXml());
        }

        private void SendXml() {
            try {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress("arhjorth@hotmail.com");
                message.To.Add(new MailAddress(UserEmail));
                message.Subject = "Test";
                message.Body = "Content";

                Diagram diagram = new Diagram() { ClassBoxes = ClassBoxes.Select(x => x.ClassBox).ToList(), Lines = Lines.Select(x => x.Line).ToList() };

                XmlSerializer xml = new XmlSerializer(diagram.GetType());


                StringBuilder sb = new StringBuilder("");
                StringWriter sw = new StringWriter(sb);

                xml.Serialize(sw, diagram);
                string old = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";
                string neew = "<?xml version=\"1.0\"?>";

                Console.WriteLine(old);
                Console.WriteLine(neew);

                sb.Replace(old, neew);
                Console.WriteLine(sb.ToString());
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));

                ContentType ct = new ContentType();
                ct.Name = "My Diagram" + ".xml";

                Attachment at = new Attachment(memoryStream, ct);

                message.Attachments.Add(at);

                smtp.Port = 587;
                smtp.Host = "smtp.live.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("arhjorth@hotmail.com", "3,141592653");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex) {
                MessageBox.Show("err: " + ex.Message);
            }
        }
        // #############


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

                if (selectedBox.IsMoveSelected) {
                    var selectedBoxes = ClassBoxes.Where(x => x.IsMoveSelected);
                    if (!selectedBoxes.Any()) selectedBoxes = new List<ClassBoxViewModel>() { selectedBox };

                    foreach (var box in selectedBoxes) {
                        initialClassBoxPositions.Add(box, new Point(box.PosX, box.PosY));
                    }
                } else {
                    initialClassBoxPositions.Add(selectedBox, new Point(selectedBox.PosX, selectedBox.PosY));
                }
                e.MouseDevice.Target.CaptureMouse();
            }
        }

        private void MouseDownCanvas(MouseButtonEventArgs e) {
            var target = ((FrameworkElement)e.MouseDevice.Target).DataContext;
            if (!isAddingLine && !(target is ClassBoxViewModel) ) {
                SelectionBoxStart = Mouse.GetPosition(e.MouseDevice.Target);
                e.MouseDevice.Target.CaptureMouse();
            }
        }

        private void MouseMoveClassBox(MouseEventArgs e) {
            if (Mouse.Captured != null) {
                var target = ((FrameworkElement)e.MouseDevice.Target).DataContext;

                if (target is ClassBoxViewModel) {
                    ClassBoxViewModel selectedBox = (ClassBoxViewModel)target;
                    var selectedBoxes = ClassBoxes.Where(x => x.IsMoveSelected);
                    if (!selectedBoxes.Any()) selectedBoxes = new List<ClassBoxViewModel>() { selectedBox };

                    var mousePosition = RelativeMousePosition(e);

                    if (selectedBox.IsMoveSelected) {
                        foreach (var box in selectedBoxes) {
                            var originalPosition = initialClassBoxPositions[box];
                            box.PosX = originalPosition.X + (mousePosition.X - initialMousePosition.X);
                            box.PosY = originalPosition.Y + (mousePosition.Y - initialMousePosition.Y);
                        }
                    } else {
                        var originalPosition = initialClassBoxPositions[selectedBox];
                        selectedBox.PosX = originalPosition.X + (mousePosition.X - initialMousePosition.X);
                        selectedBox.PosY = originalPosition.Y + (mousePosition.Y - initialMousePosition.Y);
                    }
                }
            }
        }

        private void MouseMoveCanvas(MouseEventArgs e) {
            var target = ((FrameworkElement)e.MouseDevice.Target).DataContext;
            if (Mouse.Captured != null && !isAddingLine && !(target is ClassBoxViewModel)) {
                var SelectionBoxNow = Mouse.GetPosition(e.MouseDevice.Target);
                SelectionBoxX = Math.Min(SelectionBoxStart.X, SelectionBoxNow.X);
                SelectionBoxY = Math.Min(SelectionBoxStart.Y, SelectionBoxNow.Y);
                SelectionBoxWidth = Math.Abs(SelectionBoxNow.X - SelectionBoxStart.X);
                SelectionBoxHeight = Math.Abs(SelectionBoxNow.Y - SelectionBoxStart.Y);
                RaisePropertyChanged(() => SelectionBoxX);
                RaisePropertyChanged(() => SelectionBoxY);
                RaisePropertyChanged(() => SelectionBoxWidth);
                RaisePropertyChanged(() => SelectionBoxHeight);
            }
        }

        private void MouseUpClassBox(MouseButtonEventArgs e) {

            var target = ((FrameworkElement)e.MouseDevice.Target).DataContext;
            // Used for adding a Line.
            if (isAddingLine && (target is ClassBoxViewModel)) {
                ClassBoxViewModel selectedBox = (ClassBoxViewModel)target;

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
                if (target is ClassBoxViewModel) {
                    ClassBoxViewModel selectedBox = (ClassBoxViewModel) target;
                    var mousePosition = RelativeMousePosition(e);
                    var selectedBoxes = ClassBoxes.Where(x => x.IsMoveSelected).ToList();
                    if (!selectedBoxes.Any()) selectedBoxes = new List<ClassBoxViewModel>() { selectedBox };

                    undoRedoController.Add(new CommandMoveClassBoxes( selectedBoxes, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));
                    e.MouseDevice.Target.ReleaseMouseCapture();
                    foreach (var box in selectedBoxes) {
                        foreach (LineViewModel line in box.LineList) {
                            int[] connectionsPoints = calculateConnectionPoints(line.fromBox, line.toBox);
                            line.cF = connectionsPoints[0];
                            line.cT = connectionsPoints[1];
                            line.raisePropertyChanged();
                        }
                    }
                    initialClassBoxPositions.Clear();
                    e.MouseDevice.Target.ReleaseMouseCapture();
                }
            }
        }

        private void MouseUpCanvas(MouseButtonEventArgs e) {
            var target = ((FrameworkElement)e.MouseDevice.Target).DataContext;
            if (!isAddingLine && !(target is ClassBoxViewModel)) {
                Point SelectionBoxEnd = Mouse.GetPosition(e.MouseDevice.Target);
                var smallX = Math.Min(SelectionBoxStart.X, SelectionBoxEnd.X);
                var smallY = Math.Min(SelectionBoxStart.Y, SelectionBoxEnd.Y);
                var largeX = Math.Max(SelectionBoxStart.X, SelectionBoxEnd.X);
                var largeY = Math.Max(SelectionBoxStart.Y, SelectionBoxEnd.Y);

                foreach (var box in ClassBoxes) {
                    box.IsMoveSelected = box.CanvasCenterX > smallX && box.CanvasCenterX < largeX && box.CanvasCenterY > smallY && box.CanvasCenterY < largeY;
                }

                SelectionBoxX = SelectionBoxY = SelectionBoxWidth = SelectionBoxHeight = 0;
                RaisePropertyChanged(() => SelectionBoxX);
                RaisePropertyChanged(() => SelectionBoxY);
                RaisePropertyChanged(() => SelectionBoxWidth);
                RaisePropertyChanged(() => SelectionBoxHeight);
                e.MouseDevice.Target.ReleaseMouseCapture();
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

        public bool CanPaste() { return canPaste; }

        private async void Cut() {
            var selectedBoxes = ClassBoxes.Where(x => x.IsMoveSelected).ToList();
            var selectedLines = new List<LineViewModel>();
            foreach (LineViewModel line in Lines) {
                foreach (ClassBoxViewModel box in selectedBoxes) {
                    if (line.fromBox.equals(box) || line.toBox.equals(box)) {
                        selectedLines.Add(line);
                    }
                }
            }

            foreach (ClassBoxViewModel box in selectedBoxes) {
                undoRedoController.AddAndExecute(new CommandDeleteClassBox(box, ClassBoxes, Lines));
            }

            Diagram diagram = new Diagram() { ClassBoxes = selectedBoxes.Select(x => x.ClassBox).ToList(), Lines = selectedLines.Select(x => x.Line).ToList() };

            var xml = await ControllerLoadSave.Instance.ToString(diagram);

            Clipboard.SetText(xml);
            canPaste = true;
        }

        private async void Copy() {
            var selectedBoxes = ClassBoxes.Where(x => x.IsMoveSelected).ToList();
            var selectedLines = Lines.Where(x => selectedBoxes.Contains(x.fromBox) || selectedBoxes.Contains(x.toBox));

            Diagram diagram = new Diagram() { ClassBoxes = selectedBoxes.Select(x => x.ClassBox).ToList(), Lines = selectedLines.Select(x => x.Line).ToList() };

            var xml = await ControllerLoadSave.Instance.ToString(diagram);

            Clipboard.SetText(xml);
            canPaste = true;
        }

        private async void Paste() {

            var xml = Clipboard.GetText();
            var diagram = await ControllerLoadSave.Instance.FromString(xml);
            var boxes = diagram.ClassBoxes;
            var lines = diagram.Lines;

            foreach (ClassBox box in boxes) {
                ClassBoxes.Add(new ClassBoxViewModel(box));
            }
            foreach (Line line in lines) {
                Lines.Add(new LineViewModel(line));
            }

            foreach (Line line in lines) {
                foreach (var box in boxes) {
                    box.PosX += 50;
                    box.PosY += 50;
                    if ((box.PosX == line.fromBox.PosX) && (box.PosY == line.fromBox.PosY)) {
                        line.fromBox = box;
                    } else if ((box.PosX == line.toBox.PosX) && (box.PosY == line.toBox.PosY)) {
                        line.toBox = box;
                    }
                }
                line.fromBox.LineList.Add(line);
                line.toBox.LineList.Add(line);
            }
        }
    }
}