using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;

namespace TestApp.UndoRedo {
    public class CommandMoveClassBox : IUndoRedoCommand {

        ClassBox classBox;
        double offsetX;
        double offsetY;

        public CommandMoveClassBox(ClassBox _classBox, double _offsetX, double _offsetY) {
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


