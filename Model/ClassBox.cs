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
                }
        }
        private double posY = 100;
        public double PosY {
            get { return posY; }
            set {
                posY = value;
                RaisePropertyChanged();
            }
        }

        public double CanvasCenterX {
            get { return PosX + Height / 2; }
        }

        public double CanvasCenterY {
            get { return PosY + Width / 2; }
        }

        public Point ConnectTop { get { return new Point(PosX + Width / 2, PosY); } }
        public Point ConnectLeft { get { return new Point(PosX, PosY + Height / 2); } }
        public Point ConnectRight { get { return new Point(PosX + Width, PosY + Height / 2); } }
        public Point ConnectBottom { get { return new Point(PosX + Width / 2, PosY + Height); } }

        public Brush SelectedColor => IsSelected ? Brushes.Orchid : Brushes.White;

        public bool IsSelected { get; set; }
    }
}
