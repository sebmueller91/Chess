using Chess.Config;
using Chess.Moves;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Pieces
{
    internal class Queen : Piece
    {
        public Queen() : base(Constants.TEXT_QUEEN, "Queen")
        {

        }

        public Queen(Player player) : base(player, Constants.TEXT_QUEEN, "Queen")
        {
        }

        public override Piece Clone()
        {
            var newPiece = new Queen(Player);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves(bool isActiveMove)
        {
            var possibleMoves = new List<Move>();
            PieceMoveHelpers.AddNeightboringCells(possibleMoves, Position, Helpers.GetCurrentGame());
            PieceMoveHelpers.AddCellsOnStraightLines(possibleMoves, Position, Helpers.GetCurrentGame());
            PieceMoveHelpers.AddCellsOnDiagonalLines(possibleMoves, Position, Helpers.GetCurrentGame());
            return possibleMoves;
        }
    }
}
