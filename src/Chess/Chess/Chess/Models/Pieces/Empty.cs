using Chess.Config;
using Chess.Moves;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Pieces
{
    internal class Empty : Piece
    {

        public Empty() : base(Player.None, Constants.TEXT_EMPTY, "Empty")
        {
        }

        public override Piece Clone()
        {
            var newPiece = new Empty();
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves(bool isActiveMove)
        {
            return new List<Move>();
        }
    }
}
