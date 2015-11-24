using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Collections;
using System.Xml.Serialization;

namespace TestApp.Model {
    public class ClassBox : ViewModelBase {
        public ClassBox() {
        }



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
                raiseLinePropertyChanged();
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
                raiseLinePropertyChanged();
            }
        }
        
        [XmlIgnore]
        private ArrayList lineList = new ArrayList();
        [XmlIgnore]
        public ArrayList LineList { get { return lineList; } }

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

        public Point ConnectTop {
            get {
                connectTop.X = PosX + Width / 2;
                connectTop.Y = PosY;
                RaisePropertyChanged();
                return connectTop;
            }
        }

        public Point ConnectLeft {
            get {
                connectLeft.X = PosX;
                connectLeft.Y = PosY + Height / 2;
                RaisePropertyChanged();
                return connectLeft;
            }
        }

        public Point ConnectRight {
            get {
                connectRight.X = PosX + Width;
                connectRight.Y = PosY + Height / 2;
                RaisePropertyChanged();
                return connectRight;
            }
        }
        public Point ConnectBottom {
            get {
                connectBottom.X = PosX + Width / 2;
                connectBottom.Y = PosY + Height;
                RaisePropertyChanged();
                return connectBottom;
            }
        }

        public Point getPoint(int v1) {

            switch (v1) {
                case 0:
                    return ConnectTop;
                case 1:
                    return ConnectRight;
                case 2:
                    return ConnectBottom;
                case 3:
                    return ConnectLeft;
                default:
                    return new Point();   
            }
        }

        public Brush SelectedColor => IsSelected ? Brushes.Orchid : Brushes.White;


        public bool IsSelected { get; set; }

        public void raiseLinePropertyChanged() {
            foreach (Line line in LineList) {
                line.raisePropertyChanged();
            }
        }

        public bool equals(ClassBox fromBox) {
            return (this.PosX == fromBox.PosX) && (this.PosY == fromBox.PosY);
        }
    }

}