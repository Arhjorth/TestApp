using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using TestApp.ViewModel;

namespace TestApp.UndoRedo {
    class CommandAddClassBox : IUndoRedoCommand {
        private ClassBoxViewModel classBox;
        private ObservableCollection<ClassBoxViewModel> classBoxes;

        public CommandAddClassBox(ClassBoxViewModel _ClassBox, ObservableCollection<ClassBoxViewModel> _ClassBoxes) {
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
