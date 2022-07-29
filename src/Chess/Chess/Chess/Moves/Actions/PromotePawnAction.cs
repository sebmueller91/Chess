using Chess.Models;
using Chess.Models.Pieces;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Moves.Actions
{
    internal class PromotePawnAction : RevertableAction
    {
        public Cell Position { get; set; }
        public Type PieceType { get; set; }
        public Player Player { get; set; }
        public Piece OldPiece {get; set;}
        public PromotePawnAction(Cell position, Type type) : base("PromotePawnAction")
        {
            this.Position = position;
            PieceType = type;
        }

        public override void Execute()
        {
            Player = Helpers.GetCurrentGame().Board[Position.Row][Position.Col].Player;
            OldPiece = Helpers.GetCurrentGame().Board[Position.Row][Position.Col];
            Helpers.GetCurrentGame().SetBoardEntry(Position, (Piece)Activator.CreateInstance(PieceType, Player));
        }

        public override void Rollback()
        {
             Helpers.GetCurrentGame().SetBoardEntry(Position, OldPiece);
        }

        public override RevertableAction Clone()
        {
            var newAction = new PromotePawnAction(Position.Clone(), PieceType);
            newAction.Player = Player;
            newAction.OldPiece = OldPiece;
            return newAction;
        }
    }
}
