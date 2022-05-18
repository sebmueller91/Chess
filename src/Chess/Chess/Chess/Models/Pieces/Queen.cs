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
        public Queen(GameState game, Player player) : base(game, player, Constants.TEXT_QUEEN)
        {
        }

        public override Piece Clone()
        {
            var newPiece = new Queen(Game, Player);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves()
        {
            var possibleMoves = new List<Move>();
            PieceMoveHelpers.AddNeightboringCells(possibleMoves, Position, Game, Player);
            PieceMoveHelpers.AddCellsOnStraightLines(possibleMoves, Position, Game, Player);
            PieceMoveHelpers.AddCellsOnDiagonalLines(possibleMoves, Position, Game, Player);
            return possibleMoves;
        }
    }
}
