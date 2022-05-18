using Chess.Models;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Moves.Actions
{
    internal class PromotePawnAction : RevertableAction
    {
        public Cell Position { get; private set; }
        public Type PieceType { get; private set; }
        public Player Player { get; private set; }
        public PromotePawnAction(Cell position, Type type, GameState game) : base(game)
        {
            this.Position = position;
            PieceType = type;
        }

        public override void Execute()
        {
            Player = Game.Board[Position.Row, Position.Col].Player;
            Game.SetBoardEntry(Position, (Piece)Activator.CreateInstance(PieceType, Game, Player));
        }

        public override void Rollback()
        {
             Game.SetBoardEntry(Position, new Pawn(Game, Player));
        }

        public override RevertableAction Clone()
        {
            return new PromotePawnAction(Position.Clone(), PieceType, Game);
        }
    }
}
