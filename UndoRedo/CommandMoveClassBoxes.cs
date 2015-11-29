using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using TestApp.ViewModel;

namespace TestApp.UndoRedo {
    public class CommandMoveClassBoxes : IUndoRedoCommand {

        private List<ClassBoxViewModel> classBoxes;
        private double offsetX;
        private double offsetY;

        public CommandMoveClassBoxes(List<ClassBoxViewModel> _classBoxes, double _offsetX, double _offsetY) {
            classBoxes = _classBoxes;
            offsetX = _offsetX;
            offsetY = _offsetY;
        }


        public void Execute() {
            foreach (var box in classBoxes) {
                box.PosX += offsetX;
                box.PosY += offsetY;
            }
        }

        public void UnExecute() {
            foreach (var box in classBoxes) {
                box.PosX -= offsetX;
                box.PosY -= offsetY;
            }
        }
    }
}


