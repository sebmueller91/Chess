using Chess.Config;
using Chess.Moves;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Pieces
{
    internal class Empty : Piece
    {
        public Empty(GameState game) : base(game, Player.None, Constants.TEXT_EMPTY)
        {
        }

        public override Piece Clone()
        {
            var newPiece = new Empty(Game);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves()
        {
            return new List<Move>();
        }
    }
}
