using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using System.Collections.ObjectModel;

namespace TestApp.UndoRedo {
    public class CommandAddLine : IUndoRedoCommand {
        private ObservableCollection<Line> lines;

        private Line line;


        public CommandAddLine(ObservableCollection<Line> lines, Line line) {
            this.lines = lines;
            this.line = line;
        }


        public void Execute() {
            lines.Add(line);
        }

        public void UnExecute() {
            lines.Remove(line); 
        }
    }
}
