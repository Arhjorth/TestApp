using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;

namespace TestApp.UndoRedo {
    class CommandAddClassBox : IUndoRedoCommand {
        private ClassBox classBox;
        private ObservableCollection<ClassBox> classBoxes;

        public CommandAddClassBox(ClassBox _ClassBox, ObservableCollection<ClassBox> _ClassBoxes) {
            classBox = _ClassBox;
            classBoxes = _ClassBoxes;
        }

        public void Execute() {
            classBoxes.Add(classBox);
        }

        public void UnExecute() {
            classBoxes.Remove(classBox);
        }
    }
}
