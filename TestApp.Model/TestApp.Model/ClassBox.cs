using System;
using System.Windows;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace TestApp.Model {
    public class ClassBox {

        public List<String> Methods = new List<String>();
        public String Name { get; set; } = "class";
        public double Height { get; set; } = 60;
        public double Width { get; set; } = 100;
        public double PosX { get; set; } = 100;
        public double PosY { get; set; } = 100;
        public Point ConnectTop { get; set; } = new Point();
        public Point ConnectLeft { get; set; } = new Point();
        public Point ConnectRight { get; set; } = new Point();
        public Point ConnectBottom { get; set; } = new Point();
        [XmlIgnore]
        public ArrayList LineList = new ArrayList();

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
    }
}