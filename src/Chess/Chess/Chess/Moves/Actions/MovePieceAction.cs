using Chess.Models;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves.Actions
{
    internal class MovePieceAction : RevertableAction
    {
        public Cell OldCell { get; private set; }
        public Cell NewCell { get; private set; }

        public MovePieceAction(Cell fromCell, Cell toCell, GameState game) : base(game)
        {
            OldCell = fromCell;
            NewCell = toCell;
        }

        public override void Execute()
        {
            Game.SetBoardEntry(NewCell, Game.Board[OldCell.Row, OldCell.Col]);
            Game.SetBoardEntry(OldCell, new Empty(Game));
        }

        public override void Rollback()
        {
            Game.SetBoardEntry(OldCell, Game.Board[NewCell.Row, NewCell.Col]);
            Game.SetBoardEntry(NewCell, new Empty(Game));
        }

        public override RevertableAction Clone()
        {
            return new MovePieceAction(OldCell.Clone(), NewCell.Clone(), Game);
        }
    }
}
