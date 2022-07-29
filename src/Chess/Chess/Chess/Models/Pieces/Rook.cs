using Chess.Config;
using Chess.Moves;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Pieces
{
    internal class Rook : Piece
    {
        public Rook() : base (Constants.TEXT_ROOK, "Rook")
        {

        }

        public Rook(Player player) : base(player, Constants.TEXT_ROOK, "Rook")
        {
        }

        public override Piece Clone()
        {
            var newPiece = new Rook(Player);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves(bool isActiveMove)
        {
            var possibleMoves = new List<Move>();
            PieceMoveHelpers.AddCellsOnStraightLines(possibleMoves, Position, Helpers.GetCurrentGame());
            return possibleMoves;
        }
    }
}
