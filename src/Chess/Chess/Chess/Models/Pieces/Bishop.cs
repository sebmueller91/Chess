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

        public Bishop(GameState game, Player player) : base(game, player, Constants.TEXT_BISHOP)
        {
        }
        public override Piece Clone()
        {
            var newPiece = new Bishop(Game, Player);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves()
        {
            var possibleMoves = new List<Move>();
            PieceMoveHelpers.AddCellsOnDiagonalLines(possibleMoves, Position, Game, Player);
            return possibleMoves;
        }
    }
}