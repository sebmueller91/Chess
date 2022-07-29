using Chess.Models;
using System;
using System.Collections.Generic;
using Chess.Utils;
using System.Linq;
using Chess.Models.Pieces;
using Chess.Moves;
using Chess.Moves.Actions;

namespace Chess.Utils
{
    public static class PieceMoveHelpers
    {
        public static bool[,] GetFieldsInCheck(GameState game, Player playerToCheck)
        {
            var checkFields = new bool[8, 8];
            var possibleMoves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game.Board[i][j].Player == Helpers.GetOpposingPlayer(playerToCheck))
                    {
                        var possibleMovesOfCurPiece = game.Board[i][j].GetPossibleMoves(false);
                        possibleMoves.AddRange(possibleMovesOfCurPiece);
                    }
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (possibleMoves.Any(x => x.ToCell.Row == i && x.ToCell.Col == j))
                    {
                        checkFields[i, j] = true;
                    }
                }
            }

            return checkFields;
        }

        public static void AddCellsOnStraightLines(List<Move> possibleMoves, Cell position, GameState game)
        {
            if (game.Board[position.Row][position.Col] is Empty)
            {
                return;
            }
            var player = game.Board[position.Row][position.Col].Player;

            for (int r = position.Row + 1; r < 8; r++)
            {
                if (!AddCellAndCheckIfToContinue(possibleMoves, position, new Cell(r, position.Col), game, player))
                {
                    break;
                }
            }

            for (int r = position.Row - 1; r >= 0; r--)
            {
                if (!AddCellAndCheckIfToContinue(possibleMoves, position, new Cell(r, position.Col), game, player))
                {
                    break;
                }
            }


            for (int c = position.Col + 1; c < 8; c++)
            {
                if (!AddCellAndCheckIfToContinue(possibleMoves, position, new Cell(position.Row, c), game, player))
                {
                    break;
                }
            }


            for (int c = position.Col - 1; c >= 0; c--)
            {
                if (!AddCellAndCheckIfToContinue(possibleMoves, position, new Cell(position.Row, c), game, player))
                {
                    break;
                }
            }
        }

        public static void AddCellsOnDiagonalLines(List<Move> possibleMoves, Cell position, GameState game)
        {
            if (game.Board[position.Row][position.Col] is Empty)
            {
                return;
            }
            var player = game.Board[position.Row][position.Col].Player;

            for (int i = 1; i < 7; i++)
            {
                int rRightUp = position.Row - i;
                int cRightUp = position.Col + i;
                if (rRightUp >= 0 && cRightUp < 8)
                {
                    if (!AddCellAndCheckIfToContinue(possibleMoves, position, new Cell(rRightUp, cRightUp), game, player))
                    {
                        break;
                    }
                }
            }

            for (int i = 1; i < 7; i++)
            {
                int rRightDown = position.Row + i;
                int cRightDown = position.Col + i;
                if (rRightDown < 8 && cRightDown < 8)
                {
                    if (!AddCellAndCheckIfToContinue(possibleMoves, position, new Cell(rRightDown, cRightDown), game, player))
                    {
                        break;
                    }
                }
            }

            for (int i = 1; i < 7; i++)
            {
                int rLeftUp = position.Row - i;
                int cLeftUp = position.Col - i;
                if (rLeftUp >= 0 && cLeftUp >= 0)
                {
                    if (!AddCellAndCheckIfToContinue(possibleMoves, position, new Cell(rLeftUp, cLeftUp), game, player))
                    {
                        break;
                    }
                }
            }

            for (int i = 1; i < 7; i++)
            {
                int rLeftDown = position.Row + i;
                int cLeftDown = position.Col - i;
                if (rLeftDown < 8 && cLeftDown >= 0)
                {
                    if (!AddCellAndCheckIfToContinue(possibleMoves, position, new Cell(rLeftDown, cLeftDown), game, player))
                    {
                        break;
                    }
                }
            }
        }

        public static bool AddCellAndCheckIfToContinue(List<Move> possibleMoves, Cell fromCell, Cell toCell, GameState game, Player player)
        {
            if (game.Board[toCell.Row][toCell.Col].Player == player)
            {
                return false;
            }
            AddIfValid(possibleMoves, fromCell, toCell, game);
            if (game.Board[toCell.Row][toCell.Col].Player == Helpers.GetOpposingPlayer(player))
            {
                return false;
            }
            return true;
        }

        public static void AddNeightboringCells(List<Move> possibleMoves, Cell position, GameState game)
        {
            if (game.Board[position.Row][position.Col] is Empty)
            {
                return;
            }
            var player = game.Board[position.Row][position.Col].Player;

            AddIfValid(possibleMoves, position, new Cell(position.Row + 1, position.Col - 1), game);
            AddIfValid(possibleMoves, position, new Cell(position.Row + 1, position.Col), game);
            AddIfValid(possibleMoves, position, new Cell(position.Row + 1, position.Col + 1), game);
            AddIfValid(possibleMoves, position, new Cell(position.Row, position.Col - 1), game);
            AddIfValid(possibleMoves, position, new Cell(position.Row, position.Col + 1), game);
            AddIfValid(possibleMoves, position, new Cell(position.Row - 1, position.Col - 1), game);
            AddIfValid(possibleMoves, position, new Cell(position.Row - 1, position.Col), game);
            AddIfValid(possibleMoves, position, new Cell(position.Row - 1, position.Col + 1), game);
        }

        public static bool AddIfValid(List<Move> possibleMoves, Cell fromCell, Cell toCell, GameState game)

        {
            return AddIfValid(possibleMoves, fromCell, toCell, game, (a, b) => true);
        }

        public static bool AddIfValid(List<Move> possibleMoves, Cell fromCell, Cell toCell, GameState game, Func<int, int, bool> condition)
        {
            if (game.Board[fromCell.Row][fromCell.Col] is Empty)
            {
                return false;
            }
            var player = game.Board[fromCell.Row][fromCell.Col].Player;

            if (toCell.Row < 0 || toCell.Row > 7 || toCell.Col < 0 || toCell.Col > 7)
            {
                return false;
            }
            if (game.Board[toCell.Row][toCell.Col].Player == player)
            {
                return false;
            }

            // Don't add duplicate moves
            if (possibleMoves.Any(x => x.FromCell == fromCell && x.ToCell == toCell))
            {
                return false;
            }

            if (condition(toCell.Row, toCell.Col))
            {
                var move = new Move(fromCell, new Cell(toCell.Row, toCell.Col));
                possibleMoves.Add(move);
                return true;
            }
            return false;
        }

        public static void AddActionToPreviousMove(List<Move> possibleMoves, RevertableAction action)
        {
            possibleMoves?.Last()?.Actions.Add(action);
        }

        public static bool CheckIfMovementWasDoneInLastMove(Cell oldPosition, Cell newPosition, GameState game)
        {
            if (game.MoveStack.Moves.Count == 0)
            {
                return false;
            }

            var lastMove = game.MoveStack.Moves.Last();
            if (lastMove == null)
            {
                return false;
            }
            foreach (var action in lastMove.Actions)
            {
                if (action is MovePieceAction)
                {
                    var moveAction = action as MovePieceAction;
                    if (moveAction.OldCell== oldPosition
                        && moveAction.NewCell== newPosition)
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        public static Cell GetPositionOfKing(GameState game, Player player)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game.Board[i][j] is King && game.Board[i][j].Player == player)
                    {
                        return new Cell(i, j);
                    }
                }
            }
            return null;
        }
    }
}
