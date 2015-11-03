using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.UndoRedo {
   public  class UndoRedoController {

        // undo and redo stacks
        private readonly Stack<IUndoRedoCommand> undoStack = new Stack<IUndoRedoCommand>();
        private readonly Stack<IUndoRedoCommand> redoStack = new Stack<IUndoRedoCommand>();

        // makes an instance and a getter for it
        public static UndoRedoController Instance { get; } = new UndoRedoController();
        // Part of the Singleton design pattern.
        // This ensures that only the 'UndoRedoController' can instantiate itself.
        private UndoRedoController() { }

        public void AddAndExecute(IUndoRedoCommand command) {
            undoStack.Push(command);
            redoStack.Clear();
            command.Execute();
        }


        public bool CanUndo() => undoStack.Any();

        public void Undo() {
            if (!undoStack.Any()) throw new InvalidOperationException();

            var command = undoStack.Pop();
            redoStack.Push(command);
            command.UnExecute();
        }


        public bool CanRedo() => redoStack.Any();

        public void Redo() {
            if (!redoStack.Any()) throw new InvalidOperationException();

            var command = redoStack.Pop();
            undoStack.Push(command);
            command.Execute();
        }
    }
    public interface IUndoRedoCommand {
        void Execute();
        void UnExecute();
    }
}

