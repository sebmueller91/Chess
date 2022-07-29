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
            GameState game = Helpers.GetCurrentGame();
            {
                var a = 2;
            }
            game.SetBoardEntry(NewCell, game.Board[OldCell.Row][OldCell.Col]);
            game.SetBoardEntry(OldCell, new Empty());
        }

        public override void Rollback()
        {
            GameState game = Helpers.GetCurrentGame();
            game.SetBoardEntry(OldCell, Helpers.GetCurrentGame().Board[NewCell.Row][NewCell.Col]);
            game.SetBoardEntry(NewCell, new Empty());
        }

        public override RevertableAction Clone()
        {
            return new MovePieceAction(OldCell.Clone(), NewCell.Clone());
        }
    }
}
