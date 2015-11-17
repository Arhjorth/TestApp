using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestApp.Model {
    public class Line : ViewModelBase {

        private Point fromBox;
        private Point toBox;
        
        public Line(Point fromBox, Point toBox) {
            FromBox = fromBox;
            ToBox = toBox;
        }
        
        
        public Point FromBox { get { return fromBox; } set { fromBox = value; RaisePropertyChanged(); } }

        public Point ToBox { get { return toBox; } set { toBox = value; RaisePropertyChanged(); } }
        
    }
}
