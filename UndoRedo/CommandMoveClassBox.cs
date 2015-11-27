using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using TestApp.ViewModel;

namespace TestApp.UndoRedo {
    public class CommandMoveClassBox : IUndoRedoCommand {

        private ClassBoxViewModel classBox;
        private double offsetX;
        private double offsetY;

        public CommandMoveClassBox(ClassBoxViewModel _classBox, double _offsetX, double _offsetY) {
            classBox = _classBox;
            offsetX = _offsetX;
            offsetY = _offsetY;
        }


        public void Execute() {
            classBox.PosX += offsetX;
            classBox.PosY += offsetY;
        }

        public void UnExecute() {
            classBox.PosX -= offsetX;
            classBox.PosY -= offsetY;
        }
    }
}


