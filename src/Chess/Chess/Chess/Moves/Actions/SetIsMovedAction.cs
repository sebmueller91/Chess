using Chess.Models;
using Chess.Services;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves.Actions
{
    internal class SetIsMovedAction : RevertableAction
    {
        public Cell Position { get; set; }
        public bool PreviousState { get; set; }

        public SetIsMovedAction(Cell position) : base("SetIsMovedAction")
        {
            Position = position;
        }

        public override void Execute()
        {
            if (Position != null && Helpers.GetCurrentGame()?.Board != null)
            {
                PreviousState = Helpers.GetCurrentGame().Board[Position.Row][Position.Col].IsMoved;
                Helpers.GetCurrentGame().Board[Position.Row][Position.Col].IsMoved = true;
            }
        }

        public override void Rollback()
        {
            if (Position != null && Helpers.GetCurrentGame()?.Board != null)
            {
                Helpers.GetCurrentGame().Board[Position.Row][Position.Col].IsMoved = PreviousState;
            }
        }

        public override RevertableAction Clone()
        {
            var newAction = new SetIsMovedAction(Position.Clone());
            newAction.PreviousState = this.PreviousState;
            return newAction;
        }
    }
}
