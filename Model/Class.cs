using System;
using System.Windows;

namespace TestApp.Model {
    public class Class {
        public Class() {
        }
        public double Height { get; } = 200;
        public double Width { get; } = 100;
        // otifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterY); NotifyPropertyChanged(() => CenterY); } }

        public double PosX { get; } = 0;
        public double PosY { get; } = 0;

        public double CanvasCenterX { get { return PosX + Height / 2; } }
        public double CanvasCenterY { get { return PosY + Width / 2; } }

        public Point ConnectTop { get { return new Point(PosX + Width / 2, PosY); } }
        public Point ConnectLeft { get { return new Point(PosX, PosY + Height / 2); } }
        public Point ConnectRight { get { return new Point(PosX + Width, PosY + Height / 2); } }
        public Point ConnectBottom { get { return new Point(PosX + Width / 2, PosY + Height); } }
    }
}
