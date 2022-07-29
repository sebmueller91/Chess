using Chess.Config;
using Chess.Moves;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Pieces
{
    internal class Knight : Piece
    {
        public Knight() : base(Constants.TEXT_KNIGHT, "Knight")
        {

        }

        public Knight(Player player) : base(player, Constants.TEXT_KNIGHT, "Knight")
        {
        }

        public override Piece Clone()
        {
            var newPiece = new Knight(Player);
            return this.CloneProperties(newPiece);
        }
        public override List<Move> GetPossibleMoves(bool isActiveMove) // TODO: Remove isActiveMove
        {
            var possibleMoves = new List<Move>();

            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 2, Position.Col + 1), Helpers.GetCurrentGame());
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 2, Position.Col - 1), Helpers.GetCurrentGame());
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 2, Position.Col + 1), Helpers.GetCurrentGame());
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 2, Position.Col - 1), Helpers.GetCurrentGame());
                                                                                                               
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 1, Position.Col + 2), Helpers.GetCurrentGame());
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 1, Position.Col + 2), Helpers.GetCurrentGame());
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 1, Position.Col - 2), Helpers.GetCurrentGame());
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 1, Position.Col - 2), Helpers.GetCurrentGame());

            return possibleMoves;
        }
    }
}
