using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI.PresentationDesign.DesignUI.Services
{
    public delegate void HistoryChanged(object Target); 

    /// <summary>
    /// Global undo service
    /// </summary>
    public class UndoService
    {
        private static UndoService _instance;
        Stack<IUndoRedoAction> undoActions = new Stack<IUndoRedoAction>();
        Stack<IUndoRedoAction> redoActions = new Stack<IUndoRedoAction>();

        public event HistoryChanged OnHistoryChanged;

        public static UndoService Instance
        {
            get
            {
                return _instance;
            }
        }

        public UndoService()
        {
            _instance = this;
        }

        public static UndoService CreateUndoService()
        {
            return new UndoService();
        }


        /// <summary>
        /// Записывает действие в очередь на отмену
        /// </summary>
        public void PushAction(IUndoRedoAction action)
        {
            undoActions.Push(action);
            FireChanged(action.Target);
        }

        /// <summary>
        /// Возвращает первое ожидающее отмены действие
        /// </summary>
        public IUndoRedoAction PeekUndoAction()
        {
            return undoActions.Peek();
        }

        /// <summary>
        /// Возвращает первое ожидающее повтора действие
        /// </summary>
        public IUndoRedoAction PeekRedoAction()
        {
            return redoActions.Peek();
        }

        /// <summary>
        /// Очищает историю действий
        /// </summary>
        public void ClearHistory()
        {
            undoActions.Clear();
            redoActions.Clear();

            FireChanged(null);
        }

        /// <summary>
        /// Отменяет действие
        /// </summary>
        public void Undo()
        {
            IUndoRedoAction action = undoActions.Pop();
            action.Undo();
            redoActions.Push(action);

            FireChanged(action.Target);
        }

        /// <summary>
        /// Повторяет отмененное действие
        /// </summary>
        public void Redo()
        {
            IUndoRedoAction action = redoActions.Pop();
            action.Redo();
            undoActions.Push(action);

            FireChanged(action.Target);
        }


        /// <summary>
        /// true, если есть действие, ожидающее отмены
        /// </summary>
        public bool CanUndo
        {
            get
            {
                if (undoActions.Count > 0)
                {
                    IUndoRedoAction action = undoActions.Peek();
                    return action.CanUndo();
                }
                return false;
            }
        }

        /// <summary>
        /// true, если есть действие, ожидающее повтора
        /// </summary>
        public bool CanRedo
        {
            get
            {
                if (redoActions.Count > 0)
                {
                    IUndoRedoAction action = redoActions.Peek();
                    return action.CanRedo();
                }
                return false;
            }
        }


        private void FireChanged(object Target)
        {
            if (OnHistoryChanged != null)
                OnHistoryChanged(Target);
        }
    }
}
