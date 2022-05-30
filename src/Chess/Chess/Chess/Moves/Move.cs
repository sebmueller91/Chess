using Chess.Models;
using Chess.Moves.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves
{
    public class Move
    {
        public Cell FromCell { get; set; }
        public Cell ToCell { get; set; }

        public List<RevertableAction> Actions { get; set; } = new List<RevertableAction>();

        public Move(Cell fromCell, Cell toCell, List<RevertableAction> actions = null)
        {
            FromCell = fromCell;
            ToCell = toCell;

            if (actions == null) { 
            Actions.Add(new MovePieceAction(fromCell, toCell));
            } else
            {
                Actions = actions;
            }
        }

        public void Execute()
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Execute();
            }
        }

        public void Rollback()
        {
            for (int i = Actions.Count - 1; i >= 0; i--)
            {
                Actions[i].Rollback();
            }
        }

        public Move Clone()
        {
            var newActions = new List<RevertableAction>();
            for (int i = 0; i < Actions.Count; i++)
            {
                newActions.Add(Actions[i].Clone());
            }
            var newMove = new Move(FromCell.Clone(), ToCell.Clone(), newActions);
            return newMove;
        }
    }
}
