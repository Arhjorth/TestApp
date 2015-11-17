using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model {
    public class Diagram {
        public List<ClassBox> ClassBoxes { get; set; }
        public List<Line> Lines { get; set; }
    }
}
