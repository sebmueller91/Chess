using Chess.Config;
using Chess.Moves;
using Chess.Moves.Actions;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Models.Pieces
{
    internal class King : Piece
    {
        public King(GameState game, Player player) : base(game, player, Constants.TEXT_KING)
        {
        }

        public override Piece Clone()
        {
            var newPiece = new King(Game, Player);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves()
        {
            var possibleMoves = new List<Move>();
            var king = Game.Board[Position.Row, Position.Col];
            PieceMoveHelpers.AddNeightboringCells(possibleMoves, Position, Game, Player);

            // Castling
            if (Game.CurrentPlayer != king.Player)
            {
                return possibleMoves;
            }
            var checkFields = PieceMoveHelpers.GetFieldsInCheck(Game, Player);
            if (checkFields[Position.Row, Position.Col])
            {
                return possibleMoves;
            }

            if (Player == Player.White)
            {
                // King-side castling
                if (CheckCastlingConditionFulfilled(new Cell(7, 4), new Cell(7, 7), Game, Player))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(7, 6), Game, Player))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new MovePieceAction(new Cell(7, 7), new Cell(7, 5), Game));
                    }
                }

                // Queen-side castling
                if (CheckCastlingConditionFulfilled(new Cell(7, 4), new Cell(7, 0), Game, Player))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(7, 2), Game, Player))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new MovePieceAction(new Cell(7, 0), new Cell(7, 3), Game));
                    }
                }
            }
            else
            {
                // King-side castling
                if (CheckCastlingConditionFulfilled(new Cell(0, 4), new Cell(0, 7), Game, Player))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(0, 6), Game, Player))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new MovePieceAction(new Cell(0, 7), new Cell(0, 5), Game));
                    }
                }

                // Queen-side castling
                if (CheckCastlingConditionFulfilled(new Cell(0, 4), new Cell(0, 0), Game, Player))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(0, 2), Game, Player))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new MovePieceAction(new Cell(0, 0), new Cell(0, 3), Game));
                    }
                }
            }
            return possibleMoves;
        }

        private bool CheckCastlingConditionFulfilled(Cell kingPosition, Cell rookPosition, GameState Game, Player Player)
        {
            var king = Game.Board[kingPosition.Row, kingPosition.Col];
            var rook = Game.Board[rookPosition.Row, rookPosition.Col];


            if (!(king is King
                && king.Player == Player
                && !king.IsMoved))
            {
                return false;
            }

            if (!(rook is Rook
                    && rook.Player == Player
                    && !rook.IsMoved))
            {
                return false;
            }

            // Check if fields between rook and king are empty and are not in check
            var checkFields = PieceMoveHelpers.GetFieldsInCheck(Game, Player);
            var row = kingPosition.Row;
            var colMax = kingPosition.Col > rookPosition.Col ? kingPosition.Col : rookPosition.Col;
            var colMin = kingPosition.Col > rookPosition.Col ? rookPosition.Col : kingPosition.Col;
            for (int i = colMin + 1; i < colMax; i++)
            {
                if (!(Game.Board[row, i] is Empty) || checkFields[row, i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
