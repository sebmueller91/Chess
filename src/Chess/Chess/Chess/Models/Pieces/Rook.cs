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
        public Rook(GameState game, Player player) : base(game, player, Constants.TEXT_ROOK)
        {
        }

        public override Piece Clone()
        {
            var newPiece = new Rook(Game, Player);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves()
        {
            var possibleMoves = new List<Move>();
            PieceMoveHelpers.AddCellsOnStraightLines(possibleMoves, Position, Game, Player);
            return possibleMoves;
        }
    }
}
