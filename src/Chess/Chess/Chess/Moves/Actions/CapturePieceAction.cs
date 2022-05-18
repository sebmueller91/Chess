using Chess.Models;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves.Actions
{
    internal class CapturePieceAction : RevertableAction
    {
        private Cell Position { get; set; }
        private Piece Piece { get; set; }

        public CapturePieceAction(Cell cell, GameState game) : base(game)
        {
            Position = cell;
        }

        public override void Execute()
        {
            Piece = Game.Board[Position.Row, Position.Col];
            Game.SetBoardEntry(Position, new Empty(Game));
        }

        public override void Rollback()
        {
            Game.Board[Position.Row, Position.Col] = Piece;
        }

        public override RevertableAction Clone()
        {
            return new CapturePieceAction(Position.Clone(), Game);
        }
    }
}
