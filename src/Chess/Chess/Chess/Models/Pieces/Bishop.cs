using Chess.Config;
using Chess.Moves;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Pieces
{
    internal class Bishop : Piece
    {
        public Bishop() : base(Constants.TEXT_BISHOP, "Bishop")
        {

        }

        public Bishop(Player player) : base(player, Constants.TEXT_BISHOP, "Bishop")
        {
        }
        public override Piece Clone()
        {
            var newPiece = new Bishop(Player);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves(bool isActiveMove)
        {
            var possibleMoves = new List<Move>();
            PieceMoveHelpers.AddCellsOnDiagonalLines(possibleMoves, Position, Helpers.GetCurrentGame());
            return possibleMoves;
        }
    }
}