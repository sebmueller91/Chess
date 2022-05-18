using Chess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Moves
{
    public class MoveStack
    {
        public List<Move> Moves { get; private set; }
        public Move Iterator { get; private set; }
        private int _iteratorIndex;
        public int IteratorIndex
        {
            get { return _iteratorIndex; }
            private set
            {
                if (value >= 0 && value < Moves.Count)
                {
                    _iteratorIndex = value;
                    Iterator = Moves[_iteratorIndex];
                }
            }
        }

        public MoveStack()
        {
            Moves = new List<Move>();
            Moves.Add(null);
            IteratorIndex = 0;
        }

        public MoveStack Clone()
        {
            var newMoveStack = new MoveStack();
            for (int i = 0; i < Moves.Count; i++)
            {
                newMoveStack.Moves.Add(Moves[i].Clone());
            }
            return newMoveStack;
        }

        public void ResetMoveStack()
        {
            Moves.Clear();
        }

        public bool DoneActionsOnStack()
        {
            return IteratorIndex > 0;
        }

        public bool UndoneActionsOnStack()
        {
            return IteratorIndex < Moves.Count-1;
        }

        public void ExecuteMove(Move move)
        {
            DeleteElementsAfterIndex(IteratorIndex);
            move.Execute();
            Moves.Add(move);
            IteratorIndex = Moves.Count - 1;
        }

        public void RollbackLastMove()
        {
            if (!DoneActionsOnStack())
            {
                return;
            }
            Iterator.Rollback();
            IteratorIndex--;
        }

        public void RedoNextMoveOnStack()
        {
            if (!UndoneActionsOnStack())
            {
                return;
            }
            IteratorIndex++;
            Iterator.Execute();
        }

        private void DeleteElementsAfterIndex(int index)
        {
            if (index < 0 || index >= Moves.Count)
            {
                return;
            }
            while (Moves.Count > index+1)
            {
                Moves.RemoveAt(Moves.Count-1);
            }
        }
        // endregion
    }
}