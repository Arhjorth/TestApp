using System.Collections.ObjectModel;
using TestApp.ViewModel;

namespace TestApp.UndoRedo {
    class CommandDeleteClassBox: IUndoRedoCommand {

        private ObservableCollection<LineViewModel> lines;
        private ObservableCollection<ClassBoxViewModel> classBoxes;

        private ClassBoxViewModel classBox;


        public CommandDeleteClassBox(ClassBoxViewModel classBox, ObservableCollection<ClassBoxViewModel> classBoxes, ObservableCollection<LineViewModel> lines) {
            this.lines = lines;
            this.classBoxes = classBoxes;
            this.classBox = classBox;
        }


        public void Execute() {
            foreach(LineViewModel line in lines) {
                line.toBox.LineList.Remove(line);
                line.fromBox.LineList.Remove(line);
                lines.Remove(line);
            }
            classBoxes.Remove(classBox);
        }

        public void UnExecute() {
            classBoxes.Add(classBox);
            foreach (LineViewModel line in lines) {
                line.toBox.LineList.Add(line);
                line.fromBox.LineList.Add(line);
                lines.Add(line);
            }
        }
    }
}

