using Chess.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Config;
using Chess.Moves;
using Chess.Moves.Actions;

namespace Chess.Models.Pieces
{
    internal class Pawn : Piece
    {
        public Pawn() : base(Constants.TEXT_PAWN, "Pawn")
        {

        }

        public Pawn(Player player) : base(player, Constants.TEXT_PAWN, "Pawn")
        {
        }

        public override Piece Clone()
        {
            var newPiece = new Pawn(Player);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves(bool isActiveMove)
        {
            var possibleMoves = new List<Move>();
            if (Player == Player.Black)
            {
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 1, Position.Col), Helpers.GetCurrentGame(),
                    (r, c) => Helpers.GetCurrentGame().Board[r][c] is Empty))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row + 1, Position.Col), isActiveMove);
                }
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 1, Position.Col + 1), Helpers.GetCurrentGame(),
                    (r, c) => Helpers.GetCurrentGame().Board[r][c].Player == Helpers.GetOpposingPlayer(Player)))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row + 1, Position.Col + 1), isActiveMove);
                }
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 1, Position.Col - 1), Helpers.GetCurrentGame(),
                    (r, c) => Helpers.GetCurrentGame().Board[r][c].Player == Helpers.GetOpposingPlayer(Player)))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row + 1, Position.Col - 1), isActiveMove);
                }
                if (Position.Row == 1 && Helpers.GetCurrentGame().Board[Position.Row + 1][Position.Col] is Empty)
                {
                    PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 2, Position.Col), Helpers.GetCurrentGame(),
                        (r, c) => Helpers.GetCurrentGame().Board[r][c] is Empty);
                }

                // En passant
                if (Position.Row == 4)
                {
                    if (Position.Col - 1 >= 0
                        && Helpers.GetCurrentGame().Board[4][Position.Col - 1] is Pawn
                        && Helpers.GetCurrentGame().Board[4][Position.Col - 1].Player == Helpers.GetOpposingPlayer(Player)
                        && PieceMoveHelpers.CheckIfMovementWasDoneInLastMove(new Cell(6, Position.Col - 1), new Cell(4, Position.Col - 1), Helpers.GetCurrentGame()))
                    {
                        if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(5, Position.Col - 1), Helpers.GetCurrentGame(),
                            (r, c) => Helpers.GetCurrentGame().Board[r][c] is Empty))
                        {
                            PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new CapturePieceAction(new Cell(4, Position.Col - 1)));
                        }
                    }
                    if (Position.Col + 1 <= 7
                        && Helpers.GetCurrentGame().Board[4][Position.Col + 1] is Pawn
                        && Helpers.GetCurrentGame().Board[4][Position.Col + 1].Player == Helpers.GetOpposingPlayer(Player)
                        && PieceMoveHelpers.CheckIfMovementWasDoneInLastMove(new Cell(6, Position.Col + 1), new Cell(4, Position.Col + 1), Helpers.GetCurrentGame()))
                    {
                        if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(5, Position.Col + 1), Helpers.GetCurrentGame(),
                            (r, c) => Helpers.GetCurrentGame().Board[r][c] is Empty))
                        {
                            PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new CapturePieceAction(new Cell(4, Position.Col + 1)));
                        }
                    }
                }
            }
            else
            {
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 1, Position.Col), Helpers.GetCurrentGame(),
                    (r, c) => Helpers.GetCurrentGame().Board[r][c] is Empty))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row - 1, Position.Col), isActiveMove);
                }
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 1, Position.Col + 1), Helpers.GetCurrentGame(),
                    (r, c) => Helpers.GetCurrentGame().Board[r][c].Player == Helpers.GetOpposingPlayer(Player)))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row - 1, Position.Col + 1), isActiveMove);
                }
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 1, Position.Col - 1), Helpers.GetCurrentGame(),
                    (r, c) => Helpers.GetCurrentGame().Board[r][c].Player == Helpers.GetOpposingPlayer(Player)))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row - 1, Position.Col - 1), isActiveMove);
                }
                if (Position.Row == 6 && Helpers.GetCurrentGame().Board[Position.Row - 1][Position.Col] is Empty)
                {
                    PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 2, Position.Col), Helpers.GetCurrentGame(),
                        (r, c) => Helpers.GetCurrentGame().Board[r][c] is Empty);
                }
            }

            // En passant
            if (Position.Row == 3)
            {
                if (Position.Col - 1 >= 0
                    && Helpers.GetCurrentGame().Board[3][Position.Col - 1] is Pawn
                    && Helpers.GetCurrentGame().Board[3][Position.Col - 1].Player == Helpers.GetOpposingPlayer(Player)
                    && PieceMoveHelpers.CheckIfMovementWasDoneInLastMove(new Cell(1, Position.Col - 1), new Cell(3, Position.Col - 1), Helpers.GetCurrentGame()))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(2, Position.Col - 1), Helpers.GetCurrentGame(),
                        (r, c) => Helpers.GetCurrentGame().Board[r][c] is Empty))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new CapturePieceAction(new Cell(3, Position.Col - 1)));
                    }
                }
                if (Position.Col + 1 <= 7
                    && Helpers.GetCurrentGame().Board[3][Position.Col + 1] is Pawn
                    && Helpers.GetCurrentGame().Board[3][Position.Col + 1].Player == Helpers.GetOpposingPlayer(Player)
                    && PieceMoveHelpers.CheckIfMovementWasDoneInLastMove(new Cell(1, Position.Col + 1), new Cell(3, Position.Col + 1), Helpers.GetCurrentGame()))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(2, Position.Col + 1), Helpers.GetCurrentGame(),
                        (r, c) => Helpers.GetCurrentGame().Board[r][c] is Empty))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new CapturePieceAction(new Cell(3, Position.Col + 1)));
                    }
                }
            }

            return possibleMoves;
        }

        private void AddPromotePawnIfValid(List<Move> possibleMoves, Cell nextPosition, bool isActiveMove)
        {
            if (!isActiveMove)
            {
                return;
            }

            if (Player == Player.Black && Position.Row == 6
                || Player == Player.White && Position.Row == 1)
            {
                Move cloneRook = possibleMoves.Last().Clone();
                Move cloneBishop = possibleMoves.Last().Clone();
                Move cloneKnight = possibleMoves.Last().Clone();

                PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new PromotePawnAction(nextPosition, typeof(Queen)));

                possibleMoves.Add(cloneRook);
                PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new PromotePawnAction(nextPosition, typeof(Rook)));

                possibleMoves.Add(cloneBishop);
                PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new PromotePawnAction(nextPosition, typeof(Bishop)));

                possibleMoves.Add(cloneKnight);
                PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new PromotePawnAction(nextPosition, typeof(Knight)));
            }
        }
    }
}
