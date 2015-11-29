using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestApp.ViewModel;

namespace TestApp.UndoRedo {
    class CommandDeleteClassBox : IUndoRedoCommand {

        private ObservableCollection<LineViewModel> lines;
        private ObservableCollection<ClassBoxViewModel> classBoxes;

        private ClassBoxViewModel classBox;
        private List<LineViewModel> linesToRemove;


        public CommandDeleteClassBox(ClassBoxViewModel classBox, ObservableCollection<ClassBoxViewModel> classBoxes, ObservableCollection<LineViewModel> lines) {
            this.lines = lines;
            this.classBoxes = classBoxes;
            this.classBox = classBox;

            linesToRemove = classBox.LineList.Cast<LineViewModel>().ToList(); // Copy on classbox.lineList, to ensure that we know which lines to delete.
        }

        public void Execute() {


            foreach (LineViewModel line in linesToRemove) {
                line.fromBox.LineList.Remove(line);
                line.toBox.LineList.Remove(line);
                lines.Remove(line);
            }

            classBoxes.Remove(classBox);
        }

        public void UnExecute() {
            classBoxes.Add(classBox);

            foreach (LineViewModel line in linesToRemove) {
                line.fromBox.LineList.Add(line);
                line.toBox.LineList.Add(line);
                lines.Add(line);
            }
        }
    }
}