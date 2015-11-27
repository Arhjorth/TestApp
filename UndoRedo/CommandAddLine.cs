using TestApp.Model;
using System.Collections.ObjectModel;
using TestApp.ViewModel;

namespace TestApp.UndoRedo {
    public class CommandAddLine : IUndoRedoCommand {
        private ObservableCollection<LineViewModel> lines;

        private LineViewModel line;


        public CommandAddLine(LineViewModel line, ObservableCollection<LineViewModel> lines) {
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
