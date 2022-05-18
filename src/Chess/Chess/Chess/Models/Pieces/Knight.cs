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
        public Knight(GameState game, Player player) : base(game, player, Constants.TEXT_KNIGHT)
        {
        }

        public override Piece Clone()
        {
            var newPiece = new Knight(Game, Player);
            return this.CloneProperties(newPiece);
        }
        public override List<Move> GetPossibleMoves()
        {
            var possibleMoves = new List<Move>();

            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 2, Position.Col + 1), Game, Player);
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 2, Position.Col - 1), Game, Player);
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 2, Position.Col + 1), Game, Player);
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 2, Position.Col - 1), Game, Player);
                           
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 1, Position.Col + 2), Game, Player);
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 1, Position.Col + 2), Game, Player);
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 1, Position.Col - 2), Game, Player);
            PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 1, Position.Col - 2), Game, Player);

            return possibleMoves;
        }
    }
}
