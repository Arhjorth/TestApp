using System;
using System.Windows;
using System.Windows.Media;

namespace TestApp.Model {
    public class ClassBox {
        public ClassBox() {
        }
        public double Height { get; } = 100;
        public double Width { get; } = 100;
        // otifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterY); NotifyPropertyChanged(() => CenterY); } }

        public double PosX { get; set; } = 100;
        public double PosY { get; set; } = 100;

        public double CanvasCenterX { get { return PosX + Height / 2; } }
        public double CanvasCenterY { get { return PosY + Width / 2; } }

        public Point ConnectTop { get { return new Point(PosX + Width / 2, PosY); } }
        public Point ConnectLeft { get { return new Point(PosX, PosY + Height / 2); } }
        public Point ConnectRight { get { return new Point(PosX + Width, PosY + Height / 2); } }
        public Point ConnectBottom { get { return new Point(PosX + Width / 2, PosY + Height); } }

        public Brush SelectedColor => IsSelected ? Brushes.Orchid : Brushes.OrangeRed;

        public bool IsSelected { get; set; }
    }
}
