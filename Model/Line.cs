using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestApp.Model {
    public class Line : ViewModelBase {

        private ClassBox fromBox;
        private ClassBox toBox;
        
        public Line(ClassBox fromBox, ClassBox toBox) {
            FromBox = fromBox;
            ToBox = toBox;
        }
        
        
        public ClassBox FromBox { get { return fromBox; } set { fromBox = value; RaisePropertyChanged(); } }

        public ClassBox ToBox { get { return toBox; } set { toBox = value; RaisePropertyChanged(); } }
        
    }
}
