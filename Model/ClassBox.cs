using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace TestApp.Model {
    public class ClassBox : ViewModelBase {
        public ClassBox() {
        }

        public int Number { get; }

        public double Height { get; } = 100;
        public double Width { get; } = 100;

        private double posX = 100;

        public double PosX {
            get { return posX; }
            set {
                posX = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanvasCenterX));
                RaisePropertyChanged(nameof(ConnectTop));
                RaisePropertyChanged(nameof(ConnectBottom));
                RaisePropertyChanged(nameof(ConnectLeft));
                RaisePropertyChanged(nameof(ConnectRight));
            }
        }
        private double posY = 100;
        public double PosY {
            get { return posY; }
            set {
                posY = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanvasCenterY));
                RaisePropertyChanged(nameof(ConnectTop));
                RaisePropertyChanged(nameof(ConnectBottom));
                RaisePropertyChanged(nameof(ConnectLeft));
                RaisePropertyChanged(nameof(ConnectRight));

            }
        }

        public double CanvasCenterX {
            get { return PosX + Height / 2; }
        }

        public double CanvasCenterY {
            get { return PosY + Width / 2; }
        }
        private Point connectTop;
        private Point connectLeft;
        private Point connectRight;
        private Point connectBottom;

        public Point ConnectTop { get {
                connectTop.X = PosX + Width / 2;
                connectTop.Y = PosY;
                return connectTop;
            } }

        public Point ConnectLeft { get {
                connectLeft.X = PosX;
                connectLeft.Y = PosY + Height / 2;
                return connectLeft;
            } }

        public Point ConnectRight { get {
                connectRight.X = PosX + Width;
                connectRight.Y =  PosY + Height / 2;
                return connectRight;
            } }
        public Point ConnectBottom { get {
                connectBottom.X = PosX + Width / 2;
                connectBottom.Y = PosY + Height;
                return connectBottom;
            } }

        public Brush SelectedColor => IsSelected ? Brushes.Orchid : Brushes.White;
        

        public bool IsSelected { get; set; }
    }
}
