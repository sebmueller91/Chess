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
        public King() : base(Constants.TEXT_KING, "King")
        {

        }

        public King(Player player) : base(player, Constants.TEXT_KING, "King")
        {
        }

        public override Piece Clone()
        {
            var newPiece = new King(Player);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves(bool isActiveMove)
        {
            var possibleMoves = new List<Move>();
            var king = Helpers.GetCurrentGame().Board[Position.Row][Position.Col];
            PieceMoveHelpers.AddNeightboringCells(possibleMoves, Position, Helpers.GetCurrentGame());

            // Castling
            if (!isActiveMove)
            {
                return possibleMoves;
            }
            var checkFields = PieceMoveHelpers.GetFieldsInCheck(Helpers.GetCurrentGame(), Player);
            if (checkFields[Position.Row, Position.Col])
            {
                return possibleMoves;
            }

            if (Player == Player.White)
            {
                // King-side castling
                if (CheckCastlingConditionFulfilled(new Cell(7, 4), new Cell(7, 7), Helpers.GetCurrentGame()))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(7, 6), Helpers.GetCurrentGame()))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new MovePieceAction(new Cell(7, 7), new Cell(7, 5)));
                    }
                }

                // Queen-side castling
                if (CheckCastlingConditionFulfilled(new Cell(7, 4), new Cell(7, 0), Helpers.GetCurrentGame()))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(7, 2), Helpers.GetCurrentGame()))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new MovePieceAction(new Cell(7, 0), new Cell(7, 3)));
                    }
                }
            }
            else
            {
                // King-side castling
                if (CheckCastlingConditionFulfilled(new Cell(0, 4), new Cell(0, 7), Helpers.GetCurrentGame()))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(0, 6), Helpers.GetCurrentGame()))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new MovePieceAction(new Cell(0, 7), new Cell(0, 5)));
                    }
                }

                // Queen-side castling
                if (CheckCastlingConditionFulfilled(new Cell(0, 4), new Cell(0, 0), Helpers.GetCurrentGame()))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(0, 2), Helpers.GetCurrentGame()))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new MovePieceAction(new Cell(0, 0), new Cell(0, 3)));
                    }
                }
            }
            return possibleMoves;
        }

        private bool CheckCastlingConditionFulfilled(Cell kingPosition, Cell rookPosition, GameState Game)
        {
            var king = Game.Board[kingPosition.Row][kingPosition.Col];
            var rook = Game.Board[rookPosition.Row][rookPosition.Col];


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
                if (!(Game.Board[row][i] is Empty) || checkFields[row, i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
