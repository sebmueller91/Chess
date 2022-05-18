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
        public Pawn(GameState game, Player player) : base(game, player, Constants.TEXT_PAWN)
        {
        }

        public override Piece Clone()
        {
            var newPiece = new Pawn(Game, Player);
            return this.CloneProperties(newPiece);
        }

        public override List<Move> GetPossibleMoves()
        {
            var possibleMoves = new List<Move>();
            if (Player == Player.Black)
            {
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 1, Position.Col), Game, Player,
                    (r, c) => Game.Board[r, c].Player == Player.None))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row + 1, Position.Col));
                }
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 1, Position.Col + 1), Game, Player,
                    (r, c) => Game.Board[r, c].Player == Helpers.GetOpposingPlayer(Player)))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row + 1, Position.Col + 1));
                }
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 1, Position.Col - 1), Game, Player,
                    (r, c) => Game.Board[r, c].Player == Helpers.GetOpposingPlayer(Player)))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row + 1, Position.Col - 1));

                }
                if (Position.Row == 1 && Game.Board[Position.Row + 1, Position.Col] is Empty)
                {
                    PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row + 2, Position.Col), Game, Player,
                        (r, c) => Game.Board[r, c].Player == Player.None);
                }

                // En passant
                if (Position.Row == 4)
                {
                    if (Position.Col - 1 >= 0
                        && Game.Board[4, Position.Col - 1] is Pawn
                        && Game.Board[4, Position.Col - 1].Player == Helpers.GetOpposingPlayer(Player)
                        && PieceMoveHelpers.CheckIfMovementWasDoneInLastMove(new Cell(6, Position.Col - 1), new Cell(4, Position.Col - 1), Game))
                    {
                        if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(5, Position.Col - 1), Game, Player))
                        {
                            PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new CapturePieceAction(new Cell(4, Position.Col - 1), Game));
                        }
                    }
                    if (Position.Col + 1 <= 7
                        && Game.Board[4, Position.Col + 1] is Pawn
                        && Game.Board[4, Position.Col + 1].Player == Helpers.GetOpposingPlayer(Player)
                        && PieceMoveHelpers.CheckIfMovementWasDoneInLastMove(new Cell(6, Position.Col + 1), new Cell(4, Position.Col + 1), Game))
                    {
                        if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(5, Position.Col + 1), Game, Player))
                        {
                            PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new CapturePieceAction(new Cell(4, Position.Col + 1), Game));
                        }
                    }
                }
            }
            else
            {
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 1, Position.Col), Game, Player,
                    (r, c) => Game.Board[r, c].Player == Player.None))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row - 1, Position.Col));
                }
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 1, Position.Col + 1), Game, Player,
                    (r, c) => Game.Board[r, c].Player == Helpers.GetOpposingPlayer(Player)))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row - 1, Position.Col + 1));
                }
                if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 1, Position.Col - 1), Game, Player,
                    (r, c) => Game.Board[r, c].Player == Helpers.GetOpposingPlayer(Player)))
                {
                    AddPromotePawnIfValid(possibleMoves, new Cell(Position.Row - 1, Position.Col - 1));
                }
                if (Position.Row == 6 && Game.Board[Position.Row - 1, Position.Col] is Empty)
                {
                    PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(Position.Row - 2, Position.Col), Game, Player,
                        (r, c) => Game.Board[r, c].Player == Player.None);
                }
            }

            // En passant
            if (Position.Row == 3)
            {
                if (Position.Col - 1 >= 0
                    && Game.Board[3, Position.Col - 1] is Pawn
                    && Game.Board[3, Position.Col - 1].Player == Helpers.GetOpposingPlayer(Player)
                    && PieceMoveHelpers.CheckIfMovementWasDoneInLastMove(new Cell(1, Position.Col - 1), new Cell(3, Position.Col - 1), Game))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(2, Position.Col - 1), Game, Player))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new CapturePieceAction(new Cell(3, Position.Col - 1), Game));
                    }
                }
                if (Position.Col + 1 <= 7
                    && Game.Board[3, Position.Col + 1] is Pawn
                    && Game.Board[3, Position.Col + 1].Player == Helpers.GetOpposingPlayer(Player)
                    && PieceMoveHelpers.CheckIfMovementWasDoneInLastMove(new Cell(1, Position.Col + 1), new Cell(3, Position.Col + 1), Game))
                {
                    if (PieceMoveHelpers.AddIfValid(possibleMoves, Position, new Cell(2, Position.Col + 1), Game, Player))
                    {
                        PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new CapturePieceAction(new Cell(3, Position.Col + 1), Game));
                    }
                }
            }

            return possibleMoves;
        }

        private void AddPromotePawnIfValid(List<Move> possibleMoves, Cell nextPosition)
        {
            if (Game.CurrentPlayer != Player)
            {
                return;
            }

            if (Player == Player.Black && Position.Row == 6
                || Player == Player.White && Position.Row == 1)
            {
                Move cloneRook = possibleMoves.Last().Clone();
                Move cloneBishop = possibleMoves.Last().Clone();
                Move cloneKnight = possibleMoves.Last().Clone();

                PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new PromotePawnAction(nextPosition, typeof(Queen), Game));

                possibleMoves.Add(cloneRook);
                PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new PromotePawnAction(nextPosition, typeof(Rook), Game));

                possibleMoves.Add(cloneBishop);
                PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new PromotePawnAction(nextPosition, typeof(Bishop), Game));

                possibleMoves.Add(cloneKnight);
                PieceMoveHelpers.AddActionToPreviousMove(possibleMoves, new PromotePawnAction(nextPosition, typeof(Knight), Game));
            }
        }
    }
}
