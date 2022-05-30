using Chess.Models;
using Chess.Models.Pieces;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves.Actions
{
    internal class CapturePieceAction : RevertableAction
    {
        public Cell Position { get; set; }
        public Piece Piece { get; set; }

        public CapturePieceAction(Cell cell) : base("CapturePieceAction")
        {
            Position = cell;
        }

        public override void Execute()
        {
            Piece = Helpers.GetCurrentGame().Board[Position.Row][Position.Col];
            Helpers.GetCurrentGame().SetBoardEntry(Position, new Empty());
        }

        public override void Rollback()
        {
            Helpers.GetCurrentGame().Board[Position.Row][Position.Col] = Piece;
        }

        public override RevertableAction Clone()
        {
            return new CapturePieceAction(Position.Clone());
        }
    }
}
