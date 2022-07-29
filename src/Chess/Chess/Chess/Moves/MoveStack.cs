using Chess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Moves
{
    public class MoveStack
    {
        public List<Move> Moves { get; set; } = new List<Move>();

        public int IteratorIndex { get; set; } = -1;

        public MoveStack Clone()
        {
            var newMoveStack = new MoveStack();
            for (int i = 0; i < Moves.Count; i++)
            {
                newMoveStack.Moves.Add(Moves[i].Clone());
            }
            newMoveStack.IteratorIndex = IteratorIndex;
            return newMoveStack;
        }

        public void ResetMoveStack()
        {
            Moves.Clear();
        }

        public bool DoneActionsOnStack()
        {
            return IteratorIndex >= 0;
        }

        public bool UndoneActionsOnStack()
        {
            return IteratorIndex < Moves.Count-1;
        }

        public void ExecuteMove(Move move)
        {
            DeleteElementsAfterIndex((int)IteratorIndex);
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
            Moves[IteratorIndex].Rollback();
            IteratorIndex--;
        }

        public void RedoNextMoveOnStack()
        {
            if (!UndoneActionsOnStack())
            {
                return;
            }
            IteratorIndex++;
            Moves[IteratorIndex].Execute();
        }
        
        private void DeleteElementsAfterIndex(int index)
        {
            if (index <= 0 || index >= Moves.Count)
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