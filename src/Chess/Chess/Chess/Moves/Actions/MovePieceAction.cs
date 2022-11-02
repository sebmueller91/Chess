using Chess.Models;
using Chess.Models.Pieces;
using Chess.Services;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves.Actions
{
    internal class MovePieceAction : RevertableAction
    {
        public Cell OldCell { get; set; }
        public Cell NewCell { get; set; }

        public MovePieceAction(Cell fromCell, Cell toCell) : base("MovePieceAction")
        {
            OldCell = fromCell;
            NewCell = toCell;
        }

        public override void Execute()
        {
            var game = Helpers.GetCurrentGame();
            game.SetBoardEntry(NewCell, game.Board[OldCell.Row][OldCell.Col]);
            game.SetBoardEntry(OldCell, new Empty());
        }

        public override void Rollback()
        {
            var game = Helpers.GetCurrentGame();
            var emptyCell = game.Board[OldCell.Row][OldCell.Col];
            game.SetBoardEntry(OldCell, game.Board[NewCell.Row][NewCell.Col]);
            game.SetBoardEntry(NewCell, emptyCell);
        }

        public override RevertableAction Clone()
        {
            return new MovePieceAction(OldCell.Clone(), NewCell.Clone());
        }
    }
}
