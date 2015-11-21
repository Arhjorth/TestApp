using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestApp.Model {
    public class Line : ViewModelBase {

        public ClassBox fromBox;
        public ClassBox toBox;
        Point connectFrom;
        Point connectTo;
        int cF;
        int cT;

        public Line(ClassBox fromBox, int v1, ClassBox toBox, int v2) {
            this.fromBox = fromBox;
            this.toBox = toBox;
            fromBox.getPoint(ref connectFrom, v1);
            toBox.getPoint(ref connectTo, v2);
            cF = v1;
            cT = v2;
            
        }

        public Point ConnectFrom {
            get { fromBox.getPoint(ref connectFrom, cF); return connectFrom; }

        }

        public Point ConnectTo {
            get { toBox.getPoint(ref connectTo, cT); return connectTo; }
        }

        public void raisePropertyChanged() {
            RaisePropertyChanged(nameof(ConnectFrom));
            RaisePropertyChanged(nameof(ConnectTo));
        }
    }
        
}
